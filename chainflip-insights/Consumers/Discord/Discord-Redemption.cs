namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.Redemption;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessRedemptionInfo(RedemptionInfo redemption)
        {
            if (redemption.AmountConverted < _configuration.DiscordRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Discord: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Discord: {Validator} redeemed {Amount} FLIP -> {EpochUrl}",
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

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Discord as Message {MessageId}",
                    redemption.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}