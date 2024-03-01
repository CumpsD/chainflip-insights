namespace ChainflipInsights.Feeders.Redemption
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

    public class RedemptionFeeder : IFeeder
    {
        private const string RedemptionQuery = 
            """
            {
                allAccountFundingEvents(orderBy: ID_ASC, first: 50, filter: {
                    and: {
                        id: { greaterThan: LAST_ID }
                        type: { equalTo: REDEEMED }
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
        
        private readonly ILogger<RedemptionFeeder> _logger;
        private readonly Pipeline<RedemptionInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public RedemptionFeeder(
            ILogger<RedemptionFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<RedemptionInfo> pipeline)
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
                if (!_configuration.EnableRedemption.Value)
                {
                    _logger.LogInformation(
                        "Redemption not enabled. Skipping {TaskName}",
                        nameof(RedemptionFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(RedemptionFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching Redemption Info
                await ProvideRedemptionInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(RedemptionFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(RedemptionFeeder));
            }
        }

        private async Task ProvideRedemptionInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastRedemptionId(cancellationToken);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var redemptionInfo = await GetRedemption(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (redemptionInfo == null)
                {
                    await Task.Delay(_configuration.RedemptionQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var redemption = redemptionInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                if (redemption.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new redemption to announce. Last redemption is still {RedemptionId}",
                        lastId);
                }

                // Redemption is in increasing order
                foreach (var redemptionDetails in redemption.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    if (redemptionDetails.Account.Role != "VALIDATOR")
                    {
                        lastId = redemptionDetails.Id;
                        await StoreLastRedemptionId(lastId);
                        continue;
                    }

                    var redemptionDetail = new RedemptionInfo(redemptionDetails);

                    _logger.LogInformation(
                        "Broadcasting Redemption {RedemptionId}, {Validator} redeemed {Redemption} FLIP. {RedemptionUrl}",
                        redemptionDetail.Id,
                        redemptionDetail.Validator,
                        redemptionDetail.AmountFormatted,
                        string.Format(_configuration.ValidatorUrl, redemptionDetail.ValidatorName));

                    await _pipeline.Source.SendAsync(
                        redemptionDetail, 
                        cancellationToken);
                    
                    lastId = redemptionDetail.Id;
                    await StoreLastRedemptionId(lastId);
                }
                
                await Task.Delay(_configuration.RedemptionQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastRedemptionId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastRedemptionIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastRedemptionIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastRedemptionIdLocation);
            await file.WriteAsync("3042");
            return 3042;
        }
        
        private async Task StoreLastRedemptionId(double redemptionId)
        {
            await using var file = File.CreateText(_configuration.LastRedemptionIdLocation);
            await file.WriteAsync(redemptionId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<RedemptionResponse?> GetRedemption(
            double fromId,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("Graph");

                var query = RedemptionQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
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
                        .ReadFromJsonAsync<RedemptionResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetRedemption returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching redemption failed.");
            }

            return null;
        }
    }
}