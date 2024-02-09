namespace ChainflipInsights.Feeders.PastVolume
{
    using System.Text.Json.Serialization;

    public class PastVolumeResponse
    {
        [JsonPropertyName("data")] 
        public PastVolumeInfoResponseData Data { get; set; }
    }
    
    public class PastVolumeInfoResponseData
    {
        [JsonPropertyName("allPoolSwaps")] 
        public PastVolumeInfoResponseAllSwaps Data { get; set; }
    }

    public class PastVolumeInfoResponseAllSwaps
    {
        [JsonPropertyName("groupedAggregates")] 
        public PastVolumeInfoResponseAggregates[] Data { get; set; }
    }

    public class PastVolumeInfoResponseAggregates
    {
        [JsonPropertyName("fromAssetToAsset")] 
        public string[] Assets { get; set; }
        
        [JsonPropertyName("sum")] 
        public PastVolumeInfoResponseSum Sum { get; set; }
    }

    public class PastVolumeInfoResponseSum
    {
        [JsonPropertyName("toValueUsd")] 
        public double Value { get; set; }
        
        [JsonPropertyName("liquidityFeeValueUsd")] 
        public double Fees { get; set; }
    }
}