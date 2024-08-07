namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Linq;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Infrastructure;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessPastVolumeInfo(PastVolumeInfo pastVolume)
        {
            if (!_configuration.MastodonPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Mastodon. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Mastodon: {Pairs} Past 24h Volume Pairs.",
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
                    $"📊 On {pastVolume.Date} we had a volume of " +
                    $"${totalVolume.ToReadableMetric()}, " +
                    $"${pastVolume.NetworkFeesFormatted} in network fees " +
                    $"and ${totalFees.ToReadableMetric()} in liquidity provider fees.";

                if (!string.IsNullOrWhiteSpace(burn))
                    text += $" We also burned {burn} #FLIP!";
                
                text += "\n#chainflip #flip";
                
                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Volume on Mastodon as Message {MessageId}",
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
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