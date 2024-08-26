namespace ChainflipInsights.Feeders.WeeklyLpOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure;

    public class WeeklyLpOverviewInfo
    {
        public DateTimeOffset StartDate { get; }
        
        public DateTimeOffset EndDate { get; }
        public Dictionary<string, Tuple<string, string>> LpVolume { get; set; }

        public WeeklyLpOverviewInfo(
            DateTimeOffset startDate, 
            DateTimeOffset endDate, 
            LiquidityProvider[] liquidityProviders,
            WeeklyLpOverviewResponse response)
        {
            StartDate = startDate;
            EndDate = endDate;

            var lpAccounts = response
                .Data.Data.Data
                .Where(x =>
                    x.LimitOrders.LimitOrder.Length > 0 ||
                    x.RangeOrders.RangeOrder.Length > 0)
                .Select(x => new LpInfo(liquidityProviders, x))
                .Where(x => x.VolumeFilled > 0)
                .OrderByDescending(x => x.VolumeFilled)
                .ToList();

            LpVolume =
                lpAccounts
                    .GroupBy(x => x.Name)
                    .ToDictionary(
                        x => x.Key,
                        x => new Tuple<string, decimal>(
                            x.First().Twitter,
                            x.Sum(y => y.VolumeFilled)))
                    .OrderByDescending(x => x.Value.Item2)
                    .ToDictionary(
                        x => x.Key,
                        x => new Tuple<string, string>(
                            x.Value.Item1,
                            x.Value.Item2.ToString(Constants.DollarString)));
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
            WeeklyLpOverviewNode lpNode)
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