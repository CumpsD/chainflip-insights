namespace ChainflipInsights.Feeders.Burn
{
    using System;
    using ChainflipInsights.Infrastructure;

    public class BurnInfo
    {
        private readonly PriceProvider _priceProvider;
        
        public uint LastSupplyUpdateBlock { get; }
        
        public string LastSupplyUpdateBlockHash { get; }
        
        public double FlipToBurn { get; }
        
        public string FlipToBurnFormatted
        {
            get
            {
                var flipPrice = _priceProvider
                    .GetFlipPriceInUsd()
                    .GetAwaiter()
                    .GetResult();
                
                var flip = Constants.SupportedAssets[Constants.FLIP];
                var amount = FlipToBurn / Math.Pow(10, flip.Decimals);
                
                if (flipPrice == null)
                    return Math.Round(amount, 2).ToString(flip.FormatString);

                var usdValue = Math.Round(flipPrice.Value * amount, 2);
                return $"{Math.Round(amount, 2).ToString(flip.FormatString)} (${usdValue})";
            }
        }

        public double? FlipBurned { get; }
        
        public string? FlipBurnedFormatted
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
                
                if (flipPrice == null)
                    return Math.Round(amount, 2).ToString(flip.FormatString);
                
                var usdValue = Math.Round(flipPrice.Value * amount, 2);
                return $"{Math.Round(amount, 2).ToString(flip.FormatString)} (${usdValue})";
            }
        }
        
        public bool BurnSkipped { get; }

        public BurnInfo(
            PriceProvider priceProvider,
            uint lastSupplyUpdateBlock,
            string lastSupplyUpdateBlockHash,
            double flipToBurn,
            double? flipBurned, 
            bool burnSkipped)
        {
            _priceProvider = priceProvider;
            
            LastSupplyUpdateBlock = lastSupplyUpdateBlock;
            LastSupplyUpdateBlockHash = lastSupplyUpdateBlockHash;
            FlipToBurn = flipToBurn;
            FlipBurned = flipBurned;
            BurnSkipped = burnSkipped;
        }
    }
}