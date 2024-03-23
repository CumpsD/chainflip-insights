namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.CexMovement;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.MastodonCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Mastodon. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
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
                    "Announcing CEX Movements for {Date} on Mastodon: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                var text =
                    $"🔀 CEX Movements for {cexMovement.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {cexMovement.MovementInFormatted} #FLIP moved towards CEX\n" +
                    $"⬇️ {cexMovement.MovementOutFormatted} #FLIP moved towards DEX\n" +
                    $"{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢")} {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX")} gained {cexMovement.TotalMovementFormatted} #FLIP\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing CEX Movements {Day} on Mastodon as Message {MessageId}",
                    cexMovement.DayOfYear,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}