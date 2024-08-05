namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.DiscordSwapAmountThreshold &&
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Discord: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.DiscordSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                var brokerExists = _brokers.TryGetValue(swap.Broker ?? string.Empty, out var broker);

                _logger.LogInformation(
                    "Announcing Swap on Discord: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker}{Broker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    brokerExists ? $" @ {broker}" : string.Empty,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped " +
                    $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                    $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                    $"Δ **{swap.DeltaUsdFormatted.FormatDelta()}** (*{swap.DeltaUsdPercentageFormatted}*) " +
                    $"{(brokerExists ? $"@ **{broker}** " : string.Empty)}" +
                    $"{(swap.IsBoosted ? $"⚡ **Boosted** for **{swap.BoostFeeUsdFormatted}** " : string.Empty)}" +
                    $"// **[view swap on explorer]({_configuration.ExplorerSwapsUrl}{swap.Id})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                if (swap.SourceAsset.Equals("flip", StringComparison.InvariantCultureIgnoreCase))
                {
                    message
                        .AddReactionAsync(_angryEmoji)
                        .GetAwaiter()
                        .GetResult();
                }

                if (swap.DestinationAsset.Equals("flip", StringComparison.InvariantCultureIgnoreCase))
                {
                    message
                        .AddReactionAsync(_tadaEmoji)
                        .GetAwaiter()
                        .GetResult();
                }

                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Discord as Message {MessageId}",
                    swap.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}