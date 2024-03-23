namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using ChainflipInsights.Feeders.Liquidity;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessIncomingLiquidityInfo(IncomingLiquidityInfo liquidity)
        {
            if (liquidity.DepositValueUsd < _configuration.DiscordLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Incoming Liquidity did not meet threshold (${Threshold}) for Discord: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.DiscordLiquidityAmountThreshold,
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing Incoming Liquidity on Discord: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                var text =
                    $"💵 **Liquidity Added**! An extra " +
                    $"**{liquidity.DepositAmountFormatted} {liquidity.SourceAsset}** (*${liquidity.DepositValueUsdFormatted}*) is available! " +
                    $"// **[view incoming liquidity on explorer]({_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Incoming Liquidity {LiquidityId} on Discord as Message {MessageId}",
                    liquidity.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}