namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using ChainflipInsights.Feeders.WeeklyLpOverview;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessWeeklyLpOverviewInfo(WeeklyLpOverviewInfo weeklyLpOverviewInfo)
        {
            if (!_configuration.TwitterWeeklyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly LP Overview disabled for Twitter. {StartDate} -> {EndDate}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly LP Overview for {StartDate} -> {EndDate} on Twitter.",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💼 Weekly Top LPs for {weeklyLpOverviewInfo.StartDate:yyyy-MM-dd} -> {weeklyLpOverviewInfo.EndDate:yyyy-MM-dd} are in!");

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
                };

                var lps = weeklyLpOverviewInfo.LpVolume.Take(10).ToList();
                for (var i = 0; i < lps.Count; i++)
                {
                    var lp = lps[i];

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"${lp.Value} " +
                        $"@ {lp.Key}");
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