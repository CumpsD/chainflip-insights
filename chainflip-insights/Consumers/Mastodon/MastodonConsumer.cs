namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.BigStakedFlip;
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
    using Mastonet;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MastodonConsumer : IConsumer
    {
        private readonly ILogger<MastodonConsumer> _logger;
        private readonly MastodonClient _mastodonClient;
        private readonly BotConfiguration _configuration;
        private readonly Dictionary<string, string> _brokers;

        public MastodonConsumer(
            ILogger<MastodonConsumer> logger,
            IOptions<BotConfiguration> options,
            MastodonClient mastodonClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mastodonClient = mastodonClient ?? throw new ArgumentNullException(nameof(mastodonClient));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            
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
                    if (!_configuration.EnableMastodon.Value)
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
                    
                    if (input.BigStakedFlipInfo != null)
                        ProcessBigStakedFlipInfo(input.BigStakedFlipInfo);
                    
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
                    "Mastodon Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }

        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.MastodonSwapAmountThreshold && 
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Mastodon: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.MastodonSwapAmountThreshold,
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
                    "Announcing Swap on Mastodon: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped {_configuration.ExplorerSwapsUrl}{swap.Id}\n" +
                    $"➡️ {swap.DepositAmountFormatted} #{swap.SourceAsset} (${swap.DepositValueUsdFormatted})\n" +
                    $"⬅️ {swap.EgressAmountFormatted} #{swap.DestinationAsset} (${swap.EgressValueUsdFormatted})\n" +
                    $"{(brokerExists ? $"☑️ via {broker}\n" : string.Empty)}" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Mastodon as Message {MessageId}",
                    swap.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

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

        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.MastodonEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Mastodon. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Mastodon -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                var text =
                    $"⏰ Epoch {epoch.Id} Started {_configuration.ExplorerAuthorityUrl}{epoch.Id}\n" +
                    $"➖ Minimum Bid is {epoch.MinimumBondFormatted} #FLIP\n" +
                    $"➕ Maximum Bid is {epoch.MaxBidFormatted} #FLIP\n" +
                    $"🧮 Total bonded is {epoch.TotalBondFormatted} #FLIP\n" +
                    $"💰 Last Epoch distributed {epoch.PreviousEpoch.TotalRewardsFormatted} #FLIP in rewards\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Epoch {EpochId} on Mastodon as Message {MessageId}",
                    epoch.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.MastodonFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Mastodon: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.MastodonFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Mastodon: {Validator} added {Amount} FLIP -> {EpochUrl}",
                    funding.Id,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                var text =
                    $"🪙 Validator {funding.Validator} added {funding.AmountFormatted} #FLIP! {string.Format(_configuration.ValidatorUrl, funding.ValidatorName)}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Mastodon as Message {MessageId}",
                    funding.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessRedemptionInfo(RedemptionInfo redemption)
        {
            if (redemption.AmountConverted < _configuration.MastodonRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Mastodon: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.MastodonRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Mastodon: {Validator} redeemed {Amount} #FLIP -> {EpochUrl}",
                    redemption.Id,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                var text =
                    $"💸 Validator {redemption.Validator} redeemed {redemption.AmountFormatted} FLIP! {string.Format(_configuration.ValidatorUrl, redemption.ValidatorName)}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Mastodon as Message {MessageId}",
                    redemption.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.MastodonCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Mastodon. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
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
                    "Announcing CEX Movements for {Date} on Mastodon: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);
                
                var text =
                    $"🔀 CEX Movements for {cexMovement.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {cexMovement.MovementInFormatted} #FLIP moved towards CEX\n" +
                    $"⬇️ {cexMovement.MovementOutFormatted} #FLIP moved towards DEX\n" +
                    $"{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢" )} {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX" )} gained {cexMovement.TotalMovementFormatted} #FLIP\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing CEX Movements {Day} on Mastodon as Message {MessageId}",
                    cexMovement.DayOfYear,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessSwapLimitsInfo(SwapLimitsInfo swapLimits)
        {
            if (!_configuration.MastodonSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Mastodon: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Mastodon: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                
                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are:\n" +
                    $"{string.Join("\n", swapLimits.SwapLimits.Select(x => $"{x.SwapLimit} #{x.Asset.Ticker}"))}\n" +
                    $"#chainflip #flip";
                
                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Swap Limits on Mastodon as Message {MessageId}",
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessPastVolumeInfo(PastVolumeInfo pastVolume)
        {
            if (!_configuration.MastodonPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Mastodon. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Mastodon: {Pairs} Past 24h Volume Pairs.",
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
                
                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Volume on Mastodon as Message {MessageId}",
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
        
        private void ProcessStakedFlipInfo(StakedFlipInfo stakedFlip)
        {
            if (!_configuration.MastodonStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Mastodon. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
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
                    "Announcing Staked Flip for {Date} on Mastodon: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);
                
                var text =
                    $"🏦 stFLIP Movements for {stakedFlip.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {stakedFlip.StakedFormatted} #FLIP got staked\n" +
                    $"⬇️ {stakedFlip.UnstakedFormatted} #FLIP got unstaked\n" +
                    $"{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢" )} {stakedFlip.TotalMovementFormatted} #FLIP got {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "unstaked" : "staked" )}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Staked Flip {Date} on Mastodon as Message {MessageId}",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
        
        private void ProcessBrokerOverviewInfo(BrokerOverviewInfo brokerOverview)
        {
            if (!_configuration.MastodonBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Mastodon. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Mastodon: {Brokers} top brokers.",
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
   
                var status = _mastodonClient
                    .PublishStatus(
                        text.ToString(),
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Broker Overview {Date} on Mastodon as Message {MessageId}",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    status.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
        
        private void ProcessBigStakedFlipInfo(BigStakedFlipInfo bigStakedFlipInfo)
        {
            if (bigStakedFlipInfo.Staked < _configuration.MastodonStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Mastodon: {Amount} FLIP",
                    _configuration.MastodonStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Mastodon: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 Big #stFLIP Alert! " +
                    $"{bigStakedFlipInfo.StakedFormatted} #FLIP just got staked! " +
                    $"// {_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}\n" +
                    $"#chainflip #flip";
                
                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing staked flip {TransactionHash} on Mastodon as Message {MessageId}",
                    bigStakedFlipInfo.TransactionHash,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}