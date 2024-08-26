namespace ChainflipInsights.Feeders.DailyLpOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure;

    public class DailyLpOverviewInfo
    {
        public DateTimeOffset Date { get; }

        public Dictionary<string, LpOverviewInfo> LpVolume { get; set; }

        public DailyLpOverviewInfo(
            DateTimeOffset date,
            LiquidityProvider[] liquidityProviders,
            DailyLpOverviewResponse response)
        {
            Date = date;

            var lpAccounts = response
                .Data.Data.Data
                .Where(x =>
                    x.LimitOrders.LimitOrder.Length > 0 ||
                    x.RangeOrders.RangeOrder.Length > 0)
                .Select(x => new LpInfo(liquidityProviders, x))
                .Where(x => x.VolumeFilled > 0)
                .OrderByDescending(x => x.VolumeFilled)
                .ToList();

            var totalVolume = lpAccounts.Sum(x => x.VolumeFilled);

            LpVolume =
                lpAccounts
                    .GroupBy(x => x.Name)
                    .ToDictionary(
                        x => x.Key,
                        x => new
                        {
                            Twitter = x.First().Twitter,
                            VolumeFilled = x.Sum(y => y.VolumeFilled)
                        })
                    .OrderByDescending(x => x.Value.VolumeFilled)
                    .ToDictionary(
                        x => x.Key,
                        x => new LpOverviewInfo(
                            x.Key,
                            x.Value.Twitter,
                            x.Value.VolumeFilled,
                            totalVolume));
        }
    }

    public class LpOverviewInfo
    {
        public string Name { get; }
        public string Twitter { get; }
        
        public string VolumeFilled { get; }
        
        public string VolumePercentage { get; }

        public LpOverviewInfo(string name,
            string twitter,
            decimal volumeFilled,
            decimal totalVolume)
        {
            Name = name;
            Twitter = twitter;
            VolumeFilled = volumeFilled.ToString(Constants.DollarString);
            VolumePercentage = $"{Math.Round(100 / totalVolume * volumeFilled, 2).ToString(Constants.DollarString)}%";
        }
    }

    public class LpInfo
    {
        public string Ss58 { get; set; }

        public string Name { get; set; }
        
        public string Twitter { get; set; }
        
        public decimal VolumeFilled { get; set; }

        public LpInfo(
            LiquidityProvider[] liquidityProviders, 
            DailyLpOverviewNode lpNode)
        {
            Ss58 = lpNode.IdSs58;

            var name = liquidityProviders.SingleOrDefault(x => x.Address == lpNode.IdSs58);

            var ss58 = lpNode.IdSs58.FormatSs58();
            
            Name = name == null 
                ? ss58
                : name.Name;

            Twitter = name == null
                ? ss58
                : string.IsNullOrWhiteSpace(name.Twitter)
                    ? ss58
                    : name.Twitter;
            
            VolumeFilled = 
                lpNode.LimitOrders.LimitOrder.Sum(x => x.Sum.FilledAmountUsd)+ 
                lpNode.RangeOrders.RangeOrder.Sum(x => x.Sum.BaseFilledAmountUsd + x.Sum.QuoteFilledAmountUsd);
        }
    }
}