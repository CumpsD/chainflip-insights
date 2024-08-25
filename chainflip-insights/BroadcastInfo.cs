namespace ChainflipInsights
{
    using System;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.Burn;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.CfeVersion;
    using ChainflipInsights.Feeders.DailyLpOverview;
    using ChainflipInsights.Feeders.DailySwapOverview;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.StakedFlip;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;
    using ChainflipInsights.Feeders.WeeklySwapOverview;

    public class BroadcastInfo
    {
        public SwapInfo? SwapInfo { get; }
        public IncomingLiquidityInfo? IncomingLiquidityInfo { get; }
        public OutgoingLiquidityInfo? OutgoingLiquidityInfo { get; }
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
        public BurnInfo? BurnInfo { get; }
        public DailySwapOverviewInfo? DailySwapOverviewInfo { get; }
        public WeeklySwapOverviewInfo? WeeklySwapOverviewInfo { get; }
        public DailyLpOverviewInfo? DailyLpOverviewInfo { get; }

        public BroadcastInfo(SwapInfo swapInfo) 
            => SwapInfo = swapInfo ?? throw new ArgumentNullException(nameof(swapInfo));

        public BroadcastInfo(IncomingLiquidityInfo incomingLiquidityInfo) 
            => IncomingLiquidityInfo = incomingLiquidityInfo ?? throw new ArgumentNullException(nameof(incomingLiquidityInfo));

        public BroadcastInfo(OutgoingLiquidityInfo outgoingLiquidityInfo) 
            => OutgoingLiquidityInfo = outgoingLiquidityInfo ?? throw new ArgumentNullException(nameof(outgoingLiquidityInfo));

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
        
        public BroadcastInfo(BurnInfo burnInfo)
            => BurnInfo = burnInfo ?? throw new ArgumentNullException(nameof(burnInfo));
        
        public BroadcastInfo(DailySwapOverviewInfo dailySwapOverviewInfo)
            => DailySwapOverviewInfo = dailySwapOverviewInfo ?? throw new ArgumentNullException(nameof(dailySwapOverviewInfo));
        
        public BroadcastInfo(WeeklySwapOverviewInfo weeklySwapOverviewInfo)
            => WeeklySwapOverviewInfo = weeklySwapOverviewInfo ?? throw new ArgumentNullException(nameof(weeklySwapOverviewInfo));
        
        public BroadcastInfo(DailyLpOverviewInfo dailyLpOverviewInfo)
            => DailyLpOverviewInfo = dailyLpOverviewInfo ?? throw new ArgumentNullException(nameof(dailyLpOverviewInfo));
    }
}