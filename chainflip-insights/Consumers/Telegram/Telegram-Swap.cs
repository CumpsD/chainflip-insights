namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessSwap(
            SwapInfo swap,
            CancellationToken cancellationToken)
        {
            if (swap.DepositValueUsd < _configuration.TelegramSwapAmountThreshold &&
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.TelegramSwapAmountThreshold,
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
                    "Announcing Swap on Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker}{Broker} -> {ExplorerUrl}",
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
                    $"Δ **{swap.DeltaUsdFormatted.FormatDelta()}** " +
                    $"{(brokerExists ? $"@ **{broker}** " : string.Empty)}" +
                    $"// **[view swap on explorer]({_configuration.ExplorerSwapsUrl}{swap.Id})**";

                foreach (var channelId in _configuration.TelegramSwapInfoChannelId)
                {
                    var message = _telegramClient
                        .SendTextMessageAsync(
                            new ChatId(channelId),
                            text,
                            parseMode: ParseMode.Markdown,
                            disableNotification: true,
                            allowSendingWithoutReply: true,
                            cancellationToken: cancellationToken)
                        .GetAwaiter()
                        .GetResult();

                    _logger.LogInformation(
                        "Announcing Swap {SwapId} on Telegram as Message {MessageId}",
                        swap.Id,
                        message.MessageId);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}