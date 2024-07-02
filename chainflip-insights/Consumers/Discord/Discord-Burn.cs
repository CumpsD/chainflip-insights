namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.Burn;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessBurnInfo(BurnInfo burn)
        {
            if (!_configuration.DiscordBurnEnabled.Value)
            {
                _logger.LogInformation(
                    "Burn disabled for Discord. Burn {BurnBlock} -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} on Discord -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                var text =
                    $"🔥 Burned **{burn.FlipBurnedFormatted} FLIP**{(string.IsNullOrWhiteSpace(burn.FlipBurnedFormattedUsd) ? string.Empty : $" (**${burn.FlipBurnedFormattedUsd}**)")}! " +
                    $"// **[view block on explorer]({_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} on Discord as Message {MessageId}",
                    burn.LastSupplyUpdateBlock,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}