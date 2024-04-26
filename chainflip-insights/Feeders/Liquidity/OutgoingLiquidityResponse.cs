namespace ChainflipInsights.Feeders.Liquidity
{
    using System;
    using System.Text.Json.Serialization;

    public class OutgoingLiquidityResponse
    {
        [JsonPropertyName("data")] 
        public OutgoingLiquidityResponseData Data { get; set; }
    }
    
    public class OutgoingLiquidityResponseData
    {
        [JsonPropertyName("allLiquidityWithdrawals")] 
        public OutgoingLiquidityResponseAllLiquidityWithdrawals Data { get; set; }
    }

    public class OutgoingLiquidityResponseAllLiquidityWithdrawals
    {
        [JsonPropertyName("edges")] 
        public OutgoingLiquidityResponseEdges[] Data { get; set; }
    }

    public class OutgoingLiquidityResponseEdges
    {
        [JsonPropertyName("node")] 
        public OutgoingLiquidityResponseNode Data { get; set; }
    }

    public class OutgoingLiquidityResponseNode
    {
        // "node": {
        //     "id": 91,
        //     "amount": "2499970317",
        //     "valueUsd": "1609355.891568750000000000000000000000",
        //     "chain": "Bitcoin",
        //     "asset": "Btc",
        //     "block": {
        //         "timestamp": "2024-04-25T07:17:12+00:00"
        //     }
        // }
                
        [JsonPropertyName("id")] 
        public double Id { get; set; }
        
        [JsonPropertyName("amount")] 
        public double WithdrawalAmount { get; set; }
        
        [JsonPropertyName("valueUsd")] 
        public double WithdrawalValueUsd { get; set; }
        
        [JsonPropertyName("chain")] 
        public string Chain { get; set; }
        
        [JsonPropertyName("asset")] 
        public string Asset { get; set; }
        
        [JsonPropertyName("block")] 
        public OutgoingLiquidityResponseBlock Block { get; set; }
    }

    public class OutgoingLiquidityResponseBlock
    {
        [JsonPropertyName("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
        
        [JsonPropertyName("id")] 
        public ulong BlockId { get; set; }
    }
}