namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Text;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.DailySwapOverview;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessDailySwapOverviewInfo(DailySwapOverviewInfo dailySwapOverviewInfo)
        {
            if (!_configuration.MastodonDailySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily Swap Overview disabled for Mastodon. {Date}",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Daily Swap Overview for {Date} on Mastodon.",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💲 Top Swaps for {dailySwapOverviewInfo.Date:yyyy-MM-dd} are in!");

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
                    "Announcing Daily Swap Overview {Date} on Mastodon as Message {MessageId}",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}