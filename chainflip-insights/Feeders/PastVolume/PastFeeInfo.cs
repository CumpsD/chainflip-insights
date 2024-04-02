namespace ChainflipInsights.Feeders.PastVolume
{
    public class PastFeeInfo
    {
        public double NetworkFee { get; }
        
        public PastFeeInfo(PastFeesInfoResponse response)
        {
            NetworkFee = 
                (response.SourceAsset, response.SourceChain, response.DestinationAsset, response.DestinationChain) switch
                {
                    ("Usdc", "Ethereum", _, _) =>
                        // Take network fee from swapInputValueUsd
                        response.SwapInputValue / 100 * 0.10,
                    
                    (_, _, "Usdc", "Ethereum") =>
                        // Take network fee from swapOutputValueUsd
                        response.SwapOutputValue / 100 * 0.10,
                    
                    _ => 
                        // Take network fee from intermediateValueUsd
                        (response.IntermediateValue ?? 0) / 100 * 0.10
                };
        }
    }
}