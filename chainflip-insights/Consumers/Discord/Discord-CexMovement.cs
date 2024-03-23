namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.CexMovement;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.DiscordCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Discord. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing CEX Movements for {Date} on Discord: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                var text =
                    $"🔀 CEX Movements for **{cexMovement.Date:yyyy-MM-dd}** are in! " +
                    $"**{cexMovement.MovementInFormatted} FLIP** moved towards CEX, **{cexMovement.MovementOutFormatted} FLIP** moved towards DEX. " +
                    $"In total, **{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX")}** gained **{cexMovement.TotalMovementFormatted} FLIP** {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢")}";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing CEX Movements {Day} on Discord as Message {MessageId}",
                    cexMovement.DayOfYear,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}