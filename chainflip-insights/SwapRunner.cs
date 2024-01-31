namespace ChainflipInsights
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;

    public class SwapRunner
    {
        private readonly ILogger<SwapRunner> _logger;
        private readonly Pipeline<SwapInfo> _swapPipeline;
        
        private readonly DiscordConsumer _discordConsumer;
        private readonly TelegramConsumer _telegramConsumer;
        private readonly TwitterConsumer _twitterConsumer;

        private ITargetBlock<SwapInfo> _discordSwapPipelineTarget = null!;
        private ITargetBlock<SwapInfo> _telegramSwapPipelineTarget = null!;
        private ITargetBlock<SwapInfo> _twitterSwapPipelineTarget = null!;
        
        public SwapRunner(
            ILogger<SwapRunner> logger,
            Pipeline<SwapInfo> swapPipeline,
            DiscordConsumer discordConsumer,
            TelegramConsumer telegramConsumer,
            TwitterConsumer twitterConsumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _swapPipeline = swapPipeline ?? throw new ArgumentNullException(nameof(swapPipeline));
            _discordConsumer = discordConsumer ?? throw new ArgumentNullException(nameof(discordConsumer));
            _telegramConsumer = telegramConsumer ?? throw new ArgumentNullException(nameof(telegramConsumer));
            _twitterConsumer = twitterConsumer ?? throw new ArgumentNullException(nameof(twitterConsumer));

            SetupSwapPipeline();
        }

        private void SetupSwapPipeline()
        {
            // We take in SwapInfo
            var pipeline = _swapPipeline.Source;

            pipeline.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Swap Pipeline completed, {Status}",
                    task.Status),
                _swapPipeline.CancellationToken);

            // And broadcast them out to all consumers
            var broadcast = new BroadcastBlock<SwapInfo>(
                e => e,
                new DataflowBlockOptions
                {
                    CancellationToken = _swapPipeline.CancellationToken
                });

            broadcast.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Swap Broadcast completed, {Status}",
                    task.Status),
                _swapPipeline.CancellationToken);

            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };
            
            _discordSwapPipelineTarget = SetupDiscordSwapPipeline(broadcast, linkOptions);
            _telegramSwapPipelineTarget = SetupTelegramSwapPipeline(broadcast, linkOptions);
            _twitterSwapPipelineTarget = SetupTwitterSwapPipeline(broadcast, linkOptions);
            
            pipeline.LinkTo(broadcast, linkOptions);
        }

        private ITargetBlock<SwapInfo> SetupDiscordSwapPipeline(
            BroadcastBlock<SwapInfo> broadcast, 
            DataflowLinkOptions linkOptions)
        {
            var pipeline = _discordConsumer.Build(
                _swapPipeline.CancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Discord Swap Pipeline completed, {Status}",
                    task.Status),
                _swapPipeline.CancellationToken);
            
            broadcast.LinkTo(pipeline, linkOptions);

            return pipeline;
        }

        private ITargetBlock<SwapInfo> SetupTelegramSwapPipeline(
            BroadcastBlock<SwapInfo> broadcast, 
            DataflowLinkOptions linkOptions)
        {
            var pipeline = _telegramConsumer.Build(
                _swapPipeline.CancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Telegram Swap Pipeline completed, {Status}",
                    task.Status),
                _swapPipeline.CancellationToken);
            
            broadcast.LinkTo(pipeline, linkOptions);

            return pipeline;
        }
        
        private ITargetBlock<SwapInfo> SetupTwitterSwapPipeline(
            BroadcastBlock<SwapInfo> broadcast, 
            DataflowLinkOptions linkOptions)
        {
            var pipeline = _twitterConsumer.Build(
                _swapPipeline.CancellationToken);

            pipeline.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Twitter Swap Pipeline completed, {Status}",
                    task.Status),
                _swapPipeline.CancellationToken);
            
            broadcast.LinkTo(pipeline, linkOptions);

            return pipeline;
        }

        public IEnumerable<Task> Start()
        {
            _logger.LogInformation(
                "Starting {TaskName}", nameof(SwapRunner));

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