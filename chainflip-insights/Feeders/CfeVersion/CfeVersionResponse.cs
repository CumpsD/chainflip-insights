namespace ChainflipInsights.Feeders.CfeVersion
{
    using System.Text.Json.Serialization;

    public class CfeVersionResponse
    {
        [JsonPropertyName("data")] 
        public CfeVersionInfoResponseData Data { get; set; }
    }
    
    public class CfeVersionInfoResponseData
    {
        [JsonPropertyName("allCfeVersions")] 
        public CfeVersionInfoResponseAllVersions Data { get; set; }
    }

    public class CfeVersionInfoResponseAllVersions
    {
        [JsonPropertyName("edges")] 
        public CfeVersionInfoResponseEdges[] Data { get; set; }
    }

    public class CfeVersionInfoResponseEdges
    {
        [JsonPropertyName("node")] 
        public CfeVersionInfoResponseNode Data { get; set; }
    }

    public class CfeVersionInfoResponseNode
    {
        // "id": "1.1.7",
        // "validatorsByCfeVersionId": {
        //     "edges": [
        //       {
        //         "node": {
        //            "accountByAccountId": {
        //                "idSs58": "cFP2cGErEhxzJfVUxk1gHVuE1ALxHJQx335o19bT7QoSWwjhU"
        //            }
        //           "lastHeartbeatBlockId": 1202408
        //         }
        //        ...
        //      },
                
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("validatorsByCfeVersionId")] 
        public CfeVersionValidatorsResponse Validators { get; set; }
    }

    public class CfeVersionValidatorsResponse
    {
        [JsonPropertyName("edges")] 
        public CfeVersionValidatorsResponseEdges[] Data { get; set; }
    }

    public class CfeVersionValidatorsResponseEdges
    {  
        [JsonPropertyName("node")] 
        public CfeVersionValidatorsNode Data { get; set; }
    }

    public class CfeVersionValidatorsNode
    {
        [JsonPropertyName("accountByAccountId")] 
        public CfeVersionAccountNode Account { get; set; }
        
        [JsonPropertyName("lastHeartbeatBlockId")]
        public double? LastHeartBeat { get; set; }
    }
    
    public class CfeVersionAccountNode
    {
        [JsonPropertyName("idSs58")] 
        public string Name { get; set; }
    }
}