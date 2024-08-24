namespace ChainflipInsights.Feeders.WeeklySwapOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChainflipInsights.EntityFramework;

    public class WeeklySwapOverviewInfo
    {
        public DateTimeOffset StartDate { get; }
        
        public DateTimeOffset EndDate { get; }
        
        public List<SwapOverviewAsset> Swaps { get; }
        
        public WeeklySwapOverviewInfo(
            DateTimeOffset startDate, 
            DateTimeOffset endDate, 
            IEnumerable<SwapOverviewAsset> swaps)
        {
            StartDate = startDate;
            EndDate = endDate;
            Swaps = swaps
                .OrderByDescending(x => x.Swap.EgressValueUsd)
                .ToList();
        }
    }

    public class SwapOverviewAsset
    {
        public string Asset { get; set; }

        public SwapOverviewInfo Swap { get; set; }

        public SwapOverviewAsset(
            string asset, 
            SwapOverviewInfo swap)
        {
            Asset = asset;
            Swap = swap;
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