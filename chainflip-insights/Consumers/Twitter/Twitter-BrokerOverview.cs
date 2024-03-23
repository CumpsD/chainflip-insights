namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using ChainflipInsights.Feeders.BrokerOverview;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessBrokerOverviewInfo(BrokerOverviewInfo brokerOverview)
        {
            if (!_configuration.TwitterBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Twitter. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Twitter: {Brokers} top brokers.",
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
                    var name = brokerExists
                        ? string.IsNullOrWhiteSpace(broker!.Twitter)
                            ? broker.Name
                            : broker.Twitter
                        : brokerInfo.Ss58;

                    text.AppendLine(
                        $"{emojis[i]} {name} (${brokerInfo.VolumeFormatted} Volume, ${brokerInfo.FeesFormatted} Fees)");
                }

                text.AppendLine("#chainflip #flip");

                _twitterClient.Execute
                    .AdvanceRequestAsync(x =>
                    {
                        x.Query.Url = "https://api.twitter.com/2/tweets";
                        x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                        x.Query.HttpContent = JsonContent.Create(
                            new TweetV2PostRequest { Text = text.ToString() },
                            mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
                    })
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Twitter meh.");
            }
        }
    }
}