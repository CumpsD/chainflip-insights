namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessBigStakedFlipInfo(BigStakedFlipInfo bigStakedFlipInfo)
        {
            if (bigStakedFlipInfo.Staked < _configuration.DiscordStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Discord: {Amount} FLIP",
                    _configuration.DiscordStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Discord: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 **Big stFLIP Alert**! " +
                    $"**{bigStakedFlipInfo.StakedFormatted} FLIP** just got staked! " +
                    $"// **[view transaction on explorer]({_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing staked flip {TransactionHash} on Discord as Message {MessageId}",
                    bigStakedFlipInfo.TransactionHash,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}