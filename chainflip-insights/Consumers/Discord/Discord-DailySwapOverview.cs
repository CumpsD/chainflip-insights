namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Text;
    using ChainflipInsights.Feeders.DailySwapOverview;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessDailySwapOverviewInfo(DailySwapOverviewInfo dailySwapOverviewInfo)
        {
            if (!_configuration.DiscordDailySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily Swap Overview disabled for Discord. {Date}",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Daily Swap Overview for {Date} on Discord.",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💵 Top Swaps for **{dailySwapOverviewInfo.Date:yyyy-MM-dd}** are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < dailySwapOverviewInfo.Swaps.Count; i++)
                {
                    var swap = dailySwapOverviewInfo.Swaps[i];
                    var brokerExists = _brokers.TryGetValue(swap.Broker ?? string.Empty, out var broker);

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                        $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                        $"{(brokerExists ? $"@ **{broker}** " : string.Empty)}" +
                        $"// **[#{swap.Id}]({_configuration.ExplorerSwapsUrl}{swap.Id})**");
                }

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);
                
                var message = infoChannel
                    .SendMessageAsync(
                        text.ToString(),
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Daily Swap Overview {Date} on Discord as Message {MessageId}",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"),
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}