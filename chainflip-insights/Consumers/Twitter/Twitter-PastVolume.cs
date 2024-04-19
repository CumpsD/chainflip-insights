namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Infrastructure;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessPastVolumeInfo(PastVolumeInfo pastVolume)
        {
            if (!_configuration.TwitterPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Twitter. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Twitter: {Pairs} Past 24h Volume Pairs.",
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
                    $"${totalVolume.ToReadableMetric()} with " +
                    $"${pastVolume.NetworkFeesFormatted} in network fees " +
                    $"and ${totalFees.ToReadableMetric()} in liquidity provider fees.";

                if (!string.IsNullOrWhiteSpace(burn))
                    text += $" We also burned {burn} $FLIP!";
                
                text += "\n#chainflip #flip";
                
                _twitterClient.Execute
                    .AdvanceRequestAsync(x =>
                    {
                        x.Query.Url = "https://api.twitter.com/2/tweets";
                        x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                        x.Query.HttpContent = JsonContent.Create(
                            new TweetV2PostRequest { Text = text },
                            mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
                    })
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Twitter meh.");
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