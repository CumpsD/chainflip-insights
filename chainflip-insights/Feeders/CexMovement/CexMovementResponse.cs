namespace ChainflipInsights.Feeders.CexMovement
{
    using System;
    using System.Text.Json.Serialization;

    public class CexMovementResponse
    {
        [JsonPropertyName("result")] 
        public CexMovementInfoResponseData Data { get; set; }
    }
    
    public class CexMovementInfoResponseData
    {
        [JsonPropertyName("rows")] 
        public CexMovementInfoResponseRow[] Data { get; set; }
    }

    public class CexMovementInfoResponseRow
    {
        // {
        //     "Day": 34,
        //     "movement": -294.06039877999865,
        //     "movement_in": 12443.3841937,
        //     "movement_out": -12737.444592479998
        // },
                
        [JsonPropertyName("Day")] 
        public int DayOfYear { get; set; }
        
        [JsonPropertyName("movement")] 
        public double TotalMovement { get; set; }
        
        [JsonPropertyName("movement_in")] 
        public double MovementIn { get; set; }
        
        [JsonPropertyName("movement_out")] 
        public double MovementOut { get; set; }
    }
}