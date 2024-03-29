namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.Burn;
    using global::Telegram.Bot;
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

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramSwapInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Burn {BurnBlock} ({BurnBlockHash}) on Telegram as Message {MessageId}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}