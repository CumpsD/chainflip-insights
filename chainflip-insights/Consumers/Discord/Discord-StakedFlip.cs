namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.StakedFlip;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessStakedFlipInfo(StakedFlipInfo stakedFlip)
        {
            if (!_configuration.DiscordStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Discord. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Staked Flip for {Date} on Discord: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                var text =
                    $"🏦 stFLIP Movements for **{stakedFlip.Date:yyyy-MM-dd}** are in! " +
                    $"**{stakedFlip.StakedFormatted} FLIP** got staked, **{stakedFlip.UnstakedFormatted} FLIP** got unstaked. " +
                    $"In total, **{stakedFlip.TotalMovementFormatted} FLIP** was **{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreStaked ? "staked" : "unstaked")}** {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢")}";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Staked Flip {Date} on Discord as Message {MessageId}",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}