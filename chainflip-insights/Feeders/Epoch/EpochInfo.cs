namespace ChainflipInsights.Feeders.Epoch
{
    using System;
    using System.Linq;
    using Humanizer;

    public class EpochInfo
    {
        private const int FlipDecimals = 18;
        
        public double Id { get; }
        
        public double MinimumBond { get; }
        
        public string MinimumBondFormatted => Math.Round(MinimumBond / Math.Pow(10, FlipDecimals), 3).ToMetric();

        public double TotalBond { get; }
        
        public string TotalBondFormatted 
            => Math.Round(TotalBond / Math.Pow(10, FlipDecimals), 3).ToMetric();
        
        public DateTimeOffset EpochStart { get; }
        
        public double MaxBid { get; }
        
        public string MaxBidFormatted 
            => Math.Round(MaxBid / Math.Pow(10, FlipDecimals), 3).ToMetric();
        
        public double TotalRewards { get; }
        
        public string TotalRewardsFormatted 
            => Math.Round(TotalRewards / Math.Pow(10, FlipDecimals), 3).ToString("0.00");
        
        public EpochInfo? PreviousEpoch { get; }

        public EpochInfo(
            EpochInfoResponseNode epoch,
            EpochInfo? previousEpoch)
        {
            Id = epoch.Id;
            MinimumBond = epoch.Bond;
            TotalBond = epoch.TotalBonded;
            EpochStart = epoch.StartBlock.Timestamp;
            PreviousEpoch = previousEpoch;

            var authorities = epoch
                .AuthorityMemberships
                .Data
                .ToList();

            MaxBid = authorities.Max(x => x.Data.Bid);
            TotalRewards = authorities.Sum(x => x.Data.Reward);
        }
    }
}