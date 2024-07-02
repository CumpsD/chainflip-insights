namespace ChainflipInsights.Feeders.Burn
{
    using System;
    using System.Text.Json.Serialization;

    public class BurnsResponse
    {
        [JsonPropertyName("data")] 
        public BurnsResponseData Data { get; set; }
    }
    
    public class BurnsResponseData
    {
        [JsonPropertyName("allBurns")] 
        public BurnsResponseAllBurns Data { get; set; }
    }

    public class BurnsResponseAllBurns
    {
        [JsonPropertyName("nodes")] 
        public BurnsResponseNode[] Data { get; set; }
    }
    
    public class BurnsResponseNode
    {
        [JsonPropertyName("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
        
        [JsonPropertyName("amount")] 
        public double BurnedAmount { get; set; }
        
        [JsonPropertyName("eventByEventId")] 
        public BurnsResponseEvent Event { get; set; }
    }

    public class BurnsResponseEvent
    {
        [JsonPropertyName("blockId")] 
        public ulong BlockId { get; set; }
    }
}