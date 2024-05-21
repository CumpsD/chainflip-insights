namespace ChainflipInsights
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Consumers;
    using ChainflipInsights.Consumers.Database;
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.FullTelegram;
    using ChainflipInsights.Consumers.LpTelegram;
    using ChainflipInsights.Consumers.Mastodon;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using ChainflipInsights.Feeders.BrokerOverview;   
    using ChainflipInsights.Feeders.Burn;
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
        private readonly FullTelegramConsumer _fullTelegramConsumer;
        private readonly LpTelegramConsumer _lpTelegramConsumer;
        private readonly TwitterConsumer _twitterConsumer;
        private readonly MastodonConsumer _mastodonConsumer;
        private readonly DatabaseConsumer _databaseConsumer;

        private ITargetBlock<BroadcastInfo> _discordPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _telegramPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _fullTelegramPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _lpTelegramPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _twitterPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _mastodonPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _databasePipelineTarget = null!;

        public Runner(
            ILogger<Runner> logger,
            Pipeline<BurnInfo> burnPipeline,
            Pipeline<SwapInfo> swapPipeline,
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline,
            Pipeline<OutgoingLiquidityInfo> outgoingLiquidityPipeline,
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
            FullTelegramConsumer fullTelegramConsumer,
            LpTelegramConsumer lpTelegramConsumer,
            TwitterConsumer twitterConsumer,
            MastodonConsumer mastodonConsumer,
            DatabaseConsumer databaseConsumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _discordConsumer = discordConsumer ?? throw new ArgumentNullException(nameof(discordConsumer));
            _telegramConsumer = telegramConsumer ?? throw new ArgumentNullException(nameof(telegramConsumer));
            _fullTelegramConsumer = fullTelegramConsumer ?? throw new ArgumentNullException(nameof(fullTelegramConsumer));
            _lpTelegramConsumer = lpTelegramConsumer ?? throw new ArgumentNullException(nameof(lpTelegramConsumer));
            _twitterConsumer = twitterConsumer ?? throw new ArgumentNullException(nameof(twitterConsumer));
            _mastodonConsumer = mastodonConsumer ?? throw new ArgumentNullException(nameof(mastodonConsumer));
            _databaseConsumer = databaseConsumer ?? throw new ArgumentNullException(nameof(databaseConsumer));

            SetupPipelines(
                burnPipeline,
                swapPipeline,
                incomingLiquidityPipeline,
                outgoingLiquidityPipeline,
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
            Pipeline<BurnInfo> burnPipeline, 
            Pipeline<SwapInfo> swapPipeline, 
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline, 
            Pipeline<OutgoingLiquidityInfo> outgoingLiquidityPipeline, 
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
                "Outgoing Liquidity",
                outgoingLiquidityPipeline,
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
            
            SetupFeederPipeline(
                "Burn",
                burnPipeline,
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
            
            _fullTelegramPipelineTarget = SetupConsumerPipeline(
                "Full Telegram",
                _fullTelegramConsumer,
                broadcast,
                linkOptions, 
                swapPipeline.CancellationToken);
            
            _lpTelegramPipelineTarget = SetupConsumerPipeline(
                "LP Telegram",
                _lpTelegramConsumer,
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
            
            _databasePipelineTarget = SetupConsumerPipeline(
                "Database",
                _databaseConsumer,
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
                    _fullTelegramPipelineTarget.Completion,
                    _lpTelegramPipelineTarget.Completion,
                    _twitterPipelineTarget.Completion,
                    _mastodonPipelineTarget.Completion,
                    _databasePipelineTarget.Completion,
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