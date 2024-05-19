namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class FullTelegramConsumer
    {
        private void ProcessBigStakedFlipInfo(
            BigStakedFlipInfo bigStakedFlipInfo,
            CancellationToken cancellationToken)
        {
            if (bigStakedFlipInfo.Staked < _configuration.DiscordStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Full Telegram: {Amount} FLIP",
                    _configuration.DiscordStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Full Telegram: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 **Big stFLIP Alert**! " +
                    $"**{bigStakedFlipInfo.StakedFormatted} FLIP** just got staked! " +
                    $"// **[view transaction on explorer]({_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash})**";

                var message = _telegramClient
                    .SendMessageAsync(
                        new SendMessageRequest
                        {
                            ChatId = new ChatId(_configuration.TelegramInfoChannelId.Value),
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
                    "Announcing staked flip {TransactionHash} on Full Telegram as Message {MessageId}",
                    bigStakedFlipInfo.TransactionHash,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
    }
}