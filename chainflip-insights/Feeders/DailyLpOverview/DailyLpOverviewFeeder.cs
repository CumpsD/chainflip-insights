namespace ChainflipInsights.Feeders.DailyLpOverview
{
    using System;
    using System.Globalization;
    using System.IO;
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

    public class DailyLpOverviewFeeder : IFeeder
    {
            private const string LpQuery = 
            """
            {
                allAccounts {
                    nodes {
                        idSs58
                        limitOrders: limitOrderFillsByLiquidityProviderId(
                            filter: {
                                blockTimestamp: {
                                    greaterThanOrEqualTo: \"TIME_FROM\", 
                                    lessThan: \"TIME_TO\"
                                }
                            }
                        ) {
                            groupedAggregates(groupBy: BLOCK_TIMESTAMP_TRUNCATED_TO_DAY) {
                                sum {
                                    feesEarnedValueUsd
                                    filledAmountValueUsd
                                }
                                keys
                            }
                        }
                        rangeOrders: rangeOrderFillsByLiquidityProviderId(
                            filter: {
                                blockTimestamp: {
                                    greaterThanOrEqualTo: \"TIME_FROM\", 
                                    lessThan: \"TIME_TO\"
                                }
                            }
                        ) {
                            groupedAggregates(groupBy: BLOCK_TIMESTAMP_TRUNCATED_TO_DAY) {
                                sum {
                                    quoteFeesEarnedValueUsd
                                    quoteFilledAmountValueUsd
                                    baseFeesEarnedValueUsd
                                    baseFilledAmountValueUsd
                                }
                                keys
                            }
                        }
                    }
                }
            }
            """;
                
        private readonly ILogger<DailyLpOverviewFeeder> _logger;
        private readonly Pipeline<DailyLpOverviewInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public DailyLpOverviewFeeder(
            ILogger<DailyLpOverviewFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<DailyLpOverviewInfo> pipeline)
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
                if (!_configuration.EnableDailyLpOverview.Value)
                {
                    _logger.LogInformation(
                        "Daily Lp Overview not enabled. Skipping {TaskName}",
                        nameof(DailyLpOverviewFeeder));

                    return;
                }
                
                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, _configuration.StartupDelay.Value), _pipeline.CancellationToken);

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(DailyLpOverviewFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching DailyLpOverview Info
                await ProvideDailyLpOverviewInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(DailyLpOverviewFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(DailyLpOverviewFeeder));
            }
        }

        private async Task ProvideDailyLpOverviewInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastDailyLpOverview = await GetLastDailyLpOverview(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var yesterday = DateTime.UtcNow.Date.AddDays(-1);
                var dateString = yesterday.ToString("yyyy-MM-dd");
                if (dateString == lastDailyLpOverview)
                {
                    _logger.LogInformation(
                        "No new daily LP overview to announce. Last daily lp info is still {DailyLpOverview}",
                        lastDailyLpOverview);
                    
                    await Task.Delay(_configuration.DailyLpOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var dailyLpOverviewInfo = await GetDailyLpOverview(
                    yesterday,
                    yesterday.AddDays(1),
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (dailyLpOverviewInfo == null)
                {
                    await Task.Delay(_configuration.DailyLpOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var dailyLpOverview = new DailyLpOverviewInfo(
                    yesterday, 
                    _configuration.LiquidityProviders,
                    dailyLpOverviewInfo);
                
                _logger.LogInformation(
                    "Broadcasting daily LP overview for {DailyLpOverview}",
                    dateString);

                await _pipeline.Source.SendAsync(
                    dailyLpOverview, 
                    cancellationToken);
                
                lastDailyLpOverview = dateString;
                await StoreLastDailyLpOverview(lastDailyLpOverview);
                
                await Task.Delay(_configuration.DailyLpOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastDailyLpOverview(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastDailyLpOverviewLocation))
                return await File.ReadAllTextAsync(_configuration.LastDailyLpOverviewLocation, cancellationToken);

            await using var file = File.CreateText(_configuration.LastDailyLpOverviewLocation);
            await file.WriteAsync("2024-08-21");
            return "2024-08-21";
        }
        
        private async Task StoreLastDailyLpOverview(string lastDailyLpOverview)
        {
            await using var file = File.CreateText(_configuration.LastDailyLpOverviewLocation);
            await file.WriteAsync(lastDailyLpOverview.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<DailyLpOverviewResponse?> GetDailyLpOverview(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("LpGraph");

                var query = LpQuery
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
                        .ReadFromJsonAsync<DailyLpOverviewResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetDailyLpOverview returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching daily LP overview failed.");
            }

            return null;
        }
    }
}