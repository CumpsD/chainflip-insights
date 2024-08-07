namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure;
    using Microsoft.Extensions.Logging;

    public partial class TwitterConsumer
    {
        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.TwitterSwapAmountThreshold &&
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Twitter: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.TwitterSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                return;
            }

            try
            {
                var brokerExists = _brokers.TryGetValue(swap.Broker ?? string.Empty, out var broker);

                var brokerName = brokerExists
                    ? string.IsNullOrWhiteSpace(broker!.Twitter)
                        ? broker.Name
                        : broker.Twitter
                    : string.Empty;

                _logger.LogInformation(
                    "Announcing Swap on Twitter: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped {_configuration.ExplorerSwapsUrl}{swap.Id}\n" +
                    $"📥 {swap.DepositAmountFormatted} ${swap.SourceAsset} (${swap.DepositValueUsdFormatted})\n" +
                    $"📤 {swap.EgressAmountFormatted} ${swap.DestinationAsset} (${swap.EgressValueUsdFormatted})\n" +
                    $"🔺 {swap.DeltaUsdFormatted.FormatDelta()} ({swap.DeltaUsdPercentageFormatted})\n" +
                    $"{(brokerExists ? $"🏦 via {brokerName}\n" : string.Empty)}" +
                    $"{(swap.IsBoosted ? $"⚡ Boosted for ${swap.BoostFeeUsdFormatted}\n" : string.Empty)}" +
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