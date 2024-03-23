namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Funding;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class FullTelegramConsumer
    {
        private void ProcessFundingInfo(
            FundingInfo funding,
            CancellationToken cancellationToken)
        {
            if (funding.AmountConverted < _configuration.DiscordFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Full Telegram: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Full Telegram: {Validator} added {Amount} FLIP -> {EpochUrl}",
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
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Full Telegram as Message {MessageId}",
                    funding.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
    }
}