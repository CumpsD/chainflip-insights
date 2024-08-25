namespace ChainflipInsights.Feeders.WeeklyLpOverview
{
    using System.Text.Json.Serialization;

    public class WeeklyLpOverviewResponse
    {
        [JsonPropertyName("data")] 
        public WeeklyLpOverviewResponseData Data { get; set; }
    }
    
    public class WeeklyLpOverviewResponseData
    {
        [JsonPropertyName("allAccounts")] 
        public WeeklyLpOverviewResponseAllAccounts Data { get; set; }
    }

    public class WeeklyLpOverviewResponseAllAccounts
    {
        [JsonPropertyName("nodes")] 
        public WeeklyLpOverviewNode[] Data { get; set; }
    }

    public class WeeklyLpOverviewNode
    {
        [JsonPropertyName("idSs58")] 
        public string IdSs58 { get; set; }
        
        [JsonPropertyName("limitOrders")] 
        public WeeklyLpOverviewLimitOrders LimitOrders { get; set; }
        
        [JsonPropertyName("rangeOrders")] 
        public WeeklyLpOverviewRangeOrders RangeOrders { get; set; }
    }

    public class WeeklyLpOverviewLimitOrders
    {
        [JsonPropertyName("groupedAggregates")] 
        public WeeklyLpOverviewLimitOrder[] LimitOrder { get; set; }
    }

    public class WeeklyLpOverviewRangeOrders
    {
        [JsonPropertyName("groupedAggregates")] 
        public WeeklyLpOverviewRangeOrder[] RangeOrder { get; set; }
    }

    public class WeeklyLpOverviewLimitOrder
    {
        [JsonPropertyName("sum")] 
        public WeeklyLpOverviewLimitOrderSum Sum { get; set; }
        
        [JsonPropertyName("keys")] 
        public string[] Keys { get; set; }
    }

    public class WeeklyLpOverviewRangeOrder
    {
        [JsonPropertyName("sum")] 
        public WeeklyLpOverviewRangeOrderSum Sum { get; set; }
        
        [JsonPropertyName("keys")] 
        public string[] Keys { get; set; }
    }

    public class WeeklyLpOverviewLimitOrderSum
    {
        [JsonPropertyName("filledAmountValueUsd")] 
        public decimal FilledAmountUsd { get; set; }
    }
    
    public class WeeklyLpOverviewRangeOrderSum
    {
        [JsonPropertyName("quoteFilledAmountValueUsd")] 
        public decimal QuoteFilledAmountUsd { get; set; }
        
        [JsonPropertyName("baseFilledAmountValueUsd")] 
        public decimal BaseFilledAmountUsd { get; set; }
    }
}