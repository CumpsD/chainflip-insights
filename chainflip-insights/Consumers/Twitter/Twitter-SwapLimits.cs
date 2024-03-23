namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.SwapLimits;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessSwapLimitsInfo(SwapLimitsInfo swapLimits)
        {
            if (!_configuration.TwitterSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Twitter: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Twitter: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are:\n" +
                    $"{string.Join("\n", swapLimits.SwapLimits.Select(x => $"{x.SwapLimit} ${x.Asset.Ticker}"))}\n" +
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