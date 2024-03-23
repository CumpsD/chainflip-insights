namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Linq;
    using ChainflipInsights.Feeders.SwapLimits;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessSwapLimitsInfo(SwapLimitsInfo swapLimits)
        {
            if (!_configuration.MastodonSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Mastodon: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Mastodon: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are:\n" +
                    $"{string.Join("\n", swapLimits.SwapLimits.Select(x => $"{x.SwapLimit} #{x.Asset.Ticker}"))}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap Limits on Mastodon as Message {MessageId}",
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}