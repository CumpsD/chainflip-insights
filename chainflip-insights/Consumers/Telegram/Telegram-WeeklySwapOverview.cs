namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Text;
    using System.Threading;
    using ChainflipInsights.Feeders.WeeklySwapOverview;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessWeeklySwapOverviewInfo(
            WeeklySwapOverviewInfo weeklySwapOverviewInfo,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramWeeklySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Weekly Swap Overview disabled for Telegram. {StartDate} -> {EndDate}",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Weekly Swap Overview for {StartDate} -> {EndDate} on Telegram.",
                    weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                    weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💵 Weekly Top Swaps for **{weeklySwapOverviewInfo.StartDate:yyyy-MM-dd}** -> **{weeklySwapOverviewInfo.EndDate:yyyy-MM-dd}** are in!");

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

                    text.AppendLine(
                        $"{emojis[i]} " +
                        $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** → " +
                        $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** " +
                        $"// **[#{swap.Id}]({_configuration.ExplorerSwapsUrl}{swap.Id})**");
                }

                foreach (var channelId in _configuration.TelegramSwapInfoChannelId)
                {
                    var message = _telegramClient
                        .SendMessageAsync(
                            new SendMessageRequest
                            {
                                ChatId = new ChatId(channelId),
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
                        "Announcing Weekly Swap Overview {StartDate} -> {EndDate} on Telegram as Message {MessageId}",
                        weeklySwapOverviewInfo.StartDate.ToString("yyyy-MM-dd"),
                        weeklySwapOverviewInfo.EndDate.ToString("yyyy-MM-dd"),
                        message.MessageId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}