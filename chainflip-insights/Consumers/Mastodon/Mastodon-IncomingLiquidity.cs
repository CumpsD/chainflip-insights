namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.Liquidity;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessIncomingLiquidityInfo(IncomingLiquidityInfo liquidity)
        {
            if (liquidity.DepositValueUsd < _configuration.MastodonLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Incoming Liquidity did not meet threshold (${Threshold}) for Mastodon: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.MastodonLiquidityAmountThreshold,
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Incoming Liquidity on Mastodon: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                var text =
                    $"💵 Liquidity Added! {_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}\n" +
                    $"📈 An extra {liquidity.DepositAmountFormatted} #{liquidity.SourceAsset} (${liquidity.DepositValueUsdFormatted}) is available!\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Incoming Liquidity {LiquidityId} on Mastodon as Message {MessageId}",
                    liquidity.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}