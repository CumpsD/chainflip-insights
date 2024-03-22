namespace ChainflipInsights.Feeders.StakedFlip
{
    using System.Text.Json.Serialization;

    public class StakedFlipResponse
    {
        [JsonPropertyName("data")] 
        public StakedFlipResponseData Data { get; set; }
    }
    
    public class StakedFlipResponseData
    {
        [JsonPropertyName("stuff")] 
        public StakedFlipData[] Data { get; set; }
    }

    public class StakedFlipData
    {
        [JsonPropertyName("blockTimestamp")] 
        public string BlockTimestamp { get; set; }
        
        [JsonPropertyName("amount")] 
        public double Amount { get; set; }
    }
}