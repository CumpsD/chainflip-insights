namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Text;
    using System.Threading;
    using ChainflipInsights.Feeders.BrokerOverview;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessBrokerOverviewInfo(
            BrokerOverviewInfo brokerOverview,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Telegram. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Telegram: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                var text = new StringBuilder();
                text.AppendLine($"🏭 Top Brokers for **{brokerOverview.Date:yyyy-MM-dd}** are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < brokerOverview.Brokers.Count; i++)
                {
                    var brokerInfo = brokerOverview.Brokers[i];
                    var brokerExists = _brokers.TryGetValue(brokerInfo.Ss58, out var broker);
                    var name = brokerExists ? broker : brokerInfo.Ss58;

                    text.AppendLine(
                        $"{emojis[i]} **{name}** (**${brokerInfo.VolumeFormatted}** Volume, **${brokerInfo.FeesFormatted}** Fees)");
                }

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramSwapInfoChannelId.Value),
                        text.ToString(),
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Broker Overview {Date} on Telegram as Message {MessageId}",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}