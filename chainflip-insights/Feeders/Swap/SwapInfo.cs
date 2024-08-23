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
        
        public double DeltaUsd => DepositValueUsd - EgressValueUsd;

        public double DeltaUsdPercentage => 100 / DepositValueUsd * DeltaUsd;
        
        public DateTimeOffset SwapScheduledBlockTimestamp { get; }
        
        public string? Broker { get; }
        
        public bool IsBoosted { get; set; }

        public double? BoostFeeBps { get; set; }
        
        public double? BoostFeeUsd { get; set; }

        public string? BoostFeeUsdFormatted => BoostFeeUsd?.ToString(Constants.DollarString);

        public double? BrokerFeeUsd { get; set; }

        public string? BrokerFeeUsdFormatted => BrokerFeeUsd?.ToString(Constants.DollarString);

        public double? BrokerFeePercentage => 100 / DepositValueUsd * BrokerFeeUsd;
        
        public string? BrokerFeePercentageFormatted 
            => BrokerFeePercentage == null
                ? null
                : $"{Math.Round(BrokerFeePercentage.Value, 2).ToString(Constants.DollarString)}%";

        public double ProtocolDeltaUsd => DepositValueUsd - EgressValueUsd - (BrokerFeeUsd ?? 0);

        public string ProtocolDeltaUsdFormatted => (-ProtocolDeltaUsd).ToString(Constants.DollarString);

        public double ProtocolDeltaUsdPercentage => 100 / DepositValueUsd * ProtocolDeltaUsd;
        
        public string ProtocolDeltaUsdPercentageFormatted 
            => $"{Math.Round(-ProtocolDeltaUsdPercentage, 2).ToString(Constants.DollarString)}%";
        
        public double? AllFeesUsd { get; set; }
        
        public double? AllUserFeesUsd { get; set; }
        
        public double? LiquidityFeesUsd { get; set; }
        
        public double? BurnUsd { get; set; }
        
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

            var fees = swap
                .SwapFees
                .Data;
            
            IsBoosted = swap.EffectiveBoostFeeBps != null;
            BoostFeeBps = swap.EffectiveBoostFeeBps;
            BoostFeeUsd = fees.FirstOrDefault(x => x.Data.FeeType == "BOOST")?.Data.FeeValueUsd;
            BrokerFeeUsd = fees.FirstOrDefault(x => x.Data.FeeType == "BROKER")?.Data.FeeValueUsd;

            BurnUsd = fees.FirstOrDefault(x => x.Data.FeeType == "NETWORK")?.Data.FeeValueUsd;
            
            LiquidityFeesUsd = swap.SwapInputValueUsd - swap.SwapOutputValueUsd;
            
            AllFeesUsd = fees.Sum(x => x.Data.FeeValueUsd ?? 0) + LiquidityFeesUsd;
            AllUserFeesUsd = fees
                .Where(x => 
                    x.Data.FeeType != "INGRESS" &&
                    x.Data.FeeType != "EGRESS")
                .Sum(x => x.Data.FeeValueUsd ?? 0) + LiquidityFeesUsd;
            
            Broker = GetBroker(swap);
        }

        private static string? GetBroker(SwapsResponseNode swap)
        {
            var mainBroker = swap
                .SwapChannel?
                .Broker
                .Account
                .Ss58;
            
            var beneficiaries = swap
                .SwapChannel?
                .Beneficiaries?
                .Data;

            if (beneficiaries == null)
                return mainBroker;

            var affiliate = beneficiaries
                .FirstOrDefault(x => x.Type == "AFFILIATE");

            return affiliate == null 
                ? mainBroker 
                : affiliate.Broker.Account.Ss58;
        }
    }
}