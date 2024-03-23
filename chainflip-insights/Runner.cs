namespace ChainflipInsights
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Consumers;
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.Mastodon;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.CfeVersion;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Feeders.StakedFlip;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;

    public class Runner
    {
        private readonly ILogger<Runner> _logger;
        private readonly DiscordConsumer _discordConsumer;
        private readonly TelegramConsumer _telegramConsumer;
        private readonly TwitterConsumer _twitterConsumer;
        private readonly MastodonConsumer _mastodonConsumer;

        private ITargetBlock<BroadcastInfo> _discordPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _telegramPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _twitterPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _mastodonPipelineTarget = null!;

        public Runner(
            ILogger<Runner> logger,
            Pipeline<SwapInfo> swapPipeline,
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline,
            Pipeline<EpochInfo> epochPipeline,
            Pipeline<FundingInfo> fundingPipeline,
            Pipeline<RedemptionInfo> redemptionPipeline,
            Pipeline<CexMovementInfo> cexMovementPipeline,
            Pipeline<CfeVersionsInfo> cfeVersionPipeline,
            Pipeline<SwapLimitsInfo> swapLimitsPipeline,
            Pipeline<PastVolumeInfo> pastVolumePipeline,
            Pipeline<StakedFlipInfo> stakedFlipPipeline,
            Pipeline<BrokerOverviewInfo> brokerOverviewPipeline,
            Pipeline<BigStakedFlipInfo> bigStakedFlipPipeline,
            DiscordConsumer discordConsumer,
            TelegramConsumer telegramConsumer,
            TwitterConsumer twitterConsumer,
            MastodonConsumer mastodonConsumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _discordConsumer = discordConsumer ?? throw new ArgumentNullException(nameof(discordConsumer));
            _telegramConsumer = telegramConsumer ?? throw new ArgumentNullException(nameof(telegramConsumer));
            _twitterConsumer = twitterConsumer ?? throw new ArgumentNullException(nameof(twitterConsumer));
            _mastodonConsumer = mastodonConsumer ?? throw new ArgumentNullException(nameof(mastodonConsumer));

            SetupPipelines(
                swapPipeline,
                incomingLiquidityPipeline,
                epochPipeline,
                fundingPipeline,
                redemptionPipeline,
                cexMovementPipeline,
                cfeVersionPipeline,
                swapLimitsPipeline,
                pastVolumePipeline,
                stakedFlipPipeline,
                brokerOverviewPipeline,
                bigStakedFlipPipeline);
        }

        private void SetupPipelines(
            Pipeline<SwapInfo> swapPipeline, 
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline, 
            Pipeline<EpochInfo> epochPipeline, 
            Pipeline<FundingInfo> fundingPipeline, 
            Pipeline<RedemptionInfo> redemptionPipeline, 
            Pipeline<CexMovementInfo> cexMovementPipeline, 
            Pipeline<CfeVersionsInfo> cfeVersionPipeline, 
            Pipeline<SwapLimitsInfo> swapLimitsPipeline,
            Pipeline<PastVolumeInfo> pastVolumePipeline,
            Pipeline<StakedFlipInfo> stakedFlipPipeline,
            Pipeline<BrokerOverviewInfo> brokerOverviewPipeline,
            Pipeline<BigStakedFlipInfo> bigStakedFlipPipeline)
        {
            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };
            
            var broadcast = new BroadcastBlock<BroadcastInfo>(
                e => e,
                new DataflowBlockOptions
                {
                    CancellationToken = swapPipeline.CancellationToken
                });
            
            broadcast.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Broadcast completed, {Status}",
                    task.Status),
                swapPipeline.CancellationToken);

            SetupFeederPipeline(
                "Swap",
                swapPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));

            SetupFeederPipeline(
                "Incoming Liquidity",
                incomingLiquidityPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "Epoch",
                epochPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
           
            SetupFeederPipeline(
                "Funding",
                fundingPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "Redemption",
                redemptionPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "CEX Movement",
                cexMovementPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "CFE Version",
                cfeVersionPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "Swap Limits",
                swapLimitsPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "Past Volume",
                pastVolumePipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "Staked Flip",
                stakedFlipPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            SetupFeederPipeline(
                "Broker Overview",
                brokerOverviewPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));

            SetupFeederPipeline(
                "Big Staked Flip",
                bigStakedFlipPipeline,
                linkOptions,
                broadcast,
                x => new BroadcastInfo(x));
            
            _discordPipelineTarget = SetupConsumerPipeline(
                "Discord",
                _discordConsumer,
                broadcast,
                linkOptions, 
                swapPipeline.CancellationToken);
            
            _telegramPipelineTarget = SetupConsumerPipeline(
                "Telegram",
                _telegramConsumer,
                broadcast,
                linkOptions, 
                swapPipeline.CancellationToken);
            
            _twitterPipelineTarget = SetupConsumerPipeline(
                "Twitter",
                _twitterConsumer,
                broadcast,
                linkOptions, 
                swapPipeline.CancellationToken);
            
            _mastodonPipelineTarget = SetupConsumerPipeline(
                "Mastodon",
                _mastodonConsumer,
                broadcast,
                linkOptions, 
                swapPipeline.CancellationToken);
        }

        private void SetupFeederPipeline<T>(
            string name,
            Pipeline<T> pipeline, 
            DataflowLinkOptions linkOptions, 
            BroadcastBlock<BroadcastInfo> broadcast,
            Func<T, BroadcastInfo> buildBroadcastInfo)
        {
            var source = pipeline.Source;
            source.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "{Name} Source completed, {Status}",
                    name,
                    task.Status),
                pipeline.CancellationToken);
            
            var wrap = new TransformBlock<T, BroadcastInfo>(
                buildBroadcastInfo,
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = pipeline.CancellationToken,
                    MaxDegreeOfParallelism = 1,
                    EnsureOrdered = true,
                    SingleProducerConstrained = true
                });

            wrap.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Wrap {Name} completed, {Status}",
                    name,
                    task.Status),
                pipeline.CancellationToken);
            
            source.LinkTo(wrap, linkOptions);
            wrap.LinkTo(broadcast, linkOptions);
        }

        private ITargetBlock<BroadcastInfo> SetupConsumerPipeline<T>(
            string name,
            T consumer,
            BroadcastBlock<BroadcastInfo> broadcast, 
            DataflowLinkOptions linkOptions,
            CancellationToken cancellationToken) where T : IConsumer
        {
            var pipeline = consumer.Build(cancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "{Name} Pipeline completed, {Status}",
                    name,
                    task.Status),
                cancellationToken);
            
            broadcast.LinkTo(pipeline, linkOptions);

            return pipeline;
        }
        
        public IEnumerable<Task> Start()
        {
            _logger.LogInformation(
                "Starting {TaskName}", nameof(Runner));

            try
            {
                return
                [
                    _discordPipelineTarget.Completion,
                    _telegramPipelineTarget.Completion,
                    _twitterPipelineTarget.Completion,
                    _mastodonPipelineTarget.Completion
                ];
            }
            catch (AggregateException ex)
            {
                ex.Handle(e =>
                {
                    _logger.LogCritical(
                        "Encountered {ExceptionName}: {Message}",
                        e.GetType().Name,
                        e.Message);
                    return true;
                });

                throw;
            }
        }
    }
}