namespace ChainflipInsights.Feeders.Burn
{
    using System;
    using ChainflipInsights.Infrastructure;

    public class BurnInfo
    {
        private readonly PriceProvider _priceProvider;
        
        public ulong LastSupplyUpdateBlock { get; }
        
        public DateTimeOffset LastBurnTime { get; }
        
        public double? FlipBurned { get; }
        
        public string? FlipBurnedFormatted
        {
            get
            {
                if (FlipBurned == null)
                    return null;
                
                var flip = Constants.SupportedAssets[Constants.FLIP];
                var amount = FlipBurned.Value / Math.Pow(10, flip.Decimals);
                return Math.Round(amount, 2).ToString(flip.FormatString);
            }
        }
        
        public string? FlipBurnedFormattedUsd
        {
            get
            {
                if (FlipBurned == null)
                    return null;
             
                var flipPrice = _priceProvider
                    .GetFlipPriceInUsd()
                    .GetAwaiter()
                    .GetResult();
                
                var flip = Constants.SupportedAssets[Constants.FLIP];
                var amount = FlipBurned.Value / Math.Pow(10, flip.Decimals);
                
                return flipPrice == null 
                    ? null
                    : Math.Round(flipPrice.Value * amount, 2).ToString(Constants.DollarString);
            }
        }
        
        public BurnInfo(
            PriceProvider priceProvider,
            ulong lastSupplyUpdateBlock,
            DateTimeOffset lastBurnTime,
            double flipBurned)
        {
            _priceProvider = priceProvider;
            
            LastSupplyUpdateBlock = lastSupplyUpdateBlock;
            LastBurnTime = lastBurnTime;
            FlipBurned = flipBurned;
        }
    }
}