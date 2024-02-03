namespace ChainflipInsights
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;

    public class Runner
    {
        private readonly ILogger<Runner> _logger;
        private readonly DiscordConsumer _discordConsumer;
        private readonly TelegramConsumer _telegramConsumer;
        private readonly TwitterConsumer _twitterConsumer;

        private ITargetBlock<BroadcastInfo> _discordSwapPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _telegramSwapPipelineTarget = null!;
        private ITargetBlock<BroadcastInfo> _twitterSwapPipelineTarget = null!;
        
        public Runner(
            ILogger<Runner> logger,
            Pipeline<SwapInfo> swapPipeline,
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline,
            Pipeline<EpochInfo> epochPipeline,
            Pipeline<FundingInfo> fundingPipeline,
            DiscordConsumer discordConsumer,
            TelegramConsumer telegramConsumer,
            TwitterConsumer twitterConsumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _discordConsumer = discordConsumer ?? throw new ArgumentNullException(nameof(discordConsumer));
            _telegramConsumer = telegramConsumer ?? throw new ArgumentNullException(nameof(telegramConsumer));
            _twitterConsumer = twitterConsumer ?? throw new ArgumentNullException(nameof(twitterConsumer));

            SetupPipelines(
                swapPipeline,
                incomingLiquidityPipeline,
                epochPipeline,
                fundingPipeline);
        }

        private void SetupPipelines(
            Pipeline<SwapInfo> swapPipeline, 
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline, 
            Pipeline<EpochInfo> epochPipeline, 
            Pipeline<FundingInfo> fundingPipeline)
        {
            var swapSource = swapPipeline.Source;
            swapSource.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Swap Source completed, {Status}",
                    task.Status),
                swapPipeline.CancellationToken);
            
            var incomingLiquiditySource = incomingLiquidityPipeline.Source;
            incomingLiquiditySource.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Incoming Liquidity Source completed, {Status}",
                    task.Status),
                incomingLiquidityPipeline.CancellationToken);
            
            var epochSource = epochPipeline.Source;
            epochSource.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Epoch Source completed, {Status}",
                    task.Status),
                epochPipeline.CancellationToken);
            
            var fundingSource = fundingPipeline.Source;
            fundingSource.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Funding Source completed, {Status}",
                    task.Status),
                fundingPipeline.CancellationToken);
            
            var wrapSwaps = new TransformBlock<SwapInfo, BroadcastInfo>(
                swapInfo => new BroadcastInfo(swapInfo),
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = swapPipeline.CancellationToken,
                    MaxDegreeOfParallelism = 1,
                    EnsureOrdered = true,
                    SingleProducerConstrained = true
                });

            wrapSwaps.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Wrap Swaps completed, {Status}",
                    task.Status),
                swapPipeline.CancellationToken);
            
            var wrapIncomingLiquidity = new TransformBlock<IncomingLiquidityInfo, BroadcastInfo>(
                incomingLiquidityInfo => new BroadcastInfo(incomingLiquidityInfo),
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = incomingLiquidityPipeline.CancellationToken,
                    MaxDegreeOfParallelism = 1,
                    EnsureOrdered = true,
                    SingleProducerConstrained = true
                });
            
            wrapIncomingLiquidity.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Wrap Incoming Liquidity completed, {Status}",
                    task.Status),
                incomingLiquidityPipeline.CancellationToken);
            
            var wrapEpoch = new TransformBlock<EpochInfo, BroadcastInfo>(
                epochInfo => new BroadcastInfo(epochInfo),
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = epochPipeline.CancellationToken,
                    MaxDegreeOfParallelism = 1,
                    EnsureOrdered = true,
                    SingleProducerConstrained = true
                });
            
            wrapEpoch.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Wrap Epoch completed, {Status}",
                    task.Status),
                epochPipeline.CancellationToken);
            
            var wrapFunding = new TransformBlock<FundingInfo, BroadcastInfo>(
                fundingInfo => new BroadcastInfo(fundingInfo),
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = fundingPipeline.CancellationToken,
                    MaxDegreeOfParallelism = 1,
                    EnsureOrdered = true,
                    SingleProducerConstrained = true
                });
            
            wrapFunding.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Wrap Funding completed, {Status}",
                    task.Status),
                fundingPipeline.CancellationToken);
            
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
            
            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };
            
            _discordSwapPipelineTarget = SetupDiscordSwapPipeline(
                broadcast,
                linkOptions, 
                swapPipeline.CancellationToken);
            
            _telegramSwapPipelineTarget = SetupTelegramSwapPipeline(
                broadcast,
                linkOptions,
                swapPipeline.CancellationToken);
            
            _twitterSwapPipelineTarget = SetupTwitterSwapPipeline(
                broadcast,
                linkOptions,
                swapPipeline.CancellationToken);
            
            swapSource.LinkTo(wrapSwaps, linkOptions);
            incomingLiquiditySource.LinkTo(wrapIncomingLiquidity, linkOptions);
            epochSource.LinkTo(wrapEpoch, linkOptions);
            fundingSource.LinkTo(wrapFunding, linkOptions);
            
            wrapSwaps.LinkTo(broadcast, linkOptions);
            wrapIncomingLiquidity.LinkTo(broadcast, linkOptions);
            wrapEpoch.LinkTo(broadcast, linkOptions);
            wrapFunding.LinkTo(broadcast, linkOptions);
        }
        
        private ITargetBlock<BroadcastInfo> SetupDiscordSwapPipeline(
            BroadcastBlock<BroadcastInfo> broadcast, 
            DataflowLinkOptions linkOptions,
            CancellationToken cancellationToken)
        {
            var pipeline = _discordConsumer.Build(cancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Discord Pipeline completed, {Status}",
                    task.Status),
                cancellationToken);
            
            broadcast.LinkTo(pipeline, linkOptions);

            return pipeline;
        }

        private ITargetBlock<BroadcastInfo> SetupTelegramSwapPipeline(
            BroadcastBlock<BroadcastInfo> broadcast, 
            DataflowLinkOptions linkOptions,
            CancellationToken cancellationToken)
        {
            var pipeline = _telegramConsumer.Build(cancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Telegram Pipeline completed, {Status}",
                    task.Status),
                cancellationToken);
            
            broadcast.LinkTo(pipeline, linkOptions);

            return pipeline;
        }
        
        private ITargetBlock<BroadcastInfo> SetupTwitterSwapPipeline(
            BroadcastBlock<BroadcastInfo> broadcast, 
            DataflowLinkOptions linkOptions,
            CancellationToken cancellationToken)
        {
            var pipeline = _twitterConsumer.Build(cancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Twitter Pipeline completed, {Status}",
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
                    _discordSwapPipelineTarget.Completion,
                    _telegramSwapPipelineTarget.Completion,
                    _twitterSwapPipelineTarget.Completion
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