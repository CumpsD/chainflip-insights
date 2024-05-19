namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Burn;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessBurnInfo(
            BurnInfo burn,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramBurnEnabled.Value)
            {
                _logger.LogInformation(
                    "Burn disabled for Telegram. Burn {BurnBlock} ({BurnBlockHash}) -> {BlockUrl}",
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
                    "Announcing Burn {BurnBlock} ({BurnBlockHash}) on Telegram -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                var text =
                    burn.BurnSkipped
                        ? $"🔥 Burn was **skipped** due to not meeting burn threshold, **{burn.FlipToBurnFormatted} FLIP**{(string.IsNullOrWhiteSpace(burn.FlipToBurnFormattedUsd) ? string.Empty : $" (**${burn.FlipToBurnFormattedUsd}**)")} waiting to be burned! " +
                          $"// **[view block on explorer]({_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock})**"
                        : $"🔥 Burned **{burn.FlipBurnedFormatted} FLIP**{(string.IsNullOrWhiteSpace(burn.FlipBurnedFormattedUsd) ? string.Empty : $" (**${burn.FlipBurnedFormattedUsd}**)")}! " +
                          $"// **[view block on explorer]({_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock})**";

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

                    _logger.LogInformation(
                        "Announcing Burn {BurnBlock} ({BurnBlockHash}) on Telegram as Message {MessageId}",
                        burn.LastSupplyUpdateBlock,
                        burn.LastSupplyUpdateBlockHash,
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