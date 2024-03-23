namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.StakedFlip;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Humanizer;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Tweetinvi;

    /// <summary>
    /// There are a lot more fields according to:
    /// https://developer.twitter.com/en/docs/twitter-api/tweets/manage-tweets/api-reference/post-tweets
    /// but these are the ones we care about for our use case.
    /// </summary>
    public class TweetV2PostRequest
    {
        /// <summary>
        /// The text of the tweet to post.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class TwitterConsumer : IConsumer
    {
        private readonly ILogger<TwitterConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly TwitterClient _twitterClient;
        private readonly Dictionary<string, string> _brokers;

        public TwitterConsumer(
            ILogger<TwitterConsumer> logger,
            IOptions<BotConfiguration> options,
            TwitterClient twitterClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _twitterClient = twitterClient ?? throw new ArgumentNullException(nameof(twitterClient));
            
            _brokers = _configuration
                .Brokers
                .ToDictionary(
                    x => x.Address,
                    x => x.Name);
        }

        public ITargetBlock<BroadcastInfo> Build(
            CancellationToken cancellationToken)
        {
            var announcer = BuildAnnouncer(cancellationToken);
            return new EncapsulatingTarget<BroadcastInfo, BroadcastInfo>(announcer, announcer);
        }

        private ActionBlock<BroadcastInfo> BuildAnnouncer(
            CancellationToken cancellationToken)
        {
            var logging = new ActionBlock<BroadcastInfo>(
                input =>
                {
                    if (!_configuration.EnableTwitter.Value)
                        return;

                    if (input.SwapInfo != null)
                        ProcessSwap(input.SwapInfo);
                    
                    if (input.IncomingLiquidityInfo != null)
                        ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo);
                    
                    if (input.EpochInfo != null)
                        ProcessEpochInfo(input.EpochInfo);
                    
                    if (input.FundingInfo != null)
                        ProcessFundingInfo(input.FundingInfo);
                    
                    if (input.RedemptionInfo != null)
                        ProcessRedemptionInfo(input.RedemptionInfo);
                    
                    if (input.CexMovementInfo != null)
                        ProcessCexMovementInfo(input.CexMovementInfo);
                    
                    if (input.SwapLimitsInfo != null)
                        ProcessSwapLimitsInfo(input.SwapLimitsInfo);
                    
                    if (input.PastVolumeInfo != null)
                        ProcessPastVolumeInfo(input.PastVolumeInfo);
                    
                    if (input.StakedFlipInfo != null)
                        ProcessStakedFlipInfo(input.StakedFlipInfo);
                    
                    if (input.BrokerOverviewInfo != null)
                        ProcessBrokerOverviewInfo(input.BrokerOverviewInfo);
                    
                    Task
                        .Delay(1500, cancellationToken)
                        .GetAwaiter()
                        .GetResult();
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
                });

            logging.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Twitter Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }

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
                
                _logger.LogInformation(
                    "Announcing Swap on Twitter: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped {_configuration.ExplorerSwapsUrl}{swap.Id}\n" +
                    $"➡️ {swap.DepositAmountFormatted} ${swap.SourceAsset} (${swap.DepositValueUsdFormatted})\n" +
                    $"⬅️ {swap.EgressAmountFormatted} ${swap.DestinationAsset} (${swap.EgressValueUsdFormatted})\n" +
                    $"{(brokerExists ? $"☑️ via {broker}\n" : string.Empty)}" +
                    $"#chainflip #flip";

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
                    $"#chainflip #flip";

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

        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.TwitterEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Twitter. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Twitter -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                var text =
                    $"⏰ Epoch {epoch.Id} Started {_configuration.ExplorerAuthorityUrl}{epoch.Id}\n" +
                    $"➖ Minimum Bid is {epoch.MinimumBondFormatted} $FLIP\n" +
                    $"➕ Maximum Bid is {epoch.MaxBidFormatted} $FLIP\n" +
                    $"🧮 Total bonded is {epoch.TotalBondFormatted} $FLIP\n" +
                    $"💰 Last Epoch distributed {epoch.PreviousEpoch.TotalRewardsFormatted} $FLIP in rewards\n" +
                    $"#chainflip #flip";

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

        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.TwitterFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Twitter: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.TwitterFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Twitter: {Validator} added {Amount} FLIP -> {EpochUrl}",
                    funding.Id,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                var text =
                    $"🪙 Validator {funding.Validator} added {funding.AmountFormatted} FLIP! {string.Format(_configuration.ValidatorUrl, funding.ValidatorName)}\n" +
                    $"#chainflip #flip";

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

        private void ProcessRedemptionInfo(RedemptionInfo redemption)
        {
            if (redemption.AmountConverted < _configuration.TwitterRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Twitter: {Validator} added {Amount} $FLIP -> {ExplorerUrl}",
                    _configuration.TwitterRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Twitter: {Validator} redeemed {Amount} FLIP -> {EpochUrl}",
                    redemption.Id,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                var text =
                    $"💸 Validator {redemption.Validator} redeemed {redemption.AmountFormatted} $FLIP! {string.Format(_configuration.ValidatorUrl, redemption.ValidatorName)}\n" +
                    $"#chainflip #flip";

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

        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.TwitterCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Twitter. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
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
                    "Announcing CEX Movements for {Date} on Twitter: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);
                
                var text =
                    $"🔀 CEX Movements for {cexMovement.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {cexMovement.MovementInFormatted} $FLIP moved towards CEX\n" +
                    $"⬇️ {cexMovement.MovementOutFormatted} $FLIP moved towards DEX\n" +
                    $"{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢" )} {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX" )} gained {cexMovement.TotalMovementFormatted} $FLIP\n" +
                    $"#chainflip #flip";
                
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

        private void ProcessSwapLimitsInfo(SwapLimitsInfo swapLimits)
        {
            if (!_configuration.TwitterSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Twitter: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Twitter: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                
                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are:\n" +
                    $"{string.Join("\n", swapLimits.SwapLimits.Select(x => $"{x.SwapLimit} ${x.Asset.Ticker}"))}\n" +
                    $"#chainflip #flip";
                
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

        private void ProcessPastVolumeInfo(PastVolumeInfo pastVolume)
        {
            if (!_configuration.TwitterPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Twitter. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Twitter: {Pairs} Past 24h Volume Pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                var totalVolume = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Value);
                
                var totalFees = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Fees);

                var text =
                    $"📊 On {pastVolume.Date} we had a volume of " +
                    $"${totalVolume.ToMetric(decimals: 2)} and ${totalFees.ToMetric(decimals: 2)} in fees!\n" +
                    $"#chainflip #flip";
                
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
        
        private void ProcessStakedFlipInfo(StakedFlipInfo stakedFlip)
        {
            if (!_configuration.TwitterStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Twitter. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
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
                    "Announcing Staked Flip for {Date} on Twitter: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);
                
                var text =
                    $"🏦 stFLIP Movements for {stakedFlip.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {stakedFlip.StakedFormatted} $FLIP got staked\n" +
                    $"⬇️ {stakedFlip.UnstakedFormatted} $FLIP got unstaked\n" +
                    $"{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢" )} {stakedFlip.TotalMovementFormatted} $FLIP got {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "unstaked" : "staked" )}\n" +
                    $"#chainflip #flip";
                
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
        
        private void ProcessBrokerOverviewInfo(BrokerOverviewInfo brokerOverview)
        {
            if (!_configuration.TwitterBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Twitter. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Twitter: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);
                
                var text = new StringBuilder();
                text.AppendLine($"🏭 Top Brokers for {brokerOverview.Date:yyyy-MM-dd} are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < brokerOverview.Brokers.Count; i++)
                {
                    var brokerInfo = brokerOverview.Brokers[i];
                    var brokerExists = _brokers.TryGetValue(brokerInfo.Ss58, out var broker);
                    var name = brokerExists ? broker : brokerInfo.Ss58;

                    text.AppendLine($"{emojis[i]} {name} (${brokerInfo.VolumeFormatted} Volume, ${brokerInfo.FeesFormatted} Fees)");
                }

                text.AppendLine("#chainflip #flip");
                
                _twitterClient.Execute
                    .AdvanceRequestAsync(x =>
                    {
                        x.Query.Url = "https://api.twitter.com/2/tweets";
                        x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                        x.Query.HttpContent = JsonContent.Create(
                            new TweetV2PostRequest { Text = text.ToString() },
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