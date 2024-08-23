namespace ChainflipInsights.Feeders.BrokerOverview
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

    public class BrokerOverviewFeeder : IFeeder
    {
        // TODO: Since Chainflip API does not return Affiliate Brokers, we need to build this from the swaps database
        /*
           SELECT SUM(DepositValueUsd) AS Volume, SUM(BrokerFeeUsd) AS Fees, Broker 
           FROM Insights.swap_info
           WHERE SwapDate > 'TIME_FROM'
           AND SwapDate < 'TIME_TO'
           GROUP BY Broker
           ORDER BY SUM(DepositValueUsd) DESC
         */
        
        private const string BrokerOverviewQuery = 
            """
            {
                brokersAggregate(
                    startDate: \"TIME_FROM\", 
                    endDate: \"TIME_TO\") {
                    idSs58
                    swapCount
                    swapFeeUsd
                    volume
                }
            }
            """;

        private readonly ILogger<BrokerOverviewFeeder> _logger;
        private readonly Pipeline<BrokerOverviewInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public BrokerOverviewFeeder(
            ILogger<BrokerOverviewFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<BrokerOverviewInfo> pipeline)
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
                if (!_configuration.EnableBrokerOverview.Value)
                {
                    _logger.LogInformation(
                        "Broker Overview not enabled. Skipping {TaskName}",
                        nameof(BrokerOverviewFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(BrokerOverviewFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching BrokerOverview Info
                await ProvideBrokerOverviewInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(BrokerOverviewFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(BrokerOverviewFeeder));
            }
        }

        private async Task ProvideBrokerOverviewInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastBrokerOverview = await GetLastBrokerOverview(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var yesterday = DateTime.UtcNow.Date.AddDays(-1);
                var dateString = yesterday.ToString("yyyy-MM-dd");
                if (dateString == lastBrokerOverview)
                {
                    _logger.LogInformation(
                        "No new broker overview to announce. Last broker overview info is still {BrokerOverview}",
                        lastBrokerOverview);
                    
                    await Task.Delay(_configuration.BrokerOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var brokerOverviewInfo = await GetBrokerOverview(
                    yesterday,
                    yesterday.AddDays(1),
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (brokerOverviewInfo == null)
                {
                    await Task.Delay(_configuration.BrokerOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var brokerOverview = new BrokerOverviewInfo(
                    yesterday, 
                    brokerOverviewInfo.Data.Data.Select(x => new BrokerInfo(x)));
                
                _logger.LogInformation(
                    "Broadcasting {TotalBrokers} brokers",
                    brokerOverview.Brokers.Count);

                await _pipeline.Source.SendAsync(
                    brokerOverview, 
                    cancellationToken);
                
                lastBrokerOverview = dateString;
                await StoreLastBrokerOverview(lastBrokerOverview);
                
                await Task.Delay(_configuration.BrokerOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastBrokerOverview(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastBrokerOverviewLocation))
                return await File.ReadAllTextAsync(_configuration.LastBrokerOverviewLocation, cancellationToken);

            await using var file = File.CreateText(_configuration.LastBrokerOverviewLocation);
            await file.WriteAsync("2024-03-27");
            return "2024-03-27";
        }
        
        private async Task StoreLastBrokerOverview(string lastBrokerOverview)
        {
            await using var file = File.CreateText(_configuration.LastBrokerOverviewLocation);
            await file.WriteAsync(lastBrokerOverview.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<BrokerOverviewResponse?> GetBrokerOverview(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = BrokerOverviewQuery
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
                        .ReadFromJsonAsync<BrokerOverviewResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetBrokerOverview returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching broker overview failed.");
            }

            return null;
        }
    }
}