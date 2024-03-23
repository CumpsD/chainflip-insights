namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Text;
    using ChainflipInsights.Feeders.BrokerOverview;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessBrokerOverviewInfo(BrokerOverviewInfo brokerOverview)
        {
            if (!_configuration.DiscordBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Discord. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Discord: {Brokers} top brokers.",
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

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text.ToString(),
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Broker Overview {Date} on Discord as Message {MessageId}",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}