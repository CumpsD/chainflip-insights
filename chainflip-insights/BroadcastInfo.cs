namespace ChainflipInsights
{
    using System;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.CfeVersion;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.StakedFlip;
    using ChainflipInsights.Feeders.Substrate;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;

    public class BroadcastInfo
    {
        public SubstrateInfo? SubstrateInfo { get; }
        public SwapInfo? SwapInfo { get; }
        public IncomingLiquidityInfo? IncomingLiquidityInfo { get; }
        public EpochInfo? EpochInfo { get; }
        public FundingInfo? FundingInfo { get; }
        public RedemptionInfo? RedemptionInfo { get; }
        public CexMovementInfo? CexMovementInfo { get; }
        public CfeVersionsInfo? CfeVersionInfo { get; }
        public SwapLimitsInfo? SwapLimitsInfo { get; }
        public PastVolumeInfo? PastVolumeInfo { get; }
        public StakedFlipInfo? StakedFlipInfo { get; }
        public BrokerOverviewInfo? BrokerOverviewInfo { get; }
        public BigStakedFlipInfo? BigStakedFlipInfo { get; }

        public BroadcastInfo(SubstrateInfo substrateInfo)
            => SubstrateInfo = substrateInfo ?? throw new ArgumentNullException(nameof(substrateInfo));
        
        public BroadcastInfo(SwapInfo swapInfo) 
            => SwapInfo = swapInfo ?? throw new ArgumentNullException(nameof(swapInfo));

        public BroadcastInfo(IncomingLiquidityInfo incomingLiquidityInfo) 
            => IncomingLiquidityInfo = incomingLiquidityInfo ?? throw new ArgumentNullException(nameof(incomingLiquidityInfo));

        public BroadcastInfo(EpochInfo epochInfo)
            => EpochInfo = epochInfo ?? throw new ArgumentNullException(nameof(epochInfo));

        public BroadcastInfo(FundingInfo fundingInfo)
            => FundingInfo = fundingInfo ?? throw new ArgumentNullException(nameof(fundingInfo));

        public BroadcastInfo(RedemptionInfo redemptionInfo)
            => RedemptionInfo = redemptionInfo ?? throw new ArgumentNullException(nameof(redemptionInfo));

        public BroadcastInfo(CexMovementInfo cexMovementInfo) 
            => CexMovementInfo = cexMovementInfo ?? throw new ArgumentNullException(nameof(cexMovementInfo));

        public BroadcastInfo(CfeVersionsInfo cfeVersionInfo) 
            => CfeVersionInfo = cfeVersionInfo ?? throw new ArgumentNullException(nameof(cfeVersionInfo));

        public BroadcastInfo(SwapLimitsInfo swapLimitsInfo) 
            => SwapLimitsInfo = swapLimitsInfo ?? throw new ArgumentNullException(nameof(swapLimitsInfo));

        public BroadcastInfo(PastVolumeInfo pastVolumeInfo) 
            => PastVolumeInfo = pastVolumeInfo ?? throw new ArgumentNullException(nameof(pastVolumeInfo));

        public BroadcastInfo(StakedFlipInfo stakedFlipInfo)
            => StakedFlipInfo = stakedFlipInfo ?? throw new ArgumentNullException(nameof(stakedFlipInfo));

        public BroadcastInfo(BrokerOverviewInfo brokerOverviewInfo)
            => BrokerOverviewInfo = brokerOverviewInfo ?? throw new ArgumentNullException(nameof(brokerOverviewInfo));

        public BroadcastInfo(BigStakedFlipInfo bigStakedFlipInfo)
            => BigStakedFlipInfo = bigStakedFlipInfo ?? throw new ArgumentNullException(nameof(bigStakedFlipInfo));
    }
}