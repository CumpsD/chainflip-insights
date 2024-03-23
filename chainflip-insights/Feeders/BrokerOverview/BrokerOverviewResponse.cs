namespace ChainflipInsights.Feeders.BrokerOverview
{
    using System.Text.Json.Serialization;

    public class BrokerOverviewResponse
    {
        [JsonPropertyName("data")] 
        public BrokerOverviewInfoResponseData Data { get; set; }
    }
    
    public class BrokerOverviewInfoResponseData
    {
        [JsonPropertyName("brokersAggregate")] 
        public BrokerOverviewInfoBroker[] Data { get; set; }
    }

    public class BrokerOverviewInfoBroker
    {
        [JsonPropertyName("idSs58")] 
        public string Ss58 { get; set; }
        
        [JsonPropertyName("swapCount")] 
        public double? Swaps { get; set; }
        
        [JsonPropertyName("swapFeeUsd")] 
        public double? Fees { get; set; }
        
        [JsonPropertyName("volume")] 
        public double? Volume { get; set; }
    }
}