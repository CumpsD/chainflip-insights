namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Linq;
    using System.Threading;
    using ChainflipInsights.Feeders.SwapLimits;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class FullTelegramConsumer
    {
        private void ProcessSwapLimitsInfo(
            SwapLimitsInfo swapLimits,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Full Telegram: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Full Telegram: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are: " +
                    $"{string.Join(", ", swapLimits.SwapLimits.Select(x => $"**{x.SwapLimit} {x.Asset.Ticker}**"))}";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap Limits on Full Telegram as Message {MessageId}",
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
    }
}