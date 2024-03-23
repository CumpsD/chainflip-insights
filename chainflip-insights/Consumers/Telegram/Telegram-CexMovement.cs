namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Threading;
    using ChainflipInsights.Feeders.CexMovement;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class TelegramConsumer
    {
        private void ProcessCexMovementInfo(
            CexMovementInfo cexMovement,
            CancellationToken cancellationToken)
        {
            if (!_configuration.TelegramCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Telegram. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing CEX Movements for {Date} on Telegram: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                var text =
                    $"🔀 CEX Movements for **{cexMovement.Date:yyyy-MM-dd}** are in! " +
                    $"**{cexMovement.MovementInFormatted} FLIP** moved towards CEX, **{cexMovement.MovementOutFormatted} FLIP** moved towards DEX. " +
                    $"In total, **{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX")}** gained **{cexMovement.TotalMovementFormatted} FLIP** {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢")}";

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
                    "Announcing CEX Movements {Day} on Telegram as Message {MessageId}",
                    cexMovement.DayOfYear,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }
        }
    }
}