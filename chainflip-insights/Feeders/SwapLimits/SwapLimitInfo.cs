namespace ChainflipInsights.Feeders.SwapLimits
{
    using System;

    public class SwapLimitsInfo
    {
        public SwapLimitInfo[] SwapLimits { get; }

        public SwapLimitsInfo(SwapLimitInfo[] swapLimits) 
            => SwapLimits = swapLimits;
    }
    
    public class SwapLimitInfo
    {
        public string SwapLimit { get; }
        
        public AssetInfo Asset { get; }

        public SwapLimitInfo(SwapLimitResponse swapLimit, AssetInfo asset)
        {
            SwapLimit = Math
                .Round(swapLimit.Result / Math.Pow(10, asset.Decimals), 8)
                .ToString(asset.FormatString);
            
            Asset = asset;
        }
    }
}