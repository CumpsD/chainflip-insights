namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using ChainflipInsights.Feeders.DailySwapOverview;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessDailySwapOverviewInfo(DailySwapOverviewInfo dailySwapOverviewInfo)
        {
            if (!_configuration.TwitterDailySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily Swap Overview disabled for Twitter. {Date}",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Daily Swap Overview for {Date} on Twitter.",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💵 Top Swaps for {dailySwapOverviewInfo.Date:yyyy-MM-dd} are in!");

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
                    var name = brokerExists
                        ? string.IsNullOrWhiteSpace(broker!.Twitter)
                            ? broker.Name
                            : broker.Twitter
                        : string.Empty;
                    
                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"{swap.DepositAmountFormatted} ${swap.SourceAsset} (${swap.DepositValueUsdFormatted}) → " +
                        $"{swap.EgressAmountFormatted} ${swap.DestinationAsset} (${swap.EgressValueUsdFormatted}) " +
                        $"{(brokerExists ? $"@ {name}" : string.Empty)}");
                }
                
                text.AppendLine("#chainflip $flip");

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