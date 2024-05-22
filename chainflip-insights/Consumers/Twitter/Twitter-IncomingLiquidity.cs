namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.Liquidity;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessIncomingLiquidityInfo(IncomingLiquidityInfo liquidity)
        {
            if (liquidity.DepositValueUsd < _configuration.TwitterLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Incoming Liquidity did not meet threshold (${Threshold}) for Twitter: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.TwitterLiquidityAmountThreshold,
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Incoming Liquidity on Twitter: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                var text =
                    $"💵 Liquidity Added! {_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}\n" +
                    $"📈 An extra {liquidity.DepositAmountFormatted} #{liquidity.SourceAsset} (${liquidity.DepositValueUsdFormatted}) is available!\n" +
                    $"#chainflip $flip";

                _twitterClient.Execute
                    .AdvanceRequestAsync(x =>
                    {
                        x.Query.Url = "https://api.twitter.com/2/tweets";
                        x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                        x.Query.HttpContent = JsonContent.Create(
                            new TweetV2PostRequest { Text = text },
                            mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
                    })
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Twitter meh.");
            }
        }
    }
}