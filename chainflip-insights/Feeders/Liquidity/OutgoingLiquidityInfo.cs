namespace ChainflipInsights.Feeders.Liquidity
{
    using System;

    public class OutgoingLiquidityInfo
    {
        public double Id { get; }
        
        public double WithdrawalAmount { get; }
        
        public string WithdrawalAmountFormatted
        {
            get
            {
                var swapInput = WithdrawalAmount / Math.Pow(10, SourceAssetInfo.Decimals);
                return Math.Round(swapInput, 8).ToString(SourceAssetInfo.FormatString);
            }
        }
        
        public double WithdrawalValueUsd { get; }
        
        public string WithdrawalValueUsdFormatted => WithdrawalValueUsd.ToString(Constants.DollarString);
        
        public string SourceAsset { get; }
        
        public AssetInfo SourceAssetInfo { get; }
        
        public string Network { get; }
        
        public DateTimeOffset Timestamp { get; }
        
        public string BlockId { get; }
        
        public OutgoingLiquidityInfo(OutgoingLiquidityResponseNode liquidity)
        {
            Id = liquidity.Id;
            WithdrawalAmount = liquidity.WithdrawalAmount;
            WithdrawalValueUsd = liquidity.WithdrawalValueUsd;
            SourceAsset = liquidity.Asset.ToUpperInvariant();
            Network = liquidity.Chain;
            Timestamp = liquidity.Block.Timestamp;
            BlockId = liquidity.Block.BlockId;
            SourceAssetInfo = Constants.SupportedAssets[SourceAsset.ToLowerInvariant()];
        }
    }
}