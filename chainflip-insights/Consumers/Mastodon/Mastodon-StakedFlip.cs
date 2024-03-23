namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.StakedFlip;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessStakedFlipInfo(StakedFlipInfo stakedFlip)
        {
            if (!_configuration.MastodonStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Mastodon. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
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
                    "Announcing Staked Flip for {Date} on Mastodon: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                var text =
                    $"🏦 stFLIP Movements for {stakedFlip.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {stakedFlip.StakedFormatted} #FLIP got staked\n" +
                    $"⬇️ {stakedFlip.UnstakedFormatted} #FLIP got unstaked\n" +
                    $"{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢")} {stakedFlip.TotalMovementFormatted} #FLIP got {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "unstaked" : "staked")}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Staked Flip {Date} on Mastodon as Message {MessageId}",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}