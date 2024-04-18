namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Infrastructure;
    using global::Discord;
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

                var burn = GetBurn(pastVolume.Date);
                
                var text =
                    $"📊 On **{pastVolume.Date}** we had a volume of " +
                    $"**${totalVolume.ToReadableMetric()}**, " +
                    $"**${pastVolume.NetworkFeesFormatted}** in network fees " +
                    $"and **${totalFees.ToReadableMetric()}** in liquidity provider fees!";

                if (!string.IsNullOrWhiteSpace(burn))
                    text += $" We also burned {burn} FLIP!";

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

        private string? GetBurn(string date)
        {
            try
            {
                using var dbContext = _dbContextFactory.CreateDbContext();
                
                var burnDate = DateTimeOffset.Parse(date);

                var burn = dbContext.BurnInfo.SingleOrDefault(x => x.BurnDate.Date == burnDate.Date);

                return (burn?.BurnAmount / 1000000000000000000)?.ToString("###,###,###,###,##0.00");
            }
            catch
            {
                return null;
            }
        }
    }
}