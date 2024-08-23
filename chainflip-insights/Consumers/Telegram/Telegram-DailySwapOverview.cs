namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Text;
    using System.Threading;
    using ChainflipInsights.Feeders.DailySwapOverview;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessDailySwapOverviewInfo(
            DailySwapOverviewInfo dailySwapOverviewInfo,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramDailySwapOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Daily Swap Overview disabled for Telegram. {Date}",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Daily Swap Overview for {Date} on Telegram.",
                    dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"));

                var text = new StringBuilder();
                text.AppendLine($"💵 Top Swaps for **{dailySwapOverviewInfo.Date:yyyy-MM-dd}** are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < dailySwapOverviewInfo.Swaps.Count; i++)
                {
                    var swap = dailySwapOverviewInfo.Swaps[i];

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
                        "Announcing Daily Swap Overview {Date} on Telegram as Message {MessageId}",
                        dailySwapOverviewInfo.Date.ToString("yyyy-MM-dd"),
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