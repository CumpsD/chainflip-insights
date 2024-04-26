namespace ChainflipInsights.Consumers.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    
    public partial class DatabaseConsumer : IConsumer
    {
        private readonly ILogger<DatabaseConsumer> _logger;
        private readonly IDbContextFactory<BotContext> _dbContextFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BotConfiguration _configuration;
        private readonly Dictionary<string, Broker> _brokers;

        public DatabaseConsumer(
            ILogger<DatabaseConsumer> logger,
            IOptions<BotConfiguration> options,
            IDbContextFactory<BotContext> dbContextFactory,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            
            _brokers = _configuration
                .Brokers
                .ToDictionary(
                    x => x.Address,
                    x => x);
        }

        public ITargetBlock<BroadcastInfo> Build(
            CancellationToken cancellationToken)
        {
            var announcer = BuildAnnouncer(cancellationToken);
            return new EncapsulatingTarget<BroadcastInfo, BroadcastInfo>(announcer, announcer);
        }

        private ActionBlock<BroadcastInfo> BuildAnnouncer(
            CancellationToken cancellationToken)
        {
            var logging = new ActionBlock<BroadcastInfo>(
                input =>
                {
                    if (!_configuration.EnableDatabase.Value)
                        return;
                    
                    if (input.EpochInfo != null)
                        ProcessEpochInfo(input.EpochInfo);

                    if (input.IncomingLiquidityInfo != null)
                        ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo);
                    
                    if (input.OutgoingLiquidityInfo != null)
                        ProcessOutgoingLiquidityInfo(input.OutgoingLiquidityInfo);
                    
                    if (input.FundingInfo != null)
                        ProcessFundingInfo(input.FundingInfo);
                    
                    if (input.RedemptionInfo != null)
                        ProcessRedemptionInfo(input.RedemptionInfo);
                    
                    if (input.BurnInfo != null)
                        ProcessBurnInfo(input.BurnInfo);
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
                });

            logging.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Database Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }
    }
}