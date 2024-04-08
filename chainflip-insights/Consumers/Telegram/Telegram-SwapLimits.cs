namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Linq;
    using System.Threading;
    using ChainflipInsights.Feeders.SwapLimits;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessSwapLimitsInfo(
            SwapLimitsInfo swapLimits,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Telegram: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Telegram: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are: " +
                    $"{string.Join(", ", swapLimits.SwapLimits.Select(x => $"**{x.SwapLimit} {x.Asset.Ticker}**"))}";

                foreach (var channelId in _configuration.TelegramSwapInfoChannelId)
                {
                    var message = _telegramClient
                        .SendTextMessageAsync(
                            new ChatId(channelId),
                            text,
                            parseMode: ParseMode.Markdown,
                            disableNotification: true,
                            allowSendingWithoutReply: true,
                            cancellationToken: cancellationToken)
                        .GetAwaiter()
                        .GetResult();

                    _logger.LogInformation(
                        "Announcing Swap Limits on Telegram as Message {MessageId}",
                        message.MessageId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}