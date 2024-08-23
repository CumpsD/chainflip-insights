namespace ChainflipInsights.Feeders.Swap
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text.Json;
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
                allSwaps(orderBy: ID_ASC, first: NUMBER_OF_RESULTS, filter: {
                    id: { greaterThan: LAST_ID }
                 }) {
                    edges {
                        node {
                            id
                            nativeId
                            type
                            swapScheduledBlockTimestamp
            
                            depositAmount
                            depositValueUsd
                            sourceAsset
                            sourceChain
            
                            egressAmount
                            egressValueUsd
                            destinationAsset
                            destinationChain
                            destinationAddress
            
                            intermediateAmount
                            intermediateValueUsd
                            
                            swapInputAmount
                            swapInputValueUsd
                            swapOutputAmount
                            swapOutputValueUsd
                            
                            swapChannelByDepositChannelId {
                                swapChannelBeneficiariesByDepositChannelId {
                                    nodes {
                                        brokerCommissionRateBps
                                        type
                                        brokerByBrokerId {
                                            accountByAccountId {
                                                idSs58
                                            }
                                        }
                                    }
                                }
                                brokerByBrokerId {
                                    accountByAccountId {
                                        idSs58
                                    }
                                }
                            }
                            
                            effectiveBoostFeeBps
                            
                            swapFeesBySwapId {
                                edges {
                                    node {
                                        type
                                        amount
                                        asset
                                        valueUsd
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
            try
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

                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, _configuration.StartupDelay.Value), _pipeline.CancellationToken);
                
                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Swap Info
                await ProvideSwapInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(SwapFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(SwapFeeder));
            }
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
                
                var swapsInfo = await GetSwaps(lastId, 100, cancellationToken);
                
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

                if (!swapsInfo.ContainsResponse)
                {
                    lastId++;
                    await StoreLastSwapId(lastId);
                    continue;
                }
                
                List<SwapsResponseNode> swaps;
                try
                {
                    swaps = swapsInfo
                        .SwapsResponse!
                        .Data.Data.Data
                        .Select(x => x.Data)
                        .Where(x => x.SwapType != "GAS")
                        .OrderBy(x => x.Id)
                        .ToList();
                }
                catch (Exception e)
                {
                    _logger.LogError(
                        e,
                        "Error on fetching swaps, retrying");
                    
                    await Task.Delay(_configuration.SwapQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;  
                }

                if (swaps.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new swaps to announce. Last swap is still {SwapId}",
                        lastId);
                    
                    await Task.Delay(_configuration.SwapQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                _logger.LogInformation(
                    "{NewSwaps} new swaps to announce.",
                    swaps.Count);
                
                // Swaps are in increasing order
                foreach (var swap in swaps.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    try
                    {
                        var swapInfo = new SwapInfo(swap);

                        _logger.LogInformation(
                            "Broadcasting Swap {SwapId}: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) to {EgressAmount} {EgressTicker} (${EgressUsdAmount}) @ {Broker} -> {ExplorerUrl}",
                            swap.Id,
                            swapInfo.DepositAmountFormatted,
                            swapInfo.SourceAsset,
                            swapInfo.DepositValueUsdFormatted,
                            swapInfo.EgressAmountFormatted,
                            swapInfo.DestinationAsset,
                            swapInfo.EgressValueUsdFormatted,
                            swapInfo.Broker ?? "n/a",
                            $"{_configuration.ExplorerSwapsUrl}{swapInfo.Id}");

                        await _pipeline.Source.SendAsync(
                            swapInfo,
                            cancellationToken);

                        lastId = swap.Id;
                        await StoreLastSwapId(lastId);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(
                            e,
                            "Error on broadcasting Swap {SwapId}",
                            swap.Id);
                    }
                }
                
                await Task.Delay(swaps.Count == 1 ? 1000 : _configuration.SwapQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastSwapId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastSwapIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastSwapIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastSwapIdLocation);
            await file.WriteAsync("0");
            return 0;
        }

        private async Task StoreLastSwapId(double swapId)
        {
            await using var file = File.CreateText(_configuration.LastSwapIdLocation);
            await file.WriteAsync(swapId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<SwapsResponseWrapper?> GetSwaps(
            double fromId,
            int numberOfResults,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = SwapsQuery
                    .Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture))
                    .Replace("NUMBER_OF_RESULTS", numberOfResults.ToString());
                
                var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";

                var response = await client.PostAsync(
                    string.Empty,
                    new StringContent(
                        graphQuery,
                        new MediaTypeHeaderValue(MediaTypeNames.Application.Json)),
                    cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    // var json = await response.Content.ReadAsStringAsync(cancellationToken);
                    // return JsonSerializer.Deserialize<SwapsResponse>(json);

                    return new SwapsResponseWrapper
                    {
                        ContainsResponse = true,
                        SwapsResponse = await response
                            .Content
                            .ReadFromJsonAsync<SwapsResponse>(cancellationToken: cancellationToken)
                    };
                }

                _logger.LogError(
                    "GetSwaps returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (JsonException e)
            {
                // The JSON value could not be converted to System.Double. Path: $.data.allSwaps.edges[0].node.swapFeesBySwapId.edges[0].node.valueUsd
                if (!e.Message.Contains("JSON value could not be converted to") || !e.Message.Contains("valueUsd"))
                {
                    _logger.LogError(
                        e,
                        "Fetching swaps failed.");

                    return null;
                }

                if (numberOfResults == 1)
                {
                    _logger.LogError(
                        e,
                        "Swap {SwapId} contains missing USD values.",
                        fromId);
                    
                    return null;
                }
                
                _logger.LogError(
                    e,
                    "There was a swap with missing USD values.");

                // There is a broken swap in the response where the USD values are missing.
                // Let's just grab them 1 by one till we hit the faulty one
                var singleSwap = await GetSwaps(fromId, 1, cancellationToken);

                // This is the broken one!
                if (singleSwap == null)
                {
                    return new SwapsResponseWrapper
                    {
                        ContainsResponse = false
                    };
                }

                return singleSwap;
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