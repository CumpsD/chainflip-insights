namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.Epoch;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.MastodonEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Mastodon. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Mastodon -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ Epoch {epoch.Id} Started {_configuration.ExplorerAuthorityUrl}{epoch.Id}\n" +
                    $"➖ Minimum Bid is {epoch.MinimumBondFormatted} #FLIP\n" +
                    $"➕ Maximum Bid is {epoch.MaxBidFormatted} #FLIP\n" +
                    $"🧮 Total bonded is {epoch.TotalBondFormatted} #FLIP\n" +
                    $"💰 Last Epoch distributed {epoch.PreviousEpoch.TotalRewardsFormatted} #FLIP in rewards\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Epoch {EpochId} on Mastodon as Message {MessageId}",
                    epoch.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}