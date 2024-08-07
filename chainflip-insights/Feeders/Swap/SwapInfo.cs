namespace ChainflipInsights.Feeders.Swap
{
    using System;
    using System.Linq;

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
        
        public double DeltaUsd => EgressValueUsd - DepositValueUsd;

        public string DeltaUsdFormatted => DeltaUsd.ToString(Constants.DollarString);

        public double DeltaUsdPercentage => 100 / DepositValueUsd * DeltaUsd;
        
        public string DeltaUsdPercentageFormatted 
            => $"{Math.Round(DeltaUsdPercentage, 2).ToString(Constants.DollarString)}%";
        
        public DateTimeOffset SwapScheduledBlockTimestamp { get; }
        
        public string? Broker { get; }
        
        public bool IsBoosted { get; set; }

        public double? BoostFeeBps { get; set; }
        
        public double? BoostFeeUsd { get; set; }

        public string? BoostFeeUsdFormatted => BoostFeeUsd?.ToString(Constants.DollarString);

        public string Emoji =>
            DepositValueUsd switch
            {
                > 100_000 => Constants.Whale,
                >  50_000 => Constants.Shark,
                >  25_000 => Constants.Crab,
                >  10_000 => Constants.Fish,
                _ => Constants.Shrimp
            };

        public SwapInfo(
            SwapsResponseNode swap)
        {
            Id = swap.NativeId;
            DepositAmount = swap.DepositAmount;
            DepositValueUsd = swap.DepositValueUsd;
            
            // EgressAmount is null when the swap ate everything
            // It is however also null when it is still in progress
            // So we need to figure out why it is null and throw an error if it was not finished yet

            if (!swap.EgressAmount.HasValue && swap.DepositValueUsd > 1)
            {
                // There is no egress, and they deposited more than a dollar, probably still in progress
                throw new Exception("Swap not finished yet.");
            }
            
            EgressAmount = swap.EgressAmount ?? 0;
            EgressValueUsd = swap.EgressValueUsd ?? 0;

            SwapScheduledBlockTimestamp = DateTimeOffset.Parse(swap.SwapScheduledBlockTimestamp);

            SourceAssetInfo = Constants.SupportedAssets[swap.SourceAsset.ToLowerInvariant()];
            DestinationAssetInfo = Constants.SupportedAssets[swap.DestinationAsset.ToLowerInvariant()];

            SourceAsset = SourceAssetInfo.Ticker;
            DestinationAsset = DestinationAssetInfo.Ticker;

            IsBoosted = swap.EffectiveBoostFeeBps != null;
            BoostFeeBps = swap.EffectiveBoostFeeBps;
            BoostFeeUsd = swap.SwapFees.Data.FirstOrDefault(x => x.Data.FeeType == "BOOST")?.Data.FeeValueUsd;
            
            Broker = swap
                .SwapChannel?
                .Broker
                .Account
                .Ss58;
        }
    }
}