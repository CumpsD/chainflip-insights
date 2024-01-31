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
                    if (!_configuration.EnableTelegram.Value)
                        return;

                    if (input.SwapInfo != null)
                        ProcessSwap(input.SwapInfo, cancellationToken);

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
                task => _logger.LogDebug(
                    "Telegram Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }

        private void ProcessSwap(
            SwapInfo swap,
            CancellationToken cancellationToken)
        {
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

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramSwapInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Telegram as Message {MessageId}",
                    swap.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}