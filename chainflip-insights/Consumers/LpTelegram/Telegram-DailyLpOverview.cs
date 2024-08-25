namespace ChainflipInsights.Consumers.LpTelegram
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using ChainflipInsights.Feeders.DailyLpOverview;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class LpTelegramConsumer
    {
        private void ProcessDailyLpOverviewInfo(
            DailyLpOverviewInfo dailyLpOverviewInfo,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordDailyLpOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily LP Overview disabled for LP Telegram. {Date}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Daily LP Overview for {Date} on LP Telegram.",
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
                    "Announcing Daily LP Overview {Date} on LP Telegram as Message {MessageId}",
                    dailyLpOverviewInfo.Date.ToString("yyyy-MM-dd"),
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LP Telegram meh.");
            }
        }
    }
}