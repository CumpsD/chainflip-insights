namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Epoch;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessEpochInfo(
            EpochInfo epoch,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Telegram. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Telegram -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ **Epoch {epoch.Id} Started**! Current Minimum Active Bid is **{epoch.MinimumBondFormatted} FLIP**. " +
                    $"In total we have **{epoch.TotalBondFormatted}** FLIP bonded, with a maximum bid of **{epoch.MaxBidFormatted} FLIP**. " +
                    $"Last Epoch distributed **{epoch.PreviousEpoch.TotalRewardsFormatted}** FLIP as rewards. " +
                    $"// **[view authority set on explorer]({_configuration.ExplorerAuthorityUrl}{epoch.Id})**";

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
                    "Announcing Epoch {Epoch} on Telegram as Message {MessageId}",
                    epoch.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}