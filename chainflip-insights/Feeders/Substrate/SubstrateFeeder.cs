namespace ChainflipInsights.Feeders.Substrate
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Substrate.NetApi;
    using global::Substrate.NetApi.Model.Extrinsics;
    using global::Substrate.NetApiExt.Generated;
    using global::Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.any;
    using global::Substrate.NetApiExt.Generated.Storage;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Nethereum.Util;

    public class SubstrateFeeder : IFeeder
    {
        private readonly ILogger<SubstrateFeeder> _logger;
        private readonly Pipeline<SubstrateInfo> _pipeline;
        private readonly BotConfiguration _configuration;

        public SubstrateFeeder(
            ILogger<SubstrateFeeder> logger,
            IOptions<BotConfiguration> options,
            Pipeline<SubstrateInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            try
            {
                if (!_configuration.EnableSubstrate.Value)
                {
                    _logger.LogInformation(
                        "Substrate not enabled. Skipping {TaskName}",
                        nameof(SubstrateFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(SubstrateFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Substrate Info
                await ProvideSubstrateInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(SubstrateFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(SubstrateFeeder));
            }
        }
        
        private async Task ProvideSubstrateInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                try
                {
                    var client = new SubstrateClientExt(
                        new Uri(_configuration.SubstrateEndpoint),
                        ChargeTransactionPayment.Default());
                    
                    await client.ConnectAsync(cancellationToken);

                    if (client.IsConnected)
                    {
                        await FetchSubstrateInfo(client, cancellationToken);
                        
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

                await Task.Delay(_configuration.SubstrateQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }

        private async Task FetchSubstrateInfo(
            SubstrateClientExt client, 
            CancellationToken cancellationToken)
        {
            var swappingEnabled = await client.AccountRolesStorage.SwappingEnabled(null, cancellationToken);
            _logger.LogInformation(
                "[Substrate] Swapping is {SwappingEnabled}",
                swappingEnabled.Value ? "enabled" :"NOT enabled");
            
            var lastSupplyUpdateBlock = await client.EmissionsStorage.LastSupplyUpdateBlock(null, cancellationToken);
            _logger.LogInformation(
                "[Substrate] Supply was last updated at block {LastSupplyUpdateBlock}",
                lastSupplyUpdateBlock.Value);
            
            var lastSupplyUpdateBlockHash = await client.SystemStorage.BlockHash(lastSupplyUpdateBlock, null, cancellationToken);
            var lastBurnBlockHash = Utils.Bytes2HexString(lastSupplyUpdateBlockHash.Bytes).ToLowerInvariant();
            _logger.LogInformation(
                "[Substrate] Supply was last updated at block hash {LastSupplyUpdateBlockHash}",
                lastBurnBlockHash);
            
            var supplyUpdateInterval = await client.EmissionsStorage.SupplyUpdateInterval(null, cancellationToken);
            _logger.LogInformation(
                "[Substrate] Supply updates every {SupplyUpdateInterval} blocks",
                supplyUpdateInterval.Value);

            var cfeVersion = await client.EnvironmentStorage.CurrentReleaseVersion(null, cancellationToken);
            _logger.LogInformation(
                "[Substrate] Current active CFE version is {Major}.{Minor}.{Patch}",
                cfeVersion.Major.Value,
                cfeVersion.Minor.Value,
                cfeVersion.Patch.Value);

            var totalIssuance = await client.FlipStorage.TotalIssuance(null, cancellationToken);
            _logger.LogInformation(
                "[Substrate] There is currently {TotalIssuance} FLIP issued",
                UnitConversion.Convert.FromWei(totalIssuance.Value));

            var flipToBurn = await client.LiquidityPoolsStorage.FlipToBurn(null, cancellationToken);
            _logger.LogInformation(
                "[Substrate] There is currently {FlipToBurn} FLIP waiting to be burned",
                UnitConversion.Convert.FromWei(flipToBurn.Value));

            var networkFee = new LiquidityPoolsConstants().NetworkFee();
            _logger.LogInformation(
                "[Substrate] The current network fee is {NetworkFee}%",
                Convert.ToDouble(networkFee.Value.Value) / 10000);

            var lastBurnBlock = await client.SystemStorage.Events(
                lastBurnBlockHash,
                cancellationToken);
            // _logger.LogInformation(
            //     "[Substrate] Last burn amount was {BurnAmount}",
            //     lastBurnBlock
            //         .Value
            //         .First(x => x.Event.ToString()));
        }
    }
}