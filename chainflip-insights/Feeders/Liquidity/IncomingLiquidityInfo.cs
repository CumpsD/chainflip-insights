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
                var swapInput = DepositAmount / Math.Pow(10, SourceAssetInfo.Decimals);
                return Math.Round(swapInput, 8).ToString(SourceAssetInfo.FormatString);
            }
        }
        
        public double DepositValueUsd { get; }
        
        public string DepositValueUsdFormatted => DepositValueUsd.ToString(Constants.DollarString);
        
        public string SourceAsset { get; }
        
        public AssetInfo SourceAssetInfo { get; }
        
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

            SourceAssetInfo = Constants.SupportedAssets[SourceAsset.ToLowerInvariant()];
        }
    }
}