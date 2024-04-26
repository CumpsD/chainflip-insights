namespace ChainflipInsights.Feeders.Burn
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Substrate.NetApi.Model.Extrinsics;
    using global::Substrate.NetApiExt.Generated;
    using global::Substrate.NetApiExt.Generated.Model.pallet_cf_emissions.pallet;
    using global::Substrate.NetApiExt.Generated.Model.state_chain_runtime;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class BurnFeeder : IFeeder
    {
        private const string BlockHashQuery = 
            """
            {
                "jsonrpc": "2.0",
                "id": "1",
                "method": "chain_getBlockHash",
                "params": [BLOCK_NUMBER]
            }
            """;
        
        private readonly ILogger<BurnFeeder> _logger;
        private readonly Pipeline<BurnInfo> _pipeline;
        private readonly PriceProvider _priceProvider;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public BurnFeeder(
            ILogger<BurnFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<BurnInfo> pipeline,
            PriceProvider priceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _priceProvider = priceProvider ?? throw new ArgumentNullException(nameof(priceProvider));
        }
        
        public async Task Start()
        {
            try
            {
                if (!_configuration.EnableBurn.Value)
                {
                    _logger.LogInformation(
                        "Burn not enabled. Skipping {TaskName}",
                        nameof(BurnFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(BurnFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Burn Info
                await ProvideBurnInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(BurnFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(BurnFeeder));
            }
        }
        
        private async Task ProvideBurnInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastBurn = await GetLastBurn(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var substrateInfo = await GetSubstrateInfo(lastBurn, cancellationToken);
                if (substrateInfo == null)
                {
                    await Task.Delay(_configuration.BurnQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }

                if (substrateInfo.LastSupplyUpdateBlock == lastBurn)
                {
                    _logger.LogInformation(
                        "No new burn to announce. Last burn is still {LastBurnBlock}",
                        lastBurn);
                    
                    await Task.Delay(_configuration.BurnQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }

                if (substrateInfo.BurnSkipped)
                {
                    _logger.LogInformation(
                        "Broadcasting Burn {BurnBlock} ({BurnBlockHash}): Burn Skipped, {FlipToBurn} FLIP waiting",
                        substrateInfo.LastSupplyUpdateBlock,
                        substrateInfo.LastSupplyUpdateBlockHash,
                        substrateInfo.FlipToBurnFormatted);
                }
                else
                {
                    _logger.LogInformation(
                        "Broadcasting Burn {BurnBlock} ({BurnBlockHash}): {FlipBurned} FLIP",
                        substrateInfo.LastSupplyUpdateBlock,
                        substrateInfo.LastSupplyUpdateBlockHash,
                        substrateInfo.FlipBurnedFormatted);
                }

                await _pipeline.Source.SendAsync(
                    substrateInfo, 
                    cancellationToken);

                lastBurn = substrateInfo.LastSupplyUpdateBlock;
                await StoreLastBurn(lastBurn);
                
                await Task.Delay(_configuration.BurnQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }

        private async Task<BurnInfo?> GetSubstrateInfo(
            ulong lastBurn, 
            CancellationToken cancellationToken)
        {
            BurnInfo? substrateInfo = null;
            
            try
            {
                var client = new SubstrateClientExt(
                    new Uri(_configuration.SubstrateEndpoint),
                    ChargeTransactionPayment.Default());
                    
                await client.ConnectAsync(cancellationToken);

                if (client.IsConnected)
                {
                    substrateInfo = await FetchSubstrateInfo(
                        client, 
                        lastBurn,
                        cancellationToken);
                        
                    await client.CloseAsync(cancellationToken);
                }
                else
                {
                    _logger.LogError(
                        "Something went wrong connecting to substrate");
                }
                    
                client.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong fetching substrate info");
            }

            return substrateInfo;
        }

        private async Task<BurnInfo> FetchSubstrateInfo(
            SubstrateClientExt client,
            ulong lastBurn,
            CancellationToken cancellationToken)
        {
            // var swappingEnabled = await client.AccountRolesStorage.SwappingEnabled(null, cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] Swapping is {SwappingEnabled}",
            //     swappingEnabled.Value ? "enabled" : "NOT enabled");
                
            // var supplyUpdateInterval = await client.EmissionsStorage.SupplyUpdateInterval(null, cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] Supply updates every {SupplyUpdateInterval} blocks",
            //     supplyUpdateInterval.Value);

            // var cfeVersion = await client.EnvironmentStorage.CurrentReleaseVersion(null, cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] Current active CFE version is {Major}.{Minor}.{Patch}",
            //     cfeVersion.Major.Value,
            //     cfeVersion.Minor.Value,
            //     cfeVersion.Patch.Value);

            // var totalIssuance = await client.FlipStorage.TotalIssuance(null, cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] There is currently {TotalIssuance} FLIP issued",
            //     UnitConversion.Convert.FromWei(totalIssuance.Value));

            // TODO: Regenerate substrate and get flip to burn from swapping storage
            var flipToBurn = new double?(0); // await client.LiquidityPoolsStorage.FlipToBurn(null, cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] There is currently {FlipToBurn} FLIP waiting to be burned",
            //     UnitConversion.Convert.FromWei(flipToBurn.Value));

            // var networkFee = new LiquidityPoolsConstants().NetworkFee();
            // _logger.LogInformation(
            //     "[Substrate] The current network fee is {NetworkFee}%",
            //     Convert.ToDouble(networkFee.Value.Value) / 10000);
            
            var lastSupplyUpdateBlock = await client.EmissionsStorage.LastSupplyUpdateBlock(null, cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] Supply was last updated at block {LastSupplyUpdateBlock}",
            //     lastSupplyUpdateBlock.Value);

            if (lastBurn == lastSupplyUpdateBlock.Value)
                return new BurnInfo(lastBurn);
            
            var lastSupplyUpdateBlockHash = await GetBlockHash(lastSupplyUpdateBlock.Value, cancellationToken);
            var lastBurnBlockHash = lastSupplyUpdateBlockHash.Result;
            // _logger.LogInformation(
            //     "[Substrate] Supply was last updated at block hash {LastSupplyUpdateBlockHash}",
            //     lastBurnBlockHash);
            
            var lastBurnBlock = await client.SystemStorage.Events(
                lastBurnBlockHash,
                cancellationToken);
            
            var burnEmissions = lastBurnBlock
                .Value
                .FirstOrDefault(x =>
                    x.Event.Value == RuntimeEvent.Emissions && 
                    ((EnumEvent)x.Event.Value2).Value == Event.NetworkFeeBurned);

            double? flipBurned = null;
            bool? burnSkipped;
            if (burnEmissions != null)
            {
                var burnEvent = (EnumEvent)burnEmissions.Event.Value2;
                var burnData = (Substrate.NetApi.Model.Types.Base.BaseTuple<
                    Substrate.NetApi.Model.Types.Primitive.U128,
                    Substrate.NetApi.Model.Types.Base.BaseTuple<
                        Substrate.NetApiExt.Generated.Model.cf_primitives.chains.EnumForeignChain,
                        Substrate.NetApi.Model.Types.Primitive.U64>>)burnEvent.Value2;

                flipBurned = Convert.ToDouble(burnData.Value.First().ToString());
                burnSkipped = false;
            }
            else
            {
                burnSkipped = lastBurnBlock
                    .Value
                    .Any(x =>
                        x.Event.Value == RuntimeEvent.Emissions && 
                        ((EnumEvent)x.Event.Value2).Value == Event.FlipBurnSkipped);
            }
            
            // _logger.LogInformation(
            //     "[Substrate] Last burn amount was {BurnAmount} FLIP",
            //     Math.Round(flipBurned, 8).ToString(flip.FormatString));

            return new BurnInfo(
                _priceProvider,
                lastSupplyUpdateBlock.Value,
                lastBurnBlockHash,
                Convert.ToDouble(flipToBurn.Value.ToString()),
                flipBurned,
                burnSkipped.Value);
        }
        
        private async Task<ulong> GetLastBurn(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastBurnLocation))
                return Convert.ToUInt64(await File.ReadAllTextAsync(_configuration.LastBurnLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastBurnLocation);
            await file.WriteAsync("1");
            return 1;
        }
        
        private async Task StoreLastBurn(ulong lastBurn)
        {
            await using var file = File.CreateText(_configuration.LastBurnLocation);
            await file.WriteAsync(lastBurn.ToString());
        }
        
        private async Task<BlockHashResponse?> GetBlockHash(
            ulong blockNumber,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Rpc");

            var rpcQuery = BlockHashQuery.Replace("BLOCK_NUMBER", blockNumber.ToString());
            
            var response = await client.PostAsync(
                string.Empty,
                new StringContent(
                    rpcQuery, 
                    new MediaTypeHeaderValue(MediaTypeNames.Application.Json)), 
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response
                    .Content
                    .ReadFromJsonAsync<BlockHashResponse>(cancellationToken: cancellationToken);
            }
            
            _logger.LogError(
                "GetBlockHash returned {StatusCode}: {Error}\nRequest: {Request}",
                response.StatusCode,
                await response.Content.ReadAsStringAsync(cancellationToken),
                rpcQuery);

            return null;
        }
    }
}