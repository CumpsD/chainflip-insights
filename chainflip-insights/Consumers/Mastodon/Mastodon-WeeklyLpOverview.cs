namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Linq;
    using System.Text;
    using ChainflipInsights.Feeders.WeeklyLpOverview;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessWeeklyLpOverviewInfo(WeeklyLpOverviewInfo weeklyLpOverviewInfo)
        {
            if (!_configuration.MastodonWeeklyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly LP Overview disabled for Mastodon. {StartDate} -> {EndDate}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly LP Overview for {StartDate} -> {EndDate} on Mastodon.",
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
                
                text.AppendLine("#chainflip #flip");

                var status = _mastodonClient
                    .PublishStatus(
                        text.ToString(),
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Weekly LP Overview {StartDate} -> {EndDate} on Mastodon as Message {MessageId}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}