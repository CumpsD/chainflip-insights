namespace ChainflipInsights.Feeders.PastVolume
{
    using System.Text.Json.Serialization;

    public class PastFeesResponse
    {
        [JsonPropertyName("data")] 
        public PastFeesInfoResponseData Data { get; set; }
    }
    
    public class PastFeesInfoResponseData
    {
        [JsonPropertyName("allSwaps")] 
        public PastFeesInfoResponseAllSwaps Data { get; set; }
    }

    public class PastFeesInfoResponseAllSwaps
    {
        [JsonPropertyName("nodes")] 
        public PastFeesInfoResponse[] Data { get; set; }
    }
    
    public class PastFeesInfoResponse
    {
        [JsonPropertyName("sourceAsset")] 
        public string SourceAsset { get; set; }
        
        [JsonPropertyName("sourceChain")] 
        public string SourceChain { get; set; }
        
        [JsonPropertyName("swapInputValueUsd")] 
        public double SwapInputValue { get; set; }
        
        [JsonPropertyName("intermediateValueUsd")] 
        public double? IntermediateValue { get; set; }
        
        [JsonPropertyName("swapOutputValueUsd")] 
        public double SwapOutputValue { get; set; }
        
        [JsonPropertyName("destinationAsset")] 
        public string DestinationAsset { get; set; }
        
        [JsonPropertyName("destinationChain")] 
        public string DestinationChain { get; set; }
    }
}