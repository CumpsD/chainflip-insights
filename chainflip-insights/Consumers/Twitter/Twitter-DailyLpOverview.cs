namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using ChainflipInsights.Feeders.DailyLpOverview;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessDailyLpOverviewInfo(DailyLpOverviewInfo dailyLpOverviewInfo)
        {
            if (!_configuration.TwitterDailyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily LP Overview disabled for Twitter. {Date}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Daily LP Overview for {Date} on Twitter.",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💼 Top LPs for {dailyLpOverviewInfo.Date:yyyy-MM-dd} are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                var lps = dailyLpOverviewInfo.LpVolume.Take(5).ToList();
                for (var i = 0; i < lps.Count; i++)
                {
                    var lp = lps[i].Value;

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"${lp.VolumeFilled} ({lp.VolumePercentage}) " +
                        $"@ {lp.Twitter}");
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