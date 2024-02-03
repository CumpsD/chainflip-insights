namespace ChainflipInsights.Feeders.Funding
{
    using System;
    using System.Text.Json.Serialization;

    public class FundingResponse
    {
        [JsonPropertyName("data")] 
        public FundingInfoResponseData Data { get; set; }
    }
    
    public class FundingInfoResponseData
    {
        [JsonPropertyName("allValidatorFundingEvents")] 
        public FundingInfoResponseAllFunding Data { get; set; }
    }

    public class FundingInfoResponseAllFunding
    {
        [JsonPropertyName("edges")] 
        public FundingInfoResponseEdges[] Data { get; set; }
    }

    public class FundingInfoResponseEdges
    {
        [JsonPropertyName("node")] 
        public FundingInfoResponseNode Data { get; set; }
    }

    public class FundingInfoResponseNode
    {
        // "id": 3093,
        // "amount": "600000000000000000000",
        // "epochId": 77,
        // "validatorByValidatorId": {
        //     "alias": "StakedFLIP (Chorus One #10)",
        //     "idSs58": "cFNwGhUje3AJzBykDGs45umgFoGKS9xouSVn1UNz7VG1y4j4n",
        //     "cfeVersionId": "1.1.6"
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
        
        [JsonPropertyName("validatorByValidatorId")] 
        public FundingInfoValidator Validator { get; set; }
        
        [JsonPropertyName("eventByEventId")] 
        public FundingInfoEvent Event { get; set; }
    }

    public class FundingInfoValidator
    {
        [JsonPropertyName("alias")] 
        public string Alias { get; set; }

        [JsonPropertyName("idSs58")] 
        public string Name { get; set; }
    }

    public class FundingInfoEvent
    {  
        [JsonPropertyName("blockByBlockId")] 
        public FundingInfoStartBlock Block { get; set; }
    }

    public class FundingInfoStartBlock
    {
        [JsonPropertyName("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
    }
}