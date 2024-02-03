namespace ChainflipInsights
{
    using System;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.Swap;

    public class BroadcastInfo
    {
        public SwapInfo? SwapInfo { get; }
        public IncomingLiquidityInfo? IncomingLiquidityInfo { get; }
        public EpochInfo? EpochInfo { get; }
        public FundingInfo? FundingInfo { get; }

        public BroadcastInfo(SwapInfo swapInfo) 
            => SwapInfo = swapInfo ?? throw new ArgumentNullException(nameof(swapInfo));

        public BroadcastInfo(IncomingLiquidityInfo incomingLiquidityInfo) 
            => IncomingLiquidityInfo = incomingLiquidityInfo ?? throw new ArgumentNullException(nameof(incomingLiquidityInfo));

        public BroadcastInfo(EpochInfo epochInfo)
            => EpochInfo = epochInfo ?? throw new ArgumentNullException(nameof(epochInfo));

        public BroadcastInfo(FundingInfo fundingInfo)
            => FundingInfo = fundingInfo ?? throw new ArgumentNullException(nameof(fundingInfo));

    }
}