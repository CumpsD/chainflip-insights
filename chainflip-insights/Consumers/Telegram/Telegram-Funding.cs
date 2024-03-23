namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Funding;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessFundingInfo(
            FundingInfo funding,
            CancellationToken cancellationToken)
        {
            if (funding.AmountConverted < _configuration.TelegramFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Telegram: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.TelegramFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Telegram: {Validator} added {Amount} FLIP -> {EpochUrl}",
                    funding.Id,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                var validator =
                    string.IsNullOrWhiteSpace(funding.ValidatorAlias)
                        ? $"**`{funding.ValidatorName}`**"
                        : $"**`{funding.ValidatorName}`** (**{funding.ValidatorAlias}**)";

                var text =
                    $"🪙 Validator {validator} added **{funding.AmountFormatted} FLIP**! " +
                    $"// **[view validator on explorer]({string.Format(_configuration.ValidatorUrl, funding.ValidatorName)})**";

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
                    "Announcing Funding {FundingId} on Telegram as Message {MessageId}",
                    funding.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}