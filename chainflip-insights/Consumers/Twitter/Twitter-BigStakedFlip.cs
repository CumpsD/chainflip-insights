namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessBigStakedFlipInfo(BigStakedFlipInfo bigStakedFlipInfo)
        {
            if (bigStakedFlipInfo.Staked < _configuration.TwitterStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Twitter: {Amount} FLIP",
                    _configuration.TwitterStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Twitter: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 Big $stFLIP Alert! " +
                    $"{bigStakedFlipInfo.StakedFormatted} $FLIP just got staked! " +
                    $"// {_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}\n" +
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