namespace ChainflipInsights
{
    using System;
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
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

        public Runner(
            ILogger<Runner> logger,
            Pipeline<SwapInfo> swapPipeline,
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline,
            DiscordConsumer discordConsumer,
            TelegramConsumer telegramConsumer,
            TwitterConsumer twitterConsumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            // _swapPipeline = swapPipeline ?? throw new ArgumentNullException(nameof(swapPipeline));
            _discordConsumer = discordConsumer ?? throw new ArgumentNullException(nameof(discordConsumer));
            _telegramConsumer = telegramConsumer ?? throw new ArgumentNullException(nameof(telegramConsumer));
            _twitterConsumer = twitterConsumer ?? throw new ArgumentNullException(nameof(twitterConsumer));

            SetupPipelines(
                swapPipeline,
                incomingLiquidityPipeline);
        }

        private void SetupPipelines(
            Pipeline<SwapInfo> swapPipeline, 
            Pipeline<IncomingLiquidityInfo> incomingLiquidityPipeline)
        {
            
        }
    }
}