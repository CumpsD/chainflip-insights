namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Text;
    using ChainflipInsights.Feeders.WeeklySwapOverview;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessWeeklySwapOverviewInfo(WeeklySwapOverviewInfo weeklySwapOverviewInfo)
        {
            if (!_configuration.MastodonWeeklySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly Swap Overview disabled for Mastodon. {StartDate} -> {EndDate}",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly Swap Overview for {StartDate} -> {EndDate} on Mastodon.",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💵 Weekly Top Swaps for {weeklySwapOverviewInfo.StartDate:yyyy-MM-dd} -> {weeklySwapOverviewInfo.EndDate:yyyy-MM-dd} are in!");

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
                        $"{swap.DepositAmountFormatted} #{swap.SourceAsset} (${swap.DepositValueUsdFormatted}) → " +
                        $"{swap.EgressAmountFormatted} #{swap.DestinationAsset} (${swap.EgressValueUsdFormatted}) " +
                        $"{(brokerExists ? $"@ {broker}" : string.Empty)}");
                }
                
                text.AppendLine("#chainflip #flip");

                var status = _mastodonClient
                    .PublishStatus(
                        text.ToString(),
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Weekly Swap Overview {StartDate} -> {EndDate} on Mastodon as Message {MessageId}",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}