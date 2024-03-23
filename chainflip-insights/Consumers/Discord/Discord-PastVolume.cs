namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using ChainflipInsights.Feeders.PastVolume;
    using global::Discord;
    using Humanizer;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessPastVolumeInfo(PastVolumeInfo pastVolume)
        {
            if (!_configuration.DiscordPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Discord. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Discord: {Pairs} Past 24h Volume Pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                var totalVolume = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Value);

                var totalFees = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Fees);

                var text =
                    $"📊 On **{pastVolume.Date}** we had a volume of " +
                    $"**${totalVolume.ToMetric(decimals: 2)}** and **${totalFees.ToMetric(decimals: 2)}** in fees!";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Volume {Day} on Discord as Message {MessageId}",
                    pastVolume.Date,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}