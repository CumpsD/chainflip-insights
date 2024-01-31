namespace ChainflipInsights.Feeders.Liquidity
{
    using System;

    public class IncomingLiquidityInfo
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
        
        public ulong BlockId { get; }

        public string Network { get; }
        
        public string ChannelId { get; }
        
        public IncomingLiquidityInfo(IncomingLiquidityResponseNode liquidity)
        {
            Id = liquidity.Id;
            DepositAmount = liquidity.DepositAmount;
            DepositValueUsd = liquidity.DepositValueUsd;
            SourceAsset = liquidity.Channel.Asset;

            BlockId = liquidity.Channel.IssuedBlockId;
            Network = liquidity.Channel.Chain;
            ChannelId = liquidity.Channel.ChannelId;

        }
    }
}