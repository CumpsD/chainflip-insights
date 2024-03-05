namespace ChainflipInsights.Feeders.SwapLimits
{
    using System;
    using System.Collections.Generic;
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

    public class SwapLimitsFeeder : IFeeder
    {
        private const string SwapLimitQuery = 
            """
            {
                "jsonrpc": "2.0",
                "id": "1",
                "method": "cf_max_swap_amount",
                "params": [
                    "ASSET"
                ]
            }
            """;
        
        private readonly ILogger<SwapLimitsFeeder> _logger;
        private readonly Pipeline<SwapLimitsInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SwapLimitsFeeder(
            ILogger<SwapLimitsFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<SwapLimitsInfo> pipeline)
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
                if (!_configuration.EnableSwapLimits.Value)
                {
                    _logger.LogInformation(
                        "SwapLimits not enabled. Skipping {TaskName}",
                        nameof(SwapLimitsFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(SwapLimitsFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching SwapLimits Info
                await ProvideSwapLimitsInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(SwapLimitsFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(SwapLimitsFeeder));
            }
        }

        private async Task ProvideSwapLimitsInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastLimits = await GetLastSwapLimits(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var ethLimits = await GetSwapLimit(Constants.SupportedAssets[Constants.ETH], cancellationToken);
                var btcLimits = await GetSwapLimit(Constants.SupportedAssets[Constants.BTC], cancellationToken);
                var dotLimits = await GetSwapLimit(Constants.SupportedAssets[Constants.DOT], cancellationToken);
                var flipLimits = await GetSwapLimit(Constants.SupportedAssets[Constants.FLIP], cancellationToken);
                var usdcLimits = await GetSwapLimit(Constants.SupportedAssets[Constants.USDC], cancellationToken);

                var limits = new List<SwapLimitInfo>
                {
                    new(ethLimits, Constants.SupportedAssets[Constants.ETH]),
                    new(btcLimits, Constants.SupportedAssets[Constants.BTC]),
                    new(dotLimits, Constants.SupportedAssets[Constants.DOT]),
                    new(flipLimits, Constants.SupportedAssets[Constants.FLIP]),
                    new(usdcLimits, Constants.SupportedAssets[Constants.USDC]),
                };

                var swapLimits = new SwapLimitsInfo(limits.ToArray());
                var swapLimitsString = string.Join(", ", limits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}"));
                if (swapLimitsString == lastLimits)
                {
                    _logger.LogInformation(
                        "No new swap limits to announce. Last limits are still {SwapLimits}",
                        string.Join(", ", limits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                    
                    await Task.Delay(_configuration.CfeVersionQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                _logger.LogInformation(
                    "Broadcasting Swap Limits: {Limits}",
                    string.Join(", ", limits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                await _pipeline.Source.SendAsync(
                    swapLimits, 
                    cancellationToken);

                lastLimits = swapLimitsString;
                await StoreLastSwapLimits(lastLimits);
                
                await Task.Delay(_configuration.CfeVersionQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastSwapLimits(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastSwapLimitsLocation))
                return await File.ReadAllTextAsync(_configuration.LastSwapLimitsLocation, cancellationToken);
            
            await using var file = File.CreateText(_configuration.LastSwapLimitsLocation);
            await file.WriteAsync("ETH: 22.00, BTC: 1.20, DOT: 7500.00, FLIP: 10000.00, USDC: 50000.00");
            return "ETH: 22.00, BTC: 1.20, DOT: 7500.00, FLIP: 10000.00, USDC: 50000.00";
        }
        
        private async Task StoreLastSwapLimits(string swapLimits)
        {
            await using var file = File.CreateText(_configuration.LastSwapLimitsLocation);
            await file.WriteAsync(swapLimits);
        }
        
        private async Task<SwapLimitResponse?> GetSwapLimit(
            AssetInfo asset,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Rpc");

            var rpcQuery = SwapLimitQuery.Replace("ASSET", asset.Ticker);
            
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
                    .ReadFromJsonAsync<SwapLimitResponse>(cancellationToken: cancellationToken);
            }
            
            _logger.LogError(
                "GetSwapLimit returned {StatusCode}: {Error}\nRequest: {Request}",
                response.StatusCode,
                await response.Content.ReadAsStringAsync(cancellationToken),
                rpcQuery);

            return null;
        }
    }
}