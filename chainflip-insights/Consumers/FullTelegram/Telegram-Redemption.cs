namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Redemption;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class FullTelegramConsumer
    {
        private void ProcessRedemptionInfo(
            RedemptionInfo redemption,
            CancellationToken cancellationToken)
        {
            if (redemption.AmountConverted < _configuration.DiscordRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Full Telegram: {Validator} redeemed {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Full Telegram: {Validator} redeemed {Amount} FLIP -> {EpochUrl}",
                    redemption.Id,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));

                var validator =
                    string.IsNullOrWhiteSpace(redemption.ValidatorAlias)
                        ? $"**`{redemption.ValidatorName}`**"
                        : $"**`{redemption.ValidatorName}`** (**{redemption.ValidatorAlias}**)";

                var text =
                    $"💸 Validator {validator} redeemed **{redemption.AmountFormatted} FLIP**! " +
                    $"// **[view validator on explorer]({string.Format(_configuration.ValidatorUrl, redemption.ValidatorName)})**";

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
                    "Announcing Redemption {RedemptionId} on Full Telegram as Message {MessageId}",
                    redemption.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
    }
}