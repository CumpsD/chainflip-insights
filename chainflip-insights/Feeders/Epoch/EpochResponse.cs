namespace ChainflipInsights.Feeders.Epoch
{
    using System;
    using System.Text.Json.Serialization;

    public class EpochResponse
    {
        [JsonPropertyName("data")] 
        public EpochInfoResponseData Data { get; set; }
    }
    
    public class EpochInfoResponseData
    {
        [JsonPropertyName("allEpoches")] 
        public EpochInfoResponseAllEpoches Data { get; set; }
    }

    public class EpochInfoResponseAllEpoches
    {
        [JsonPropertyName("edges")] 
        public EpochInfoResponseEdges[] Data { get; set; }
    }

    public class EpochInfoResponseEdges
    {
        [JsonPropertyName("node")] 
        public EpochInfoResponseNode Data { get; set; }
    }

    public class EpochInfoResponseNode
    {
        //  id": 78,
        // "bond": "166815949362917175801528",
        // "totalBonded": "25022392404437576370000000",
        // "startBlockId": 1169857,
        // "blockByStartBlockId": {
        //     "timestamp": "2024-02-03T05:04:54+00:00"
        // },
        // "authorityMembershipsByEpochId": {
        //     "edges": [
        //         {
        //             "node": {
        //                 "validatorId": 66,
        //                 "validatorByValidatorId": {
        //                     "idSs58": "cFHsUrS1t97KUgK8m6yU3RufoUSBpfYyqueeGswAuJScXcWAD"
        //                 },
        //                 "bid": "180050930793365340000000",
        //                 "reward": "36440906077714169992"
        //             }
        //         },
        //         ...
                
        [JsonPropertyName("id")] 
        public double Id { get; set; }
        
        [JsonPropertyName("bond")] 
        public double Bond { get; set; }
        
        [JsonPropertyName("totalBonded")] 
        public double TotalBonded { get; set; }
        
        [JsonPropertyName("blockByStartBlockId")] 
        public EpochInfoStartBlock StartBlock { get; set; }
        
        [JsonPropertyName("authorityMembershipsByEpochId")] 
        public EpochInfoAuthorityMemberships AuthorityMemberships { get; set; }
    }

    public class EpochInfoStartBlock
    {
        [JsonPropertyName("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
    }

    public class EpochInfoAuthorityMemberships
    {
        [JsonPropertyName("edges")] 
        public EpochInfoAuthorityMembershipsEdges[] Data { get; set; }
    }

    public class EpochInfoAuthorityMembershipsEdges
    {
        [JsonPropertyName("node")] 
        public EpochInfoAuthorityMembershipsNode Data { get; set; }
    }

    public class EpochInfoAuthorityMembershipsNode
    {
        [JsonPropertyName("bid")] 
        public double Bid { get; set; }
        
        [JsonPropertyName("reward")] 
        public double Reward { get; set; }
    }
}