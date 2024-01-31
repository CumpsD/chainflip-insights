namespace ChainflipInsights.Feeders.Swap
{
    using System.Text.Json.Serialization;

    public class SwapResponse
    {
        [JsonPropertyName("depositReceivedAt")]
        public long DepositReceivedAt { get; set; }
         
        [JsonPropertyName("broadcastSucceededAt")]
        public long BroadcastSucceededAt { get; set; }
    }
}