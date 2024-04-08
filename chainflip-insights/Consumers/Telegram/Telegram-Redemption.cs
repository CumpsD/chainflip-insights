namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Redemption;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessRedemptionInfo(
            RedemptionInfo redemption,
            CancellationToken cancellationToken)
        {
            if (redemption.AmountConverted < _configuration.TelegramRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Telegram: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.TelegramRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Telegram: {Validator} redeemed {Amount} FLIP -> {EpochUrl}",
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
                        "Announcing Redemption {RedemptionId} on Telegram as Message {MessageId}",
                        redemption.Id,
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