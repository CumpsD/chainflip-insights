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

    public class OutgoingLiquidityFeeder : IFeeder
    {
        private const string OutgoingLiquidityQuery = 
            """
            {
                allLiquidityWithdrawals(orderBy: ID_DESC, first: 100, filter: {
                    id: { greaterThan: LAST_ID }
                 }) {
                    edges {
                        node {
                            id
                            amount
                            valueUsd
                            chain
                            asset
                            block: blockByBlockId {
                                id
                                timestamp
                            }        
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<OutgoingLiquidityFeeder> _logger;
        private readonly Pipeline<OutgoingLiquidityInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public OutgoingLiquidityFeeder(
            ILogger<OutgoingLiquidityFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<OutgoingLiquidityInfo> pipeline)
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
                        nameof(OutgoingLiquidityFeeder));

                    return;
                }

                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, 30000), _pipeline.CancellationToken);
                
                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(OutgoingLiquidityFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Liquidity Info
                await ProvideOutgoingLiquidityInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(OutgoingLiquidityFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(OutgoingLiquidityFeeder));
            }
        }

        private async Task ProvideOutgoingLiquidityInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastOutgoingLiquidityId(cancellationToken);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var outgoingLiquidityInfo = await GetOutgoingLiquidity(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (outgoingLiquidityInfo == null)
                {
                    await Task.Delay(_configuration.OutgoingLiquidityQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var outgoingLiquidity = outgoingLiquidityInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                if (outgoingLiquidity.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new outgoing liquidity to announce. Last outgoing liquidity is still {OutgoingLiquidityId}",
                        lastId);
                }
                
                // Outgoing liquidity is in increasing order
                foreach (var liquidity in outgoingLiquidity.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    var liquidityInfo = new OutgoingLiquidityInfo(liquidity);
                    
                    _logger.LogInformation(
                        "Broadcasting Outgoing Liquidity: {EgressAmount} {EgressTicker} (${EgressUsdAmount}) -> {ExplorerUrl}",
                        liquidityInfo.WithdrawalAmountFormatted,
                        liquidityInfo.SourceAsset,
                        liquidityInfo.WithdrawalValueUsdFormatted,
                        $"{_configuration.ExplorerBlocksUrl}{liquidityInfo.BlockId}");
                    
                    await _pipeline.Source.SendAsync(
                        liquidityInfo, 
                        cancellationToken);
                   
                    lastId = liquidityInfo.Id;
                    await StoreLastOutgoingLiquidityId(lastId);
                }
                
                await Task.Delay(_configuration.OutgoingLiquidityQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastOutgoingLiquidityId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastOutgoingLiquidityIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastOutgoingLiquidityIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastOutgoingLiquidityIdLocation);
            await file.WriteAsync("0");
            return 0;
        }
        
        private async Task StoreLastOutgoingLiquidityId(double outgoingLiquidityId)
        {
            await using var file = File.CreateText(_configuration.LastOutgoingLiquidityIdLocation);
            await file.WriteAsync(outgoingLiquidityId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<OutgoingLiquidityResponse?> GetOutgoingLiquidity(
            double fromId,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = OutgoingLiquidityQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
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
                        .ReadFromJsonAsync<OutgoingLiquidityResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetOutgoingLiquidity returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching outgoing liquidity failed.");
            }

            return null;
        }
    }
}