namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Mastonet;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public partial class MastodonConsumer : IConsumer
    {
        private readonly ILogger<MastodonConsumer> _logger;
        private readonly MastodonClient _mastodonClient;
        private readonly BotConfiguration _configuration;
        private readonly Dictionary<string, string> _brokers;

        public MastodonConsumer(
            ILogger<MastodonConsumer> logger,
            IOptions<BotConfiguration> options,
            MastodonClient mastodonClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mastodonClient = mastodonClient ?? throw new ArgumentNullException(nameof(mastodonClient));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            
            _brokers = _configuration
                .Brokers
                .ToDictionary(
                    x => x.Address,
                    x => x.Name);
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
                    if (!_configuration.EnableMastodon.Value)
                        return;

                    if (input.SwapInfo != null)
                        ProcessSwap(input.SwapInfo);
                    
                    if (input.IncomingLiquidityInfo != null)
                        ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo);
                    
                    if (input.EpochInfo != null)
                        ProcessEpochInfo(input.EpochInfo);
                    
                    if (input.FundingInfo != null)
                        ProcessFundingInfo(input.FundingInfo);
                    
                    if (input.RedemptionInfo != null)
                        ProcessRedemptionInfo(input.RedemptionInfo);
                    
                    if (input.CexMovementInfo != null)
                        ProcessCexMovementInfo(input.CexMovementInfo);
                    
                    if (input.SwapLimitsInfo != null)
                        ProcessSwapLimitsInfo(input.SwapLimitsInfo);
                    
                    if (input.PastVolumeInfo != null)
                        ProcessPastVolumeInfo(input.PastVolumeInfo);
                    
                    if (input.StakedFlipInfo != null)
                        ProcessStakedFlipInfo(input.StakedFlipInfo);
                    
                    if (input.BrokerOverviewInfo != null)
                        ProcessBrokerOverviewInfo(input.BrokerOverviewInfo);
                    
                    if (input.BigStakedFlipInfo != null)
                        ProcessBigStakedFlipInfo(input.BigStakedFlipInfo);
                    
                    if (input.BurnInfo != null)
                        ProcessBurnInfo(input.BurnInfo);
                    
                    Task
                        .Delay(1500, cancellationToken)
                        .GetAwaiter()
                        .GetResult();
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
                });

            logging.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Mastodon Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }
    }
}