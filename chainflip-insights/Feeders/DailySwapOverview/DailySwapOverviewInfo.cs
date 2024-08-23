namespace ChainflipInsights.Feeders.DailySwapOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChainflipInsights.EntityFramework;

    public class DailySwapOverviewInfo
    {
        public DateTimeOffset Date { get; }
        
        public List<SwapOverviewInfo> Swaps { get; }
        
        public DailySwapOverviewInfo(
            DateTimeOffset date, 
            IEnumerable<SwapOverviewInfo> swaps)
        {
            Date = date;
            Swaps = swaps
                .OrderByDescending(x => x.EgressValueUsd)
                .Take(5)
                .ToList();
        }
    }
    
    public class SwapOverviewInfo
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
        
        public string? Broker { get; }

        public SwapOverviewInfo(
            SwapInfo swap)
        {
            Id = swap.SwapId;
            
            DepositAmount = swap.DepositAmount;
            DepositValueUsd = swap.DepositValueUsd;
            SourceAsset = swap.SourceAsset;

            EgressAmount = swap.EgressAmount ;
            EgressValueUsd = swap.EgressValueUsd;
            DestinationAsset = swap.DestinationAsset;
            
            Broker = swap.Broker;

            SourceAssetInfo = Constants.SupportedAssets.Single(x => x.Value.Ticker == SourceAsset).Value;
            DestinationAssetInfo = Constants.SupportedAssets.Single(x => x.Value.Ticker == DestinationAsset).Value;
        }
    }
}