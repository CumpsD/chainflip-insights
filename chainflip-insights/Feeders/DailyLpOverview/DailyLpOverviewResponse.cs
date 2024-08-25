namespace ChainflipInsights.Feeders.DailyLpOverview
{
    using System.Text.Json.Serialization;

    public class DailyLpOverviewResponse
    {
        [JsonPropertyName("data")] 
        public DailyLpOverviewResponseData Data { get; set; }
    }
    
    public class DailyLpOverviewResponseData
    {
        [JsonPropertyName("allAccounts")] 
        public DailyLpOverviewResponseAllAccounts Data { get; set; }
    }

    public class DailyLpOverviewResponseAllAccounts
    {
        [JsonPropertyName("nodes")] 
        public DailyLpOverviewNode[] Data { get; set; }
    }

    public class DailyLpOverviewNode
    {
        [JsonPropertyName("idSs58")] 
        public string IdSs58 { get; set; }
        
        [JsonPropertyName("limitOrders")] 
        public DailyLpOverviewLimitOrders LimitOrders { get; set; }
        
        [JsonPropertyName("rangeOrders")] 
        public DailyLpOverviewRangeOrders RangeOrders { get; set; }
    }

    public class DailyLpOverviewLimitOrders
    {
        [JsonPropertyName("groupedAggregates")] 
        public DailyLpOverviewLimitOrder[] LimitOrder { get; set; }
    }

    public class DailyLpOverviewRangeOrders
    {
        [JsonPropertyName("groupedAggregates")] 
        public DailyLpOverviewRangeOrder[] RangeOrder { get; set; }
    }

    public class DailyLpOverviewLimitOrder
    {
        [JsonPropertyName("sum")] 
        public DailyLpOverviewLimitOrderSum Sum { get; set; }
        
        [JsonPropertyName("keys")] 
        public string[] Keys { get; set; }
    }

    public class DailyLpOverviewRangeOrder
    {
        [JsonPropertyName("sum")] 
        public DailyLpOverviewRangeOrderSum Sum { get; set; }
        
        [JsonPropertyName("keys")] 
        public string[] Keys { get; set; }
    }

    public class DailyLpOverviewLimitOrderSum
    {
        [JsonPropertyName("filledAmountValueUsd")] 
        public decimal FilledAmountUsd { get; set; }
    }
    
    public class DailyLpOverviewRangeOrderSum
    {
        [JsonPropertyName("quoteFilledAmountValueUsd")] 
        public decimal QuoteFilledAmountUsd { get; set; }
        
        [JsonPropertyName("baseFilledAmountValueUsd")] 
        public decimal BaseFilledAmountUsd { get; set; }
    }
}