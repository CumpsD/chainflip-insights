namespace ChainflipInsights.Feeders.BigStakedFlip
{
    using System.Text.Json.Serialization;

    public class BigStakedFlipResponse
    {
        [JsonPropertyName("data")] 
        public BigStakedFlipResponseData Data { get; set; }
    }
    
    public class BigStakedFlipResponseData
    {
        [JsonPropertyName("stuff")] 
        public BigStakedFlipData[] Data { get; set; }
    }

    public class BigStakedFlipData
    {
        [JsonPropertyName("blockNumber")] 
        public double BlockNumber { get; set; }

        [JsonPropertyName("blockTimestamp")] 
        public string BlockTimestamp { get; set; }
        
        [JsonPropertyName("amount")] 
        public double Amount { get; set; }
        
        [JsonPropertyName("to")] 
        public string To { get; set; }
        
        [JsonPropertyName("transactionHash")] 
        public string TransactionHash { get; set; }
    }
}