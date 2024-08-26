namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using System.Text;
    using ChainflipInsights.Feeders.WeeklyLpOverview;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessWeeklyLpOverviewInfo(WeeklyLpOverviewInfo weeklyLpOverviewInfo)
        {
            if (!_configuration.DiscordWeeklyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly LP Overview disabled for Discord. {StartDate} -> {EndDate}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly LP Overview for {StartDate} -> {EndDate} on Discord.",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💼 Weekly Top LPs for **{weeklyLpOverviewInfo.StartDate:yyyy-MM-dd}** -> **{weeklyLpOverviewInfo.EndDate:yyyy-MM-dd}** are in!");

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
                    var lp = lps[i].Value;

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"**${lp.VolumeFilled}** (*{lp.VolumePercentage}*) " +
                        $"@ **{lp.Name}**");
                }

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);
                
                var message = infoChannel
                    .SendMessageAsync(
                        text.ToString(),
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Weekly LP Overview {StartDate} -> {EndDate} on Discord as Message {MessageId}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"),
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}