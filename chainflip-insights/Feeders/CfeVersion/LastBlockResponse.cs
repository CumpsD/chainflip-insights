namespace ChainflipInsights.Feeders.CfeVersion
{
    using System.Text.Json.Serialization;

    public class LastBlockResponse
    {
        [JsonPropertyName("data")] 
        public LastBlockInfoResponseData Data { get; set; }
    }
    
    public class LastBlockInfoResponseData
    {
        [JsonPropertyName("allBlocks")] 
        public LastBlockInfoResponseAllBlocks Data { get; set; }
    }

    public class LastBlockInfoResponseAllBlocks
    {
        [JsonPropertyName("nodes")] 
        public LastBlockInfoResponseNode[] Data { get; set; }
    }

    public class LastBlockInfoResponseNode
    {
        [JsonPropertyName("id")] 
        public double Id { get; set; }
    }
}