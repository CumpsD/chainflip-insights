namespace ChainflipInsights.Feeders.StakedFlip
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Humanizer;

    public enum NetMovement
    {
        MoreStaked,
        MoreUnstaked
    }
    
    public class StakedFlipInfo
    {
        public DateTimeOffset Date { get; }
        
        public double TotalMovement { get; }
        
        public string TotalMovementFormatted => TotalMovement.ToMetric(decimals: 2);
        
        public double Staked { get; }
        
        public string StakedFormatted  => Staked.ToMetric(decimals: 2);
        
        public double Unstaked { get; }
        
        public string UnstakedFormatted  => Unstaked.ToMetric(decimals: 2);

        public NetMovement NetMovement { get; }
        
        public StakedFlipInfo(
            DateTimeOffset date,
            IEnumerable<StakedFlipData> staked,
            IEnumerable<StakedFlipData> unstaked)
        {
            Date = date;

            var flip = Constants.SupportedAssets[Constants.FLIP];

            Staked = Math.Round(
                staked.Sum(x => x.Amount / Math.Pow(10, flip.Decimals)), 
                8);
            
            Unstaked = Math.Round(
                unstaked.Sum(x => x.Amount / Math.Pow(10, flip.Decimals)), 
                8);

            TotalMovement = Math.Abs(Staked - Unstaked);
            
            NetMovement = Staked - Unstaked > 0 
                ? NetMovement.MoreStaked 
                : NetMovement.MoreUnstaked;
        }
    }
}