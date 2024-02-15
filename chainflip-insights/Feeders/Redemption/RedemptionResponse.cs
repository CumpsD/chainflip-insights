namespace ChainflipInsights.Feeders.Redemption
{
    using System;
    using System.Text.Json.Serialization;

    public class RedemptionResponse
    {
        [JsonPropertyName("data")] 
        public RedemptionInfoResponseData Data { get; set; }
    }
    
    public class RedemptionInfoResponseData
    {
        [JsonPropertyName("allAccountFundingEvents")] 
        public RedemptionInfoResponseAllRedemption Data { get; set; }
    }

    public class RedemptionInfoResponseAllRedemption
    {
        [JsonPropertyName("edges")] 
        public RedemptionInfoResponseEdges[] Data { get; set; }
    }

    public class RedemptionInfoResponseEdges
    {
        [JsonPropertyName("node")] 
        public RedemptionInfoResponseNode Data { get; set; }
    }

    public class RedemptionInfoResponseNode
    {
        // "id": 3093,
        // "amount": "600000000000000000000",
        // "epochId": 77,
        // "accountByAccountId": {
        //     "alias": "StakedFLIP (Chorus One #10)",
        //     "idSs58": "cFNwGhUje3AJzBykDGs45umgFoGKS9xouSVn1UNz7VG1y4j4n",
        //     "role": "VALIDATOR"
        // },
        // "eventByEventId": {
        //     "blockByBlockId": {
        //         "timestamp": "2024-02-03T04:43:48+00:00"
        //     }
        // }
                
        [JsonPropertyName("id")] 
        public double Id { get; set; }
        
        [JsonPropertyName("amount")] 
        public double Amount { get; set; }
        
        [JsonPropertyName("epochId")] 
        public double Epoch { get; set; }
        
        [JsonPropertyName("accountByAccountId")] 
        public RedemptionInfoAccount Account { get; set; }
        
        [JsonPropertyName("eventByEventId")] 
        public RedemptionInfoEvent Event { get; set; }
    }

    public class RedemptionInfoAccount
    {
        [JsonPropertyName("alias")] 
        public string Alias { get; set; }

        [JsonPropertyName("idSs58")] 
        public string Name { get; set; }
                
        [JsonPropertyName("role")] 
        public string Role { get; set; }
    }

    public class RedemptionInfoEvent
    {  
        [JsonPropertyName("blockByBlockId")] 
        public RedemptionInfoStartBlock Block { get; set; }
    }

    public class RedemptionInfoStartBlock
    {
        [JsonPropertyName("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
    }
}