namespace ChainflipInsights.Consumers.Telegram
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

                // var text =
                //     $"{swap.Emoji} Swapped " +
                //     $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                //     $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                //     $"Δ **{swap.DeltaUsdFormatted.FormatDelta()}** (*{swap.DeltaUsdPercentageFormatted}*) " +
                //     $"{(brokerExists ? $"@ **{broker}** " : string.Empty)}" +
                //     $"{(swap.IsBoosted ? $"⚡ **Boosted** for **${swap.BoostFeeUsdFormatted}** " : string.Empty)}" +
                //     $"// **[view swap on explorer]({_configuration.ExplorerSwapsUrl}{swap.Id})**";

                var text =
                    $"{swap.Emoji} Swapped **{_configuration.ExplorerSwapsUrl}{swap.Id}**\n" +
                    $"📥 **{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*)\n" +
                    $"📤 **{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*)\n" +
                    $"{(swap.ProtocolDeltaUsdPercentage < 0 ? "🟢" : "🔴")} Delta{(swap.BrokerFeeUsdFormatted != null ? " (ex. Broker)" : string.Empty)}: **{swap.ProtocolDeltaUsdFormatted.FormatDelta()}** (*{swap.ProtocolDeltaUsdPercentageFormatted}*)\n" +
                    $"{(swap.BrokerFeeUsdFormatted != null ? $"💵 Broker: **{swap.BrokerFeeUsdFormatted.FormatDelta()}** (*{swap.BrokerFeePercentageFormatted}*)\n" : string.Empty)}" +
                    $"{(brokerExists ? $"🏦 via **{broker}**\n" : string.Empty)}" +
                    $"{(swap.IsBoosted ? $"⚡ **Boosted** for **${swap.BoostFeeUsdFormatted}**\n" : string.Empty)}";

                text = text.TrimEnd('\n');
                
                foreach (var channelId in _configuration.TelegramSwapInfoChannelId)
                {
                    var message = _telegramClient
                        .SendMessageAsync(
                            new SendMessageRequest
                            {
                                ChatId = new ChatId(channelId),
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
                    
                    if (swap.SourceAsset.Equals("flip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _telegramClient
                            .SetMessageReactionAsync(
                                new SetMessageReactionRequest
                                {
                                    ChatId = new ChatId(channelId),
                                    MessageId = message.MessageId,
                                    Reaction = new[] { _angryEmoji },
                                    IsBig = false
                                },
                                cancellationToken)
                            .GetAwaiter()
                            .GetResult();
                    }

                    if (swap.DestinationAsset.Equals("flip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _telegramClient
                            .SetMessageReactionAsync(
                                new SetMessageReactionRequest
                                {
                                    ChatId = new ChatId(channelId),
                                    MessageId = message.MessageId,
                                    Reaction = new[] { _tadaEmoji },
                                    IsBig = false
                                },
                                cancellationToken)
                            .GetAwaiter()
                            .GetResult();
                    }
                    
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