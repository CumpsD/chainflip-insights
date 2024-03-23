namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.PastVolume;
    using Humanizer;
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

                var text =
                    $"📊 On {pastVolume.Date} we had a volume of " +
                    $"${totalVolume.ToMetric(decimals: 2)} and ${totalFees.ToMetric(decimals: 2)} in fees!\n" +
                    $"#chainflip #flip";

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
    }
}