namespace ChainflipInsights.Feeders.BrokerOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Infrastructure;

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
        
        public double Fees { get; }
        
        public string FeesFormatted => Fees.ToReadableMetric();

        public double Volume { get; }
        
        public string VolumeFormatted => Volume.ToReadableMetric();

        
        public BrokerInfo(
            IGrouping<string, SwapInfo> broker)
        {
            Ss58 = broker.Key;
            Fees = broker.Sum(x => x.BrokerFeeUsd ?? 0);
            Volume = broker.Sum(x => x.DepositValueUsd);
        }
    }
}