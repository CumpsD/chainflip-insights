namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Text;
    using ChainflipInsights.Feeders.WeeklySwapOverview;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessWeeklySwapOverviewInfo(WeeklySwapOverviewInfo weeklySwapOverviewInfo)
        {
            if (!_configuration.DiscordWeeklySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly Swap Overview disabled for Discord. {StartDate} -> {EndDate}",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly Swap Overview for {StartDate} -> {EndDate} on Discord.",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💵 Weekly Top Swaps for **{weeklySwapOverviewInfo.StartDate:yyyy-MM-dd}** -> **{weeklySwapOverviewInfo.EndDate:yyyy-MM-dd}** are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < weeklySwapOverviewInfo.Swaps.Count; i++)
                {
                    var swap = weeklySwapOverviewInfo.Swaps[i].Swap;
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
                    "Announcing Weekly Swap Overview {StartDate} -> {EndDate} on Discord as Message {MessageId}",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"),
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}