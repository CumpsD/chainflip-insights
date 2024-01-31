namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class TelegramConsumer
    {
        private readonly ILogger<TelegramConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly TelegramBotClient _telegramClient;

        public TelegramConsumer(
            ILogger<TelegramConsumer> logger,
            IOptions<BotConfiguration> options,
            TelegramBotClient telegramClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _telegramClient = telegramClient ?? throw new ArgumentNullException(nameof(telegramClient));
        }
        
        public ITargetBlock<SwapInfo> Build(
            CancellationToken ct)
        {
            var announcer = BuildAnnouncer(ct);
            return new EncapsulatingTarget<SwapInfo, SwapInfo>(announcer, announcer);
        }

        private ActionBlock<SwapInfo> BuildAnnouncer(
            CancellationToken ct)
        {
            var logging = new ActionBlock<SwapInfo>(
                swap =>
                {
                    if (!_configuration.EnableTelegram.Value)
                        return;
                    
                    if (swap.DepositValueUsd < _configuration.TelegramAmountThreshold)
                        return;
                    
                    try
                    {
                        _logger.LogInformation(
                            "Announcing Swap on Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                            swap.DepositAmountFormatted,
                            swap.SourceAsset,
                            swap.EgressAmountFormatted,
                            swap.DestinationAsset,
                            $"{_configuration.ExplorerSwapsUrl}{swap.Id}");
                        
                        var text =
                            $"{swap.Emoji} Swapped " +
                            $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                            $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                            $"// **[view swap on explorer]({_configuration.ExplorerSwapsUrl}{swap.Id})**";
                        
                        _telegramClient
                            .SendTextMessageAsync(
                                new ChatId(_configuration.TelegramSwapInfoChannelId.Value),
                                text,
                                parseMode: ParseMode.Markdown,
                                disableNotification: true,
                                allowSendingWithoutReply: true,
                                cancellationToken: ct)
                            .GetAwaiter()
                            .GetResult();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Telegram meh.");
                    }
                    
                    Task
                        .Delay(1500, ct)
                        .GetAwaiter()
                        .GetResult();
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = ct
                });

            logging.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Telegram Logging completed, {Status}",
                    task.Status),
                ct);

            return logging;
        }
    }
}