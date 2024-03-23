namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.Funding;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.DiscordFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Discord: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Discord: {Validator} added {Amount} FLIP -> {EpochUrl}",
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

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Discord as Message {MessageId}",
                    funding.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}