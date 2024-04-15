namespace ChainflipInsights.Feeders.Swap
{
    using System.Text.Json.Serialization;

    public class SwapsResponse
    {
        [JsonPropertyName("data")] 
        public SwapsResponseData Data { get; set; }
    }
    
    public class SwapsResponseData
    {
        [JsonPropertyName("allSwaps")] 
        public SwapsResponseAllSwaps Data { get; set; }
    }

    public class SwapsResponseAllSwaps
    {
        [JsonPropertyName("edges")] 
        public SwapsResponseEdges[] Data { get; set; }
    }

    public class SwapsResponseEdges
    {
        [JsonPropertyName("node")] 
        public SwapsResponseNode Data { get; set; }
    }

    public class SwapsResponseNode
    {
        // "node": {
        //     "id": 374,
        //     "nativeId": "374",
        //     "swapScheduledBlockTimestamp": "2024-01-29T20:20:54+00:00",
        //     "depositAmount": "2000000000000000000000",
        //     "depositValueUsd": "10020.000000000000000000000000000000",
        //     "sourceAsset": "FLIP",
        //     "egressAmount": "9971679630",
        //     "egressValueUsd": "9971.679630000000000000000000000000",
        //     "destinationAsset": "USDC",
        //     "destinationAddress": "0xc79fb6449c121a8f453eec2bc78bac857711cb1d",
        //     "intermediateAmount": null,
        //     "intermediateValueUsd": null
        // }
        
        [JsonPropertyName("id")] 
        public double Id { get; set; }
        
        [JsonPropertyName("nativeId")] 
        public double NativeId { get; set; }
        
        [JsonPropertyName("swapScheduledBlockTimestamp")] 
        public string SwapScheduledBlockTimestamp { get; set; }
        
        [JsonPropertyName("depositAmount")] 
        public double DepositAmount { get; set; }
        
        [JsonPropertyName("depositValueUsd")] 
        public double DepositValueUsd { get; set; }
        
        [JsonPropertyName("sourceAsset")] 
        public string SourceAsset { get; set; }
        
        [JsonPropertyName("egressAmount")] 
        public double EgressAmount { get; set; }
        
        [JsonPropertyName("egressValueUsd")] 
        public double EgressValueUsd { get; set; }
        
        [JsonPropertyName("destinationAsset")] 
        public string DestinationAsset { get; set; }
        
        [JsonPropertyName("destinationAddress")] 
        public string DestinationAddress { get; set; }
        
        [JsonPropertyName("intermediateAmount")] 
        public double? IntermediateAmount { get; set; }

        [JsonPropertyName("intermediateValueUsd")] 
        public double? IntermediateValueUsd { get; set; }
        
        [JsonPropertyName("swapChannelByDepositChannelId")] 
        public SwapChannelByDepositChannelId SwapChannel { get; set; }
    }

    public class SwapChannelByDepositChannelId
    {
        [JsonPropertyName("brokerByBrokerId")] 
        public BrokerByBrokerId Broker { get; set; }
    }

    public class BrokerByBrokerId
    {
        [JsonPropertyName("accountByAccountId")] 
        public AccountByAccountId Account { get; set; }
    }

    public class AccountByAccountId
    {
        [JsonPropertyName("idSs58")] 
        public string Ss58 { get; set; }
    }
}