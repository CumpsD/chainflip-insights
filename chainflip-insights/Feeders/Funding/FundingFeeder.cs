namespace ChainflipInsights.Feeders.Funding
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

    public class FundingFeeder : IFeeder
    {
        private const string FundingQuery = 
            """
            {
                allAccountFundingEvents(orderBy: ID_ASC, first: 100, filter: {
                    and: {
                        id: { greaterThan: LAST_ID }
                        type: { equalTo: FUNDED }
                    }
                }) {
                    edges {
                        node {
                            id
                            
                            amount
                            epochId
                            accountByAccountId {
                                alias
                                idSs58
                                role
                            }
                            eventByEventId {
                                blockByBlockId {
                                    timestamp
                                }
                            }
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<FundingFeeder> _logger;
        private readonly Pipeline<FundingInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public FundingFeeder(
            ILogger<FundingFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<FundingInfo> pipeline)
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
                if (!_configuration.EnableFunding.Value)
                {
                    _logger.LogInformation(
                        "Funding not enabled. Skipping {TaskName}",
                        nameof(FundingFeeder));

                    return;
                }

                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, 30000), _pipeline.CancellationToken);
                
                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(FundingFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Funding Info
                await ProvideFundingInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(FundingFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(FundingFeeder));
            }
        }

        private async Task ProvideFundingInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastFundingId(cancellationToken);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var fundingInfo = await GetFunding(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (fundingInfo == null)
                {
                    await Task.Delay(_configuration.FundingQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var funding = fundingInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                if (funding.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new funding to announce. Last funding is still {FundingId}",
                        lastId);
                }

                // Funding is in increasing order
                foreach (var fundingDetails in funding.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    if (fundingDetails.Account.Role != "VALIDATOR")
                    {
                        lastId = fundingDetails.Id;
                        await StoreLastFundingId(lastId);
                        continue;
                    }

                    var fundingDetail = new FundingInfo(fundingDetails);

                    _logger.LogInformation(
                        "Broadcasting Funding {FundingId}, {Validator} added {Funding} FLIP. {FundingUrl}",
                        fundingDetail.Id,
                        fundingDetail.Validator,
                        fundingDetail.AmountFormatted,
                        string.Format(_configuration.ValidatorUrl, fundingDetail.ValidatorName));

                    await _pipeline.Source.SendAsync(
                        fundingDetail, 
                        cancellationToken);
                    
                    lastId = fundingDetail.Id;
                    await StoreLastFundingId(lastId);
                }
                
                await Task.Delay(_configuration.FundingQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastFundingId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastFundingIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastFundingIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastFundingIdLocation);
            await file.WriteAsync("0");
            return 0;
        }
        
        private async Task StoreLastFundingId(double fundingId)
        {
            await using var file = File.CreateText(_configuration.LastFundingIdLocation);
            await file.WriteAsync(fundingId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<FundingResponse?> GetFunding(
            double fromId,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = FundingQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
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
                        .ReadFromJsonAsync<FundingResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetFunding returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching funding failed.");
            }

            return null;
        }
    }
}