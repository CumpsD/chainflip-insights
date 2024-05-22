namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.Epoch;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.TwitterEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Twitter. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Twitter -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ Epoch {epoch.Id} Started {_configuration.ExplorerAuthorityUrl}{epoch.Id}\n" +
                    $"➖ Minimum Bid is {epoch.MinimumBondFormatted} $FLIP\n" +
                    $"➕ Maximum Bid is {epoch.MaxBidFormatted} $FLIP\n" +
                    $"🧮 Total bonded is {epoch.TotalBondFormatted} $FLIP\n" +
                    $"💰 Last Epoch distributed {epoch.PreviousEpoch.TotalRewardsFormatted} $FLIP in rewards\n" +
                    $"#chainflip $flip";

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