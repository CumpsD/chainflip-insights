namespace ChainflipInsights.Feeders.Burn
{
    using System.Text.Json.Serialization;

    public class BlockHashResponse
    {
        [JsonPropertyName("result")] 
        public string? Result { get; set; }
    }
}