namespace ChainflipInsights.Feeders.Burn
{
    using System;
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

    public class BurnFeeder : IFeeder
    {
        private const string BurnsQuery = 
            """
            {
                allBurns(orderBy: TIMESTAMP_DESC) {
                    nodes {
                        timestamp
                        amount
                        eventByEventId {
                            blockId
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<BurnFeeder> _logger;
        private readonly Pipeline<BurnInfo> _pipeline;
        private readonly PriceProvider _priceProvider;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public BurnFeeder(
            ILogger<BurnFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<BurnInfo> pipeline,
            PriceProvider priceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _priceProvider = priceProvider ?? throw new ArgumentNullException(nameof(priceProvider));
        }
        
        public async Task Start()
        {
            try
            {
                if (!_configuration.EnableBurn.Value)
                {
                    _logger.LogInformation(
                        "Burn not enabled. Skipping {TaskName}",
                        nameof(BurnFeeder));

                    return;
                }

                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, 30000), _pipeline.CancellationToken);
                
                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(BurnFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Burn Info
                await ProvideBurnInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(BurnFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(BurnFeeder));
            }
        }
        
        private async Task ProvideBurnInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastBurn = await GetLastBurn(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var burns = await GetBurns(cancellationToken);
                if (burns == null)
                {
                    await Task.Delay(_configuration.BurnQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }

                var burnsIncreasing = burns.Data.Data.Data.OrderBy(x => x.Timestamp);

                foreach (var burn in burnsIncreasing)
                {
                    var burnDate = new DateOnly(burn.Timestamp.Year, burn.Timestamp.Month, burn.Timestamp.Day);
                    var lastBurnDate = new DateOnly(lastBurn.Year, lastBurn.Month, lastBurn.Day);
                    if (burnDate <= lastBurnDate) 
                        continue;
      
                    var burnInfo = new BurnInfo(
                        _priceProvider,
                        burn.Event.BlockId,
                        burn.Timestamp,
                        burn.BurnedAmount);
                        
                    _logger.LogInformation(
                        "Broadcasting Burn {BurnDate}: {FlipBurned} FLIP",
                        burn.Timestamp,
                        burnInfo.FlipBurnedFormatted);

                    await _pipeline.Source.SendAsync(
                        burnInfo, 
                        cancellationToken);

                    lastBurn = burnDate;
                    await StoreLastBurn(lastBurn);
                        
                    await Task.Delay(_configuration.BurnQueryDelay.Value.RandomizeTime(), cancellationToken);
                }
                
                await Task.Delay(_configuration.BurnQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<DateOnly> GetLastBurn(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastBurnLocation))
                return DateOnly.Parse(await File.ReadAllTextAsync(_configuration.LastBurnLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastBurnLocation);
            var lastBurn = DateOnly.Parse("2024-06-30");
            await file.WriteAsync(lastBurn.ToString());
            return lastBurn;
        }
        
        private async Task StoreLastBurn(DateOnly lastBurn)
        {
            await using var file = File.CreateText(_configuration.LastBurnLocation);
            await file.WriteAsync(lastBurn.ToString());
        }

        private async Task<BurnsResponse?> GetBurns(
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = BurnsQuery;
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
                        .ReadFromJsonAsync<BurnsResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetBurns returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching burns failed.");
            }
            
            return null;
        }
    }
}