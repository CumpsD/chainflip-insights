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
                var inputDecimals = Constants.AssetDecimals[SourceAsset.ToLowerInvariant()];
                var inputString = $"0.00{new string('#', inputDecimals - 2)}";
                var swapInput = DepositAmount / Math.Pow(10, inputDecimals);
                return Math.Round(swapInput, 8).ToString(inputString);
            }
        }
        
        public double DepositValueUsd { get; }
        
        public string DepositValueUsdFormatted => DepositValueUsd.ToString(Constants.DollarString);
        
        public string SourceAsset { get; }
        
        public double EgressAmount { get; }

        public string EgressAmountFormatted
        {
            get
            {
                var outputDecimals = Constants.AssetDecimals[DestinationAsset.ToLowerInvariant()];
                var outputString = $"0.00{new string('#', outputDecimals - 2)}";
                var swapOutput = EgressAmount / Math.Pow(10, outputDecimals);
                return Math.Round(swapOutput, 8).ToString(outputString);
            }
        }
        
        public double EgressValueUsd { get; }

        public string EgressValueUsdFormatted => EgressValueUsd.ToString(Constants.DollarString);

        public string DestinationAsset { get; }
        
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