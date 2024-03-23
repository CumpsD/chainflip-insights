namespace ChainflipInsights.Feeders.BrokerOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Humanizer;

    public class BrokerOverviewInfo
    {
        public DateTimeOffset Date { get; }
        
        public List<BrokerInfo> Brokers { get; }
        
        public BrokerOverviewInfo(
            DateTimeOffset date, 
            IEnumerable<BrokerInfo> brokers)
        {
            Date = date;
            Brokers = brokers
                .OrderByDescending(x => x.Volume)
                .Take(5)
                .ToList();
        }
    }
    
    public class BrokerInfo
    {
        public string Ss58 { get; }
        
        public double Swaps { get; }

        public double Fees { get; }
        
        public string FeesFormatted => Fees.ToMetric(decimals: 2);

        public double Volume { get; }
        
        public string VolumeFormatted => Volume.ToMetric(decimals: 2);

        
        public BrokerInfo(
            BrokerOverviewInfoBroker broker)
        {
            Ss58 = broker.Ss58;
            Swaps = broker.Swaps ?? 0;
            Fees = broker.Fees ?? 0;
            Volume = broker.Volume ?? 0;
        }
    }
}