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

        public Dictionary<string, string> LpVolume { get; set; }

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

            LpVolume =
                lpAccounts
                    .GroupBy(x => x.Name)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Sum(y => y.VolumeFilled).ToString(Constants.DollarString));
        }
    }

    public class LpInfo
    {
        public string Ss58 { get; set; }

        public string Name { get; set; }
        
        public decimal VolumeFilled { get; set; }

        public LpInfo(
            LiquidityProvider[] liquidityProviders, 
            DailyLpOverviewNode lpNode)
        {
            Ss58 = lpNode.IdSs58;

            var name = liquidityProviders.SingleOrDefault(x => x.Address == lpNode.IdSs58);

            Name = name == null 
                ? lpNode.IdSs58.FormatSs58()
                : name.Name;
            
            VolumeFilled = 
                lpNode.LimitOrders.LimitOrder.Sum(x => x.Sum.FilledAmountUsd)+ 
                lpNode.RangeOrders.RangeOrder.Sum(x => x.Sum.BaseFilledAmountUsd + x.Sum.QuoteFilledAmountUsd);
        }
    }
}