namespace ChainflipInsights.Feeders.Liquidity
{
    using System.Text.Json.Serialization;

    public class IncomingLiquidityResponse
    {
        [JsonPropertyName("data")] 
        public IncomingLiquidityResponseData Data { get; set; }
    }
    
    public class IncomingLiquidityResponseData
    {
        [JsonPropertyName("allLiquidityDeposits")] 
        public IncomingLiquidityResponseAllLiquidityDeposits Data { get; set; }
    }

    public class IncomingLiquidityResponseAllLiquidityDeposits
    {
        [JsonPropertyName("edges")] 
        public IncomingLiquidityResponseEdges[] Data { get; set; }
    }

    public class IncomingLiquidityResponseEdges
    {
        [JsonPropertyName("node")] 
        public IncomingLiquidityResponseNode Data { get; set; }
    }

    public class IncomingLiquidityResponseNode
    {
        // "node": {
        //     "id": 96,
        //     "depositAmount": "90000000000",
        //     "depositValueUsd": "90000.000000000000000000000000000000",
        //     "channel": {
        //         "issuedBlockId": 1047281,
        //         "chain": "ETHEREUM",
        //         "asset": "USDC",
        //         "channelId": "10",
        //         "depositAddress": "0x137258139a480285d82cbb0af70fcdefffbc10cf",
        //         "isExpired": true
        //     }
        // }
                
        [JsonPropertyName("id")] 
        public double Id { get; set; }
        
        [JsonPropertyName("depositAmount")] 
        public double DepositAmount { get; set; }
        
        [JsonPropertyName("depositValueUsd")] 
        public double DepositValueUsd { get; set; }
        
        [JsonPropertyName("channel")] 
        public IncomingLiquidityResponseChannel Channel { get; set; }
    }

    public class IncomingLiquidityResponseChannel
    {
        //     "channel": {
        //         "issuedBlockId": 1047281,
        //         "chain": "ETHEREUM",
        //         "asset": "USDC",
        //         "channelId": "10",
        //         "depositAddress": "0x137258139a480285d82cbb0af70fcdefffbc10cf",
        //         "isExpired": true
        //     }
        
        [JsonPropertyName("issuedBlockId")] 
        public ulong IssuedBlockId { get; set; }
        
        [JsonPropertyName("chain")] 
        public string Chain { get; set; }
        
        [JsonPropertyName("asset")] 
        public string Asset { get; set; }
        
        [JsonPropertyName("channelId")] 
        public string ChannelId { get; set; }
        
        [JsonPropertyName("depositAddress")] 
        public string DepositAddress { get; set; }
        
        [JsonPropertyName("isExpired")] 
        public bool IsExpired { get; set; }
    }
}