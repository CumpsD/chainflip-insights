namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Text;
    using ChainflipInsights.Feeders.BrokerOverview;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessBrokerOverviewInfo(BrokerOverviewInfo brokerOverview)
        {
            if (!_configuration.MastodonBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Mastodon. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Mastodon: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                var text = new StringBuilder();
                text.AppendLine($"🏭 Top Brokers for {brokerOverview.Date:yyyy-MM-dd} are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < brokerOverview.Brokers.Count; i++)
                {
                    var brokerInfo = brokerOverview.Brokers[i];
                    var brokerExists = _brokers.TryGetValue(brokerInfo.Ss58, out var broker);
                    var name = brokerExists ? broker : brokerInfo.Ss58;

                    text.AppendLine(
                        $"{emojis[i]} {name} (${brokerInfo.VolumeFormatted} Volume, ${brokerInfo.FeesFormatted} Fees)");
                }

                text.AppendLine("#chainflip #flip");

                var status = _mastodonClient
                    .PublishStatus(
                        text.ToString(),
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Broker Overview {Date} on Mastodon as Message {MessageId}",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}