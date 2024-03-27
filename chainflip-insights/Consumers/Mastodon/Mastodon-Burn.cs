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
                    "Burn disabled for Mastodon. Burn {BurnBlock} ({BurnBlockHash}) -> {BlockUrl}",
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
                    "Announcing Burn {BurnBlock} ({BurnBlockHash}) on Mastodon -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                var text =
                    $"🔥 Burned {burn.FlipBurnedFormatted} #FLIP {_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} ({BurnBlockHash}) on Mastodon as Message {MessageId}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}