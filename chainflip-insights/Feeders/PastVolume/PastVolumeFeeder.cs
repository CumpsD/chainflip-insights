namespace ChainflipInsights.Feeders.PastVolume
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

    public class PastVolumeFeeder : IFeeder
    {
        private const string PastVolumeQuery = 
            """
            {
                allPoolSwaps(orderBy: ID_DESC, filter: {
                    assetSwappedBlockTimestamp: { 
                        greaterThanOrEqualTo: \"TIME_FROM\", lessThanOrEqualTo: \"TIME_TO\"
                        }
                    }) {
                    groupedAggregates(groupBy: [FROM_ASSET, TO_ASSET]) {
                        fromAssetToAsset: keys
                        sum {
                            toValueUsd
                            liquidityFeeValueUsd
                        }
                    }
                }
            }
            """;

        private readonly ILogger<PastVolumeFeeder> _logger;
        private readonly Pipeline<PastVolumeInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public PastVolumeFeeder(
            ILogger<PastVolumeFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<PastVolumeInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            if (!_configuration.EnablePastVolume.Value)
            {
                _logger.LogInformation(
                    "Past Volume not enabled. Skipping {TaskName}",
                    nameof(PastVolumeFeeder));
                
                return;
            }
            
            _logger.LogInformation(
                "Starting {TaskName}",
                nameof(PastVolumeFeeder));

            // Give the consumers some time to connect
            await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);
            
            // Start a loop fetching PastVolume Info
            await ProvidePastVolumeInfo(_pipeline.CancellationToken);
            
            _logger.LogInformation(
                "Stopping {TaskName}",
                nameof(PastVolumeFeeder));
        }

        private async Task ProvidePastVolumeInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastVolume = await GetLastPastVolume(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var yesterday = DateTime.UtcNow.Date.AddDays(-1);
                var dateString = yesterday.ToString("yyyy-MM-dd");
                if (dateString == lastVolume)
                {
                    _logger.LogInformation(
                        "No new Past Volume to announce. Last Volume Info is still {PastVolume}",
                        lastVolume);
                    
                    await Task.Delay(_configuration.PastVolumeQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var pastVolumeInfo = await GetPastVolume(
                    yesterday.AddDays(-1),
                    yesterday,
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (pastVolumeInfo == null)
                {
                    await Task.Delay(_configuration.PastVolumeQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var pastVolumePairs = pastVolumeInfo
                    .Data.Data.Data
                    .Select(x => new PastVolumePairInfo(x.Assets, x.Sum))
                    .ToArray();
                
                _logger.LogInformation(
                    "Broadcasting {TotalLastVolumePairs} Past 24h Volume Pairs",
                    pastVolumePairs.Length);

                var pastVolume = new PastVolumeInfo(lastVolume, pastVolumePairs);
                await _pipeline.Source.SendAsync(
                    pastVolume, 
                    cancellationToken);
                
                lastVolume = dateString;
                await StoreLastPastVolume(lastVolume);
                
                await Task.Delay(_configuration.PastVolumeQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastPastVolume(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastPastVolumeLocation))
                return await File.ReadAllTextAsync(_configuration.LastPastVolumeLocation, cancellationToken);

            var thePast = $"{DateTime.UtcNow.Date.AddDays(-2):yyyy-MM-dd}";
            
            await using var file = File.CreateText(_configuration.LastPastVolumeLocation);
            await file.WriteAsync(thePast);
            return thePast;
        }
        
        private async Task StoreLastPastVolume(string lastVolume)
        {
            await using var file = File.CreateText(_configuration.LastPastVolumeLocation);
            await file.WriteAsync(lastVolume.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<PastVolumeResponse?> GetPastVolume(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Graph");

            var query = PastVolumeQuery
                .Replace("TIME_FROM", timeFrom.ToString("yyyy-MM-ddT00:00:00.000Z"))
                .Replace("TIME_TO", timeTo.ToString("yyyy-MM-ddT00:00:00.000Z"));
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
                    .ReadFromJsonAsync<PastVolumeResponse>(cancellationToken: cancellationToken);
            }
            
            _logger.LogError(
                "GetPastVolume returned {StatusCode}: {Error}\nRequest: {Request}",
                response.StatusCode,
                await response.Content.ReadAsStringAsync(cancellationToken),
                graphQuery);

            return null;
        }
    }
}