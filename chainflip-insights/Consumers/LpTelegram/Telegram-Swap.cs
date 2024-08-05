namespace ChainflipInsights.Consumers.LpTelegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class LpTelegramConsumer
    {
        private void ProcessSwap(
            SwapInfo swap,
            CancellationToken cancellationToken)
        {
            var isUsdtSwap =
                swap.DestinationAsset.Contains("usdt", StringComparison.InvariantCultureIgnoreCase) ||
                swap.SourceAsset.Contains("usdt", StringComparison.InvariantCultureIgnoreCase);
            
            var isArbUsdc =
                swap.DestinationAsset.Contains("arbusdc", StringComparison.InvariantCultureIgnoreCase) ||
                swap.SourceAsset.Contains("arbusdc", StringComparison.InvariantCultureIgnoreCase);
            
            if (!isUsdtSwap && !isArbUsdc)
            {
                _logger.LogInformation(
                    "Swap did not meet asset (USDT/arbUSDC) for LP Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
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
                    "Announcing Swap on LP Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker}{Broker} -> {ExplorerUrl}",
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

                var message = _telegramClient
                    .SendMessageAsync(
                        new SendMessageRequest
                        {
                            ChatId = new ChatId(_configuration.TelegramLpChannelId.Value),
                            Text = text,
                            ParseMode = ParseMode.Markdown,
                            DisableNotification = true,
                            LinkPreviewOptions = new LinkPreviewOptions
                            {
                                IsDisabled = true
                            },
                            ReplyParameters = new ReplyParameters
                            {
                                AllowSendingWithoutReply = true,
                            }
                        },
                        cancellationToken)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Swap {SwapId} on LP Telegram as Message {MessageId}",
                    swap.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LP Telegram meh.");
            }
        }
    }
}