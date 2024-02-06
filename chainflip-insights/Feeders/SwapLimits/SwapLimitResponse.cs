namespace ChainflipInsights.Feeders.SwapLimits
{
    using System.Text.Json.Serialization;

    public class SwapLimitResponse
    {
        [JsonPropertyName("result")] 
        public double Result { get; set; }
    }
}