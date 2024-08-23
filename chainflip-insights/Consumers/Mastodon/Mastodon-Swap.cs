namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.MastodonSwapAmountThreshold &&
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Mastodon: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.MastodonSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                return;
            }

            try
            {
                var brokerExists = _brokers.TryGetValue(swap.Broker ?? string.Empty, out var broker);

                _logger.LogInformation(
                    "Announcing Swap on Mastodon: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped {_configuration.ExplorerSwapsUrl}{swap.Id}\n" +
                    $"📥 {swap.DepositAmountFormatted} #{swap.SourceAsset} (${swap.DepositValueUsdFormatted})\n" +
                    $"📤 {swap.EgressAmountFormatted} #{swap.DestinationAsset} (${swap.EgressValueUsdFormatted})\n" +
                    $"{(swap.ProtocolDeltaUsdPercentage < 0 ? "🟢" : "🔴")} Delta {(swap.BrokerFeeUsdFormatted != null ? "(ex. Broker):" : ":")} {swap.ProtocolDeltaUsdFormatted.FormatDelta()} ({swap.ProtocolDeltaUsdPercentageFormatted})\n" +
                    $"{(swap.BrokerFeeUsdFormatted != null ? $"💵 Broker: {swap.BrokerFeeUsdFormatted.FormatDelta()} ({swap.BrokerFeePercentageFormatted})\n" : string.Empty)}" +
                    $"{(brokerExists ? $"🏦 via {broker}\n" : string.Empty)}" +
                    $"{(swap.IsBoosted ? $"⚡ Boosted for ${swap.BoostFeeUsdFormatted}\n" : string.Empty)}" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Mastodon as Message {MessageId}",
                    swap.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}