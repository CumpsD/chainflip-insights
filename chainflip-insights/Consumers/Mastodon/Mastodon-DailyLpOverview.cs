namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Linq;
    using System.Text;
    using ChainflipInsights.Feeders.DailyLpOverview;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessDailyLpOverviewInfo(DailyLpOverviewInfo dailyLpOverviewInfo)
        {
            if (!_configuration.MastodonDailyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily LP Overview disabled for Mastodon. {Date}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Daily LP Overview for {Date} on Mastodon.",
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
                    var lp = lps[i];

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"${lp.Value} " +
                        $"@ {lp.Key}");
                }
                
                text.AppendLine("#chainflip #flip");

                var status = _mastodonClient
                    .PublishStatus(
                        text.ToString(),
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Daily LP Overview {Date} on Mastodon as Message {MessageId}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}