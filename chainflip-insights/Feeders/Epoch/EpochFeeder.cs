namespace ChainflipInsights.Feeders.Epoch
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

    public class EpochFeeder
    {
        private const string EpochQuery = 
            """
            {
                allEpoches(orderBy: ID_DESC, first: 50, filter: {
                    id: { greaterThanOrEqualTo: LAST_ID }
                }) {
                    edges {
                        node {
                            id
                            bond # MAB, divide by 10^18
                            totalBonded # divide by 10^18
                            startBlockId
                            blockByStartBlockId {
                                timestamp
                            }
                            authorityMembershipsByEpochId {
                                edges {
                                    node {
                                        validatorId
                                        validatorByValidatorId {
                                            idSs58
                                        }
                                        bid
                                        reward
                                    }
                                }
                            }
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<EpochFeeder> _logger;
        private readonly Pipeline<EpochInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public EpochFeeder(
            ILogger<EpochFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<EpochInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            if (!_configuration.EnableEpoch.Value)
            {
                _logger.LogInformation(
                    "Epoch not enabled. Skipping {TaskName}",
                    nameof(EpochFeeder));
                
                return;
            }
            
            _logger.LogInformation(
                "Starting {TaskName}",
                nameof(EpochFeeder));

            // Give the consumers some time to connect
            await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);
            
            // Start a loop fetching Liquidity Info
            await ProvideEpochInfo(_pipeline.CancellationToken);
            
            _logger.LogInformation(
                "Stopping {TaskName}",
                nameof(EpochFeeder));
        }

        private async Task ProvideEpochInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastEpochId(cancellationToken);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var epochInfo = await GetEpoch(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (epochInfo == null)
                {
                    await Task.Delay(_configuration.EpochQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var epoch = epochInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                if (epoch.Count <= 1)
                {
                    _logger.LogInformation(
                        "No new epoch to announce. Last epoch is still {EpochId}",
                        lastId);
                }

                // Epoch is in increasing order, we skip the first one since we always need to look back for rewards
                var previousEpoch = new EpochInfo(epoch.First(), null);
                foreach (var epochDetails in epoch.Skip(1).TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    var epochDetail = new EpochInfo(epochDetails, previousEpoch);

                    _logger.LogInformation(
                        "Broadcasting Epoch {EpochId}, started {EpochStart} with MAB {MinimumBond} FLIP. " +
                        "In total we have {TotalBond} FLIP bonded, with a maximum bond of {MaxBid} FLIP. Last Epoch distributed {Rewards} FLIP. {AuthorityUrl}",
                        epochDetail.Id,
                        epochDetail.EpochStart,
                        epochDetail.MinimumBondFormatted,
                        epochDetail.TotalBondFormatted,
                        epochDetail.MaxBidFormatted,
                        previousEpoch.TotalRewardsFormatted,
                        $"{_configuration.ExplorerAuthorityUrl}{epochDetail.Id}");

                    await _pipeline.Source.SendAsync(
                        epochDetail, 
                        cancellationToken);
                    
                    lastId = epochDetail.Id;
                    await StoreLastEpochId(lastId);
                    
                    previousEpoch = epochDetail;
                }
                
                await Task.Delay(_configuration.EpochQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastEpochId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastEpochIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastEpochIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastEpochIdLocation);
            await file.WriteAsync("77");
            return 77;
        }
        
        private async Task StoreLastEpochId(double epochId)
        {
            await using var file = File.CreateText(_configuration.LastEpochIdLocation);
            await file.WriteAsync(epochId.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<EpochResponse?> GetEpoch(
            double fromId,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Graph");

            var query = EpochQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
            var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";
            
            var response = await client.PostAsync(
                string.Empty,
                new StringContent(
                    graphQuery, 
                    new MediaTypeHeaderValue(MediaTypeNames.Application.Json)), 
                cancellationToken);

            return await response.Content.ReadFromJsonAsync<EpochResponse>(cancellationToken: cancellationToken);
        }
    }
}