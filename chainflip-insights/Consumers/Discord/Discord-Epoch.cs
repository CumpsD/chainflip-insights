namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.Epoch;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.DiscordEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Discord. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Discord -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ **Epoch {epoch.Id} Started**! Current Minimum Active Bid is **{epoch.MinimumBondFormatted} FLIP**. " +
                    $"In total we have **{epoch.TotalBondFormatted}** FLIP bonded, with a maximum bid of **{epoch.MaxBidFormatted} FLIP**. " +
                    $"Last Epoch distributed **{epoch.PreviousEpoch.TotalRewardsFormatted}** FLIP as rewards. " +
                    $"// **[view authority set on explorer]({_configuration.ExplorerAuthorityUrl}{epoch.Id})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Discord as Message {MessageId}",
                    epoch.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}