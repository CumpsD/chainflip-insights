namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using System.Text;
    using ChainflipInsights.Feeders.DailyLpOverview;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessDailyLpOverviewInfo(DailyLpOverviewInfo dailyLpOverviewInfo)
        {
            if (!_configuration.DiscordDailyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily LP Overview disabled for Discord. {Date}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Daily LP Overview for {Date} on Discord.",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💼 Top LPs for **{dailyLpOverviewInfo.Date:yyyy-MM-dd}** are in!");

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
                    "Announcing Daily LP Overview {Date} on Discord as Message {MessageId}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"),
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}