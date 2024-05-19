namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.StakedFlip;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessStakedFlipInfo(
            StakedFlipInfo stakedFlip,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Telegram. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Staked Flip for {Date} on Telegram: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                var text =
                    $"🏦 stFLIP Movements for **{stakedFlip.Date:yyyy-MM-dd}** are in! " +
                    $"**{stakedFlip.StakedFormatted} FLIP** got staked, **{stakedFlip.UnstakedFormatted} FLIP** got unstaked. " +
                    $"In total, **{stakedFlip.TotalMovementFormatted} FLIP** was **{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreStaked ? "staked" : "unstaked")}** {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢")}";

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
                        "Announcing Staked Flip {Date} on Telegram as Message {MessageId}",
                        stakedFlip.Date.ToString("yyyy-MM-dd"),
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