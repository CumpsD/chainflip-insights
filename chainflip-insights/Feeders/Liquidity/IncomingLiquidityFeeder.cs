namespace ChainflipInsights.Feeders.Liquidity
{
    using System;
    using System.Globalization;
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
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class IncomingLiquidityFeeder : IFeeder
    {
        private const string IncomingLiquidityQuery = 
            """
            {
                allLiquidityDeposits(orderBy: ID_ASC, first: 500, filter: {
                    id: { greaterThan: LAST_ID }
                 }) {
                    edges {
                        node {
                            id
                            depositAmount
                            depositValueUsd
                            channel: liquidityDepositChannelByLiquidityDepositChannelId {
                                issuedBlockId
                                chain
                                asset
                                channelId
                                depositAddress
                                isExpired
                            }
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<IncomingLiquidityFeeder> _logger;
        private readonly Pipeline<IncomingLiquidityInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public IncomingLiquidityFeeder(
            ILogger<IncomingLiquidityFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<IncomingLiquidityInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            try
            {
                if (!_configuration.EnableLiquidity.Value)
                {
                    _logger.LogInformation(
                        "Liquidity not enabled. Skipping {TaskName}",
                        nameof(IncomingLiquidityFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(IncomingLiquidityFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Liquidity Info
                await ProvideIncomingLiquidityInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(IncomingLiquidityFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(IncomingLiquidityFeeder));
            }
        }

        private async Task ProvideIncomingLiquidityInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastIncomingLiquidityId(cancellationToken);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var incomingLiquidityInfo = await GetIncomingLiquidity(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (incomingLiquidityInfo == null)
                {
                    await Task.Delay(_configuration.IncomingLiquidityQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var incomingLiquidity = incomingLiquidityInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                if (incomingLiquidity.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new incoming liquidity to announce. Last incoming liquidity is still {IncomingLiquidityId}",
                        lastId);
                }
                
                // Incoming liquidity is in increasing order
                foreach (var liquidity in incomingLiquidity.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    var liquidityInfo = new IncomingLiquidityInfo(liquidity);
                    
                    _logger.LogInformation(
                        "Broadcasting Incoming Liquidity: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                        liquidityInfo.DepositAmountFormatted,
                        liquidityInfo.SourceAsset,
                        liquidityInfo.DepositValueUsdFormatted,
                        $"{_configuration.ExplorerLiquidityChannelUrl}{liquidityInfo.BlockId}-{liquidityInfo.Network}-{liquidityInfo.ChannelId}");
                    
                    await _pipeline.Source.SendAsync(
                        liquidityInfo, 
                        cancellationToken);
                   
                    lastId = liquidityInfo.Id;
                    await StoreLastIncomingLiquidityId(lastId);
                }
                
                await Task.Delay(_configuration.IncomingLiquidityQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastIncomingLiquidityId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastIncomingLiquidityIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastIncomingLiquidityIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastIncomingLiquidityIdLocation);
            await file.WriteAsync("118");
            return 118;
        }
        
        private async Task StoreLastIncomingLiquidityId(double incomingLiquidityId)
        {
            await using var file = File.CreateText(_configuration.LastIncomingLiquidityIdLocation);
            await file.WriteAsync(incomingLiquidityId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<IncomingLiquidityResponse?> GetIncomingLiquidity(
            double fromId,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = IncomingLiquidityQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
                var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";

                var response = await client.PostAsync(
                    string.Empty,
                    new StringContent(
                        graphQuery,
                        new MediaTypeHeaderValue(MediaTypeNames.Application.Json)),
                    cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<IncomingLiquidityResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetIncomingLiquidity returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching incoming liquidity failed.");
            }

            return null;
        }
    }
}