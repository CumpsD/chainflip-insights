namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using ChainflipInsights.Feeders.SwapLimits;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessSwapLimitsInfo(SwapLimitsInfo swapLimits)
        {
            if (!_configuration.DiscordSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Discord: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Discord: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are: " +
                    $"{string.Join(", ", swapLimits.SwapLimits.Select(x => $"**{x.SwapLimit} {x.Asset.Ticker}**"))}";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap Limits on Discord as Message {MessageId}",
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}