namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MastodonConsumer
    {
        private readonly ILogger<MastodonConsumer> _logger;
        private readonly BotConfiguration _configuration;

        public MastodonConsumer(
            ILogger<MastodonConsumer> logger,
            IOptions<BotConfiguration> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
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

                    // if (input.SwapInfo != null)
                    //     ProcessSwap(input.SwapInfo, cancellationToken);
                    //
                    // if (input.IncomingLiquidityInfo != null)
                    //     ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo, cancellationToken);
                    //
                    // if (input.EpochInfo != null)
                    //     ProcessEpochInfo(input.EpochInfo, cancellationToken);
                    //
                    // if (input.FundingInfo != null)
                    //     ProcessFundingInfo(input.FundingInfo, cancellationToken);
                    //
                    // if (input.RedemptionInfo != null)
                    //     ProcessRedemptionInfo(input.RedemptionInfo, cancellationToken);
                    
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