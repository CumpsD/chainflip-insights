namespace ChainflipInsights.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class RangeExtension
    {
        public static IEnumerable<DateTime> Range(
            this DateTime startDate, DateTime endDate) 
            => Enumerable
                .Range(0, (int)(endDate - startDate).TotalDays + 1)
                .Select(i => startDate.AddDays(i));
    }
}