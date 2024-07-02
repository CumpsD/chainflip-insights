namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.Burn;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessBurnInfo(BurnInfo burn)
        {
            if (!_configuration.MastodonBurnEnabled.Value)
            {
                _logger.LogInformation(
                    "Burn disabled for Mastodon. Burn {BurnBlock} -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} on Mastodon -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                var text =
                    $"🔥 Burned {burn.FlipBurnedFormatted} #FLIP{(string.IsNullOrWhiteSpace(burn.FlipBurnedFormattedUsd) ? string.Empty : $" (${burn.FlipBurnedFormattedUsd})")} {_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} on Mastodon as Message {MessageId}",
                    burn.LastSupplyUpdateBlock,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}