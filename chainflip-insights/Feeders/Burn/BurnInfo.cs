namespace ChainflipInsights.Feeders.Burn
{
    using System;

    public class BurnInfo
    {
        public uint LastSupplyUpdateBlock { get; }
        
        public string LastSupplyUpdateBlockHash { get; }
        
        public double FlipToBurn { get; }
        
        public string FlipToBurnFormatted
        {
            get
            {
                var flip = Constants.SupportedAssets[Constants.FLIP];
                var amount = FlipToBurn / Math.Pow(10, flip.Decimals);
                return Math.Round(amount, 8).ToString(flip.FormatString);
            }
        }

        public double? FlipBurned { get; }
        
        public string? FlipBurnedFormatted
        {
            get
            {
                if (FlipBurned == null)
                    return null;
                
                var flip = Constants.SupportedAssets[Constants.FLIP];
                var amount = FlipBurned.Value / Math.Pow(10, flip.Decimals);
                return Math.Round(amount, 8).ToString(flip.FormatString);
            }
        }
        
        public bool BurnSkipped { get; }

        public BurnInfo(uint lastSupplyUpdateBlock,
            string lastSupplyUpdateBlockHash,
            double flipToBurn,
            double? flipBurned, 
            bool burnSkipped)
        {
            LastSupplyUpdateBlock = lastSupplyUpdateBlock;
            LastSupplyUpdateBlockHash = lastSupplyUpdateBlockHash;
            FlipToBurn = flipToBurn;
            FlipBurned = flipBurned;
            BurnSkipped = burnSkipped;
        }
    }
}