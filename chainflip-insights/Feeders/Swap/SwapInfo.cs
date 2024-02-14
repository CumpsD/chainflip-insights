namespace ChainflipInsights.Feeders.Swap
{
    using System;

    public class SwapInfo
    {
        public double Id { get; }

        public double DepositAmount { get; }

        public string DepositAmountFormatted
        {
            get
            {
                var swapInput = DepositAmount / Math.Pow(10, SourceAssetInfo.Decimals);
                return Math.Round(swapInput, 8).ToString(SourceAssetInfo.FormatString);
            }
        }
        
        public double DepositValueUsd { get; }
        
        public string DepositValueUsdFormatted => DepositValueUsd.ToString(Constants.DollarString);
        
        public string SourceAsset { get; }
        
        public AssetInfo SourceAssetInfo { get; }
        
        public double EgressAmount { get; }

        public string EgressAmountFormatted
        {
            get
            {
                var swapOutput = EgressAmount / Math.Pow(10, DestinationAssetInfo.Decimals);
                return Math.Round(swapOutput, 8).ToString(DestinationAssetInfo.FormatString);
            }
        }
        
        public double EgressValueUsd { get; }

        public string EgressValueUsdFormatted => EgressValueUsd.ToString(Constants.DollarString);

        public string DestinationAsset { get; }
        
        public AssetInfo DestinationAssetInfo { get; }
        
        public string SwapScheduledBlockTimestamp { get; }

        public string Emoji =>
            DepositValueUsd switch
            {
                > 10000 => Constants.Whale,
                > 5000 => Constants.Sub10K,
                > 2500 => Constants.Sub5K,
                > 1000 => Constants.Sub2_5K,
                _ => Constants.Sub1K
            };

        public SwapInfo(
            SwapsResponseNode swap)
        {
            Id = swap.NativeId;
            DepositAmount = swap.DepositAmount;
            DepositValueUsd = swap.DepositValueUsd;
            SourceAsset = swap.SourceAsset.ToUpperInvariant();
            
            EgressAmount = swap.EgressAmount;
            EgressValueUsd = swap.EgressValueUsd;
            DestinationAsset = swap.DestinationAsset.ToUpperInvariant();

            SwapScheduledBlockTimestamp = swap.SwapScheduledBlockTimestamp;

            SourceAssetInfo = Constants.SupportedAssets[SourceAsset.ToLowerInvariant()];
            DestinationAssetInfo = Constants.SupportedAssets[DestinationAsset.ToLowerInvariant()];
        }
    }
}