namespace ChainflipInsights.Feeders.Swap
{
    using System;
    using System.Collections.Generic;

    public class SwapInfo
    {
        private const string SUB1K = "🦐";
        private const string SUB2_5K = "🐟";
        private const string SUB5K = "🦀";
        private const string SUB10K = "🦈";
        private const string WHALE = "🐳";
        
        private const string dollarString = "0.00";

        private readonly Dictionary<string, int> _assetDecimals = new()
        {
            { "btc", 8 },
            { "dot", 10 },
            { "eth", 18 },
            { "flip", 18 },
            { "usdc", 6 },
        };
        
        public double Id { get; }

        public double DepositAmount { get; }

        public string DepositAmountFormatted
        {
            get
            {
                var inputDecimals = _assetDecimals[SourceAsset.ToLowerInvariant()];
                var inputString = $"0.00{new string('#', inputDecimals - 2)}";
                var swapInput = DepositAmount / Math.Pow(10, inputDecimals);
                return Math.Round(swapInput, 8).ToString(inputString);
            }
        }
        
        public double DepositValueUsd { get; }
        
        public string DepositValueUsdFormatted => DepositValueUsd.ToString(dollarString);
        
        public string SourceAsset { get; }
        
        public double EgressAmount { get; }

        public string EgressAmountFormatted
        {
            get
            {
                var outputDecimals = _assetDecimals[DestinationAsset.ToLowerInvariant()];
                var outputString = $"0.00{new string('#', outputDecimals - 2)}";
                var swapOutput = EgressAmount / Math.Pow(10, outputDecimals);
                return Math.Round(swapOutput, 8).ToString(outputString);
            }
        }
        
        public double EgressValueUsd { get; }

        public string EgressValueUsdFormatted => EgressValueUsd.ToString(dollarString);

        public string DestinationAsset { get; }
        
        public string SwapScheduledBlockTimestamp { get; }

        public string Emoji =>
            DepositValueUsd switch
            {
                > 10000 => WHALE,
                > 5000 => SUB10K,
                > 2500 => SUB5K,
                > 1000 => SUB2_5K,
                _ => SUB1K
            };

        public SwapInfo(
            SwapsResponseNode swap)
        {
            Id = swap.Id;
            DepositAmount = swap.DepositAmount;
            DepositValueUsd = swap.DepositValueUsd;
            SourceAsset = swap.SourceAsset;
            
            EgressAmount = swap.EgressAmount;
            EgressValueUsd = swap.EgressValueUsd;
            DestinationAsset = swap.DestinationAsset;

            SwapScheduledBlockTimestamp = swap.SwapScheduledBlockTimestamp;
        }
    }
}