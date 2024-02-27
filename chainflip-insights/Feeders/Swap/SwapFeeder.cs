namespace ChainflipInsights.Feeders.Swap
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

    public class SwapFeeder : IFeeder
    {
        private const string SwapsQuery = 
            """
            {
                allSwaps(orderBy: ID_ASC, first: 500, filter: {
                    id: { greaterThan: LAST_ID }
                 }) {
                    edges {
                        node {
                            id
                            nativeId
                            swapScheduledBlockTimestamp
            
                            depositAmount
                            depositValueUsd
                            sourceAsset
            
                            egressAmount
                            egressValueUsd
                            destinationAsset
                            destinationAddress
            
                            intermediateAmount
                            intermediateValueUsd
                            
                            swapChannelByDepositChannelId {
                                brokerByBrokerId {
                                    accountByAccountId {
                                        idSs58
                                    }
                                }
                            }
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<SwapFeeder> _logger;
        private readonly Pipeline<SwapInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SwapFeeder(
            ILogger<SwapFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<SwapInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            if (!_configuration.EnableSwaps.Value)
            {
                _logger.LogInformation(
                    "Swaps not enabled. Skipping {TaskName}",
                    nameof(SwapFeeder));
                
                return;
            }
            
            _logger.LogInformation(
                "Starting {TaskName}",
                nameof(SwapFeeder));

            // Give the consumers some time to connect
            await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);
            
            // Start a loop fetching Swap Info
            await ProvideSwapInfo(_pipeline.CancellationToken);
            
            _logger.LogInformation(
                "Stopping {TaskName}",
                nameof(SwapFeeder));
        }
        
        private async Task ProvideSwapInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastSwapId(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var swapsInfo = await GetSwaps(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (swapsInfo == null)
                {
                    _logger.LogInformation(
                        "Fetching swaps failed. Last swap is still {SwapId}",
                        lastId);
                    
                    await Task.Delay(_configuration.SwapQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var swaps = swapsInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                if (swaps.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new swaps to announce. Last swap is still {SwapId}",
                        lastId);
                    
                    await Task.Delay(_configuration.SwapQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                // Swaps are in increasing order
                foreach (var swap in swaps.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    var swapInfo = new SwapInfo(swap);
                    
                    _logger.LogInformation(
                        "Broadcasting Swap: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) to {EgressAmount} {EgressTicker} (${EgressUsdAmount}) @ {Broker} -> {ExplorerUrl}",
                        swapInfo.DepositAmountFormatted,
                        swapInfo.SourceAsset,
                        swapInfo.DepositValueUsdFormatted,
                        swapInfo.EgressAmountFormatted,
                        swapInfo.DestinationAsset,
                        swapInfo.EgressValueUsdFormatted,
                        swapInfo.Broker,
                        $"{_configuration.ExplorerSwapsUrl}{swapInfo.Id}");
                    
                    await _pipeline.Source.SendAsync(
                        swapInfo, 
                        cancellationToken);
                   
                    lastId = swap.Id;
                    await StoreLastSwapId(lastId);
                }
                
                await Task.Delay(_configuration.SwapQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastSwapId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastSwapIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastSwapIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastSwapIdLocation);
            await file.WriteAsync("462");
            return 462;
        }

        private async Task StoreLastSwapId(double swapId)
        {
            await using var file = File.CreateText(_configuration.LastSwapIdLocation);
            await file.WriteAsync(swapId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<SwapsResponse?> GetSwaps(
            double fromId,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = SwapsQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
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
                        .ReadFromJsonAsync<SwapsResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetSwaps returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching swaps failed.");
            }
            
            return null;
        }

        private async Task<SwapResponse?> GetSwap(
            double swapId,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Swap");
            
            return await client.GetFromJsonAsync<SwapResponse>(
                $"swaps/{swapId}", 
                cancellationToken: cancellationToken);
        }
    }
}