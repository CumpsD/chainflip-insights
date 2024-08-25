namespace ChainflipInsights.Feeders.WeeklyLpOverview
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

    public class WeeklyLpOverviewFeeder : IFeeder
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
                
        private readonly ILogger<WeeklyLpOverviewFeeder> _logger;
        private readonly Pipeline<WeeklyLpOverviewInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeeklyLpOverviewFeeder(
            ILogger<WeeklyLpOverviewFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<WeeklyLpOverviewInfo> pipeline)
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
                if (!_configuration.EnableWeeklyLpOverview.Value)
                {
                    _logger.LogInformation(
                        "Weekly Lp Overview not enabled. Skipping {TaskName}",
                        nameof(WeeklyLpOverviewFeeder));

                    return;
                }
                
                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, _configuration.StartupDelay.Value), _pipeline.CancellationToken);

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(WeeklyLpOverviewFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching WeeklyLpOverview Info
                await ProvideWeeklyLpOverviewInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(WeeklyLpOverviewFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(WeeklyLpOverviewFeeder));
            }
        }

        private async Task ProvideWeeklyLpOverviewInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastWeeklyLpOverview = await GetLastWeeklyLpOverview(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                // Determine last week's dates
                var startingDate = DateTime.UtcNow.Date;

                while (startingDate.DayOfWeek != DayOfWeek.Monday)
                    startingDate = startingDate.AddDays(-1);

                var startOfWeek = startingDate.AddDays(-7);
                var endOfWeek = startingDate.AddDays(-1);
                
                var startOfWeekString = startOfWeek.ToString("yyyy-MM-dd");
                var endOfWeekString = endOfWeek.ToString("yyyy-MM-dd");
                
                var dateString = $"{startOfWeekString}|{endOfWeekString}";
                if (dateString == lastWeeklyLpOverview)
                {
                    _logger.LogInformation(
                        "No new weekly LP overview to announce. Last weekly lp info is still {WeeklyLpOverview}",
                        lastWeeklyLpOverview);
                    
                    await Task.Delay(_configuration.WeeklyLpOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var weeklyLpOverviewInfo = await GetWeeklyLpOverview(
                    startOfWeek,
                    endOfWeek,
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (weeklyLpOverviewInfo == null)
                {
                    await Task.Delay(_configuration.WeeklyLpOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var weeklyLpOverview = new WeeklyLpOverviewInfo(
                    startOfWeek,
                    endOfWeek,
                    _configuration.LiquidityProviders,
                    weeklyLpOverviewInfo);
                
                _logger.LogInformation(
                    "Broadcasting weekly LP overview for {WeeklyLpOverview}",
                    dateString);

                await _pipeline.Source.SendAsync(
                    weeklyLpOverview, 
                    cancellationToken);
                
                lastWeeklyLpOverview = dateString;
                await StoreLastWeeklyLpOverview(lastWeeklyLpOverview);
                
                await Task.Delay(_configuration.WeeklyLpOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastWeeklyLpOverview(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastWeeklyLpOverviewLocation))
                return await File.ReadAllTextAsync(_configuration.LastWeeklyLpOverviewLocation, cancellationToken);

            await using var file = File.CreateText(_configuration.LastWeeklyLpOverviewLocation);
            await file.WriteAsync("1");
            return "1";
        }
        
        private async Task StoreLastWeeklyLpOverview(string lastWeeklyLpOverview)
        {
            await using var file = File.CreateText(_configuration.LastWeeklyLpOverviewLocation);
            await file.WriteAsync(lastWeeklyLpOverview.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<WeeklyLpOverviewResponse?> GetWeeklyLpOverview(
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
                        .ReadFromJsonAsync<WeeklyLpOverviewResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetWeeklyLpOverview returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching weekly LP overview failed.");
            }

            return null;
        }
    }
}