namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessBigStakedFlipInfo(
            BigStakedFlipInfo bigStakedFlipInfo,
            CancellationToken cancellationToken)
        {
            if (bigStakedFlipInfo.Staked < _configuration.TelegramStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Telegram: {Amount} FLIP",
                    _configuration.TelegramStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Telegram: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 **Big stFLIP Alert**! " +
                    $"**{bigStakedFlipInfo.StakedFormatted} FLIP** just got staked! " +
                    $"// **[view transaction on explorer]({_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash})**";

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
                    "Announcing staked flip {TransactionHash} on Telegram as Message {MessageId}",
                    bigStakedFlipInfo.TransactionHash,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}