namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.Burn;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessBurnInfo(BurnInfo burn)
        {
            if (!_configuration.TwitterBurnEnabled.Value)
            {
                _logger.LogInformation(
                    "Burn disabled for Twitter. Burn {BurnBlock} ({BurnBlockHash}) -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }
            
            if (burn.BurnSkipped)
            {
                _logger.LogInformation(
                    "Burn did not meet burn threshold. Burn {BurnBlock} ({BurnBlockHash}) -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} ({BurnBlockHash}) on Twitter -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                var text =
                    $"🔥 Burned {burn.FlipBurnedFormatted} $FLIP{(string.IsNullOrWhiteSpace(burn.FlipBurnedFormattedUsd) ? string.Empty : $" (${burn.FlipBurnedFormattedUsd})")} {_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}\n" +
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