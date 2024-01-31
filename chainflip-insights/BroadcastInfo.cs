namespace ChainflipInsights
{
    using System;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.Swap;

    public class BroadcastInfo
    {
        public IncomingLiquidityInfo? IncomingLiquidityInfo { get; }
        public SwapInfo? SwapInfo { get; }

        public BroadcastInfo(SwapInfo swapInfo) 
            => SwapInfo = swapInfo ?? throw new ArgumentNullException(nameof(swapInfo));

        public BroadcastInfo(IncomingLiquidityInfo incomingLiquidityInfo) 
            => IncomingLiquidityInfo = incomingLiquidityInfo ?? throw new ArgumentNullException(nameof(incomingLiquidityInfo));
    }
}