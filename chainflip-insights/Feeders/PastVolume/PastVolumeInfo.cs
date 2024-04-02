namespace ChainflipInsights.Feeders.PastVolume
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChainflipInsights.Infrastructure;

    public class PastVolumeInfo
    {
        public string Date { get; }
        
        public double NetworkFees { get; }
        
        public string NetworkFeesFormatted  => NetworkFees.ToReadableMetric();

        public Dictionary<Tuple<string, string>, PastVolumePairInfo> VolumePairs { get; }
        
        public PastVolumeInfo(
            string date,
            IEnumerable<PastVolumePairInfo> volumePairs, 
            double networkFees)
        {
            Date = date;
            NetworkFees = networkFees;
            VolumePairs = volumePairs.ToDictionary(
                x => x.Pair,
                x => x);
        }
    }
    
    public class PastVolumePairInfo
    {
        public Tuple<string, string> Pair { get; }
        
        public double Value { get; }

        public double Fees { get; }
        
        public PastVolumePairInfo(
            IReadOnlyList<string> assets, 
            PastVolumeInfoResponseSum sum)
        {
            Pair = new Tuple<string, string>(assets[0], assets[1]);
            Value = sum.Value;
            Fees = sum.Fees;
        }
    }
}