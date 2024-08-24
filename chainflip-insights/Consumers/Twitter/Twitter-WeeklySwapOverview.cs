namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using ChainflipInsights.Feeders.WeeklySwapOverview;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessWeeklySwapOverviewInfo(WeeklySwapOverviewInfo weeklySwapOverviewInfo)
        {
            if (!_configuration.TwitterWeeklySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly Swap Overview disabled for Twitter. {StartDate} -> {EndDate}",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly Swap Overview for {StartDate} -> {EndDate} on Twitter.",
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
                    var name = brokerExists
                        ? string.IsNullOrWhiteSpace(broker!.Twitter)
                            ? broker.Name
                            : broker.Twitter
                        : string.Empty;
                    
                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"{swap.DepositAmountFormatted} ${swap.SourceAsset} → " +
                        $"{swap.EgressAmountFormatted} ${swap.DestinationAsset} " +
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