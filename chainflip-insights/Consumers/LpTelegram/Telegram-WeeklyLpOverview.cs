namespace ChainflipInsights.Consumers.LpTelegram
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using ChainflipInsights.Feeders.WeeklyLpOverview;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class LpTelegramConsumer
    {
        private void ProcessWeeklyLpOverviewInfo(
            WeeklyLpOverviewInfo weeklyLpOverviewInfo,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordWeeklyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly LP Overview disabled for LP Telegram. {StartDate} -> {EndDate}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly LP Overview for {StartDate} -> {EndDate} on LP Telegram.",
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
                    var lp = lps[i];

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"**${lp.Value}** " +
                        $"@ **{lp.Key}**");
                }

                var message = _telegramClient
                    .SendMessageAsync(
                        new SendMessageRequest
                        {
                            ChatId = new ChatId(_configuration.TelegramLpChannelId.Value),
                            Text = text.ToString(),
                            ParseMode = ParseMode.Markdown,
                            DisableNotification = true,
                            LinkPreviewOptions = new LinkPreviewOptions
                            {
                                IsDisabled = true
                            },
                            ReplyParameters = new ReplyParameters
                            {
                                AllowSendingWithoutReply = true,
                            }
                        },
                        cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Weekly LP Overview {StartDate} -> {EndDate} on LP Telegram as Message {MessageId}",
                    weeklyLpOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklyLpOverviewInfo.EndDate.ToString("yyyy-MM-dd"),
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LP Telegram meh.");
            }
        }
    }
}