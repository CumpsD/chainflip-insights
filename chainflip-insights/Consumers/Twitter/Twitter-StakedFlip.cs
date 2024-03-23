namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.StakedFlip;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessStakedFlipInfo(StakedFlipInfo stakedFlip)
        {
            if (!_configuration.TwitterStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Twitter. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Staked Flip for {Date} on Twitter: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                var text =
                    $"🏦 stFLIP Movements for {stakedFlip.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {stakedFlip.StakedFormatted} $FLIP got staked\n" +
                    $"⬇️ {stakedFlip.UnstakedFormatted} $FLIP got unstaked\n" +
                    $"{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢")} {stakedFlip.TotalMovementFormatted} $FLIP got {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "unstaked" : "staked")}\n" +
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