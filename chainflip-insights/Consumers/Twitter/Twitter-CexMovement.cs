namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.CexMovement;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.TwitterCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Twitter. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing CEX Movements for {Date} on Twitter: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                var text =
                    $"🔀 CEX Movements for {cexMovement.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {cexMovement.MovementInFormatted} $FLIP moved towards CEX\n" +
                    $"⬇️ {cexMovement.MovementOutFormatted} $FLIP moved towards DEX\n" +
                    $"{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢")} {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX")} gained {cexMovement.TotalMovementFormatted} $FLIP\n" +
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