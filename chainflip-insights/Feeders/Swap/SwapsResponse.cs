namespace ChainflipInsights.Feeders.Swap
{
    using System.Text.Json.Serialization;

    public class SwapsResponseWrapper
    {
        public bool ContainsResponse { get; set; }
        
        public SwapsResponse? SwapsResponse { get; set; }
    }
    
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
        
        [JsonPropertyName("type")] 
        public string SwapType { get; set; }
        
        [JsonPropertyName("swapScheduledBlockTimestamp")] 
        public string SwapScheduledBlockTimestamp { get; set; }
        
        [JsonPropertyName("depositAmount")] 
        public double DepositAmount { get; set; }
        
        [JsonPropertyName("depositValueUsd")] 
        public double DepositValueUsd { get; set; }
        
        [JsonPropertyName("sourceAsset")] 
        public string SourceAsset { get; set; }
        
        [JsonPropertyName("egressAmount")] 
        public double? EgressAmount { get; set; }
        
        [JsonPropertyName("egressValueUsd")] 
        public double? EgressValueUsd { get; set; }
        
        [JsonPropertyName("destinationAsset")] 
        public string DestinationAsset { get; set; }
        
        [JsonPropertyName("destinationAddress")] 
        public string DestinationAddress { get; set; }
        
        [JsonPropertyName("intermediateAmount")] 
        public double? IntermediateAmount { get; set; }

        [JsonPropertyName("intermediateValueUsd")] 
        public double? IntermediateValueUsd { get; set; }
        
        [JsonPropertyName("swapInputAmount")] 
        public double? SwapInputAmount { get; set; }

        [JsonPropertyName("swapInputValueUsd")] 
        public double? SwapInputValueUsd { get; set; }
        
        [JsonPropertyName("swapOutputAmount")] 
        public double? SwapOutputAmount { get; set; }

        [JsonPropertyName("swapOutputValueUsd")] 
        public double? SwapOutputValueUsd { get; set; }
        
        [JsonPropertyName("swapChannelByDepositChannelId")] 
        public SwapChannelByDepositChannelId SwapChannel { get; set; }
        
        [JsonPropertyName("effectiveBoostFeeBps")] 
        public double? EffectiveBoostFeeBps { get; set; }
        
        [JsonPropertyName("swapFeesBySwapId")] 
        public SwapFeesBySwapId SwapFees { get; set; }
        
        [JsonPropertyName("egress")] 
        public Egress Egress { get; set; }
        
        [JsonPropertyName("predeposit")] 
        public BlockTime? PreDeposit { get; set; }
        
        [JsonPropertyName("deposit")] 
        public BlockTime Deposit { get; set; }
    }

    public class Egress
    {
        [JsonPropertyName("blockByBlockId")] 
        public Block Block { get; set; }
    }

    public class Block
    {
        [JsonPropertyName("timestamp")] 
        public string StateChainTimestamp { get; set; }
    }

    public class BlockTime
    {
        [JsonPropertyName("stateChainTimestamp")] 
        public string StateChainTimestamp { get; set; }
    }

    public class SwapChannelByDepositChannelId
    {
        [JsonPropertyName("brokerByBrokerId")] 
        public BrokerByBrokerId Broker { get; set; }
        
        [JsonPropertyName("swapChannelBeneficiariesByDepositChannelId")]
        public BeneficiariesByDepositChannelId Beneficiaries { get; set; }
    }

    public class BeneficiariesByDepositChannelId
    {
        [JsonPropertyName("nodes")] 
        public BeneficiariesNode[] Data { get; set; }
    }

    public class BeneficiariesNode
    {
        [JsonPropertyName("type")] 
        public string Type { get; set; }
        
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

    public class SwapFeesBySwapId
    {
        [JsonPropertyName("edges")] 
        public SwapFeesBySwapIdEdges[] Data { get; set; }
    }

    public class SwapFeesBySwapIdEdges
    {
        [JsonPropertyName("node")] 
        public SwapFeesBySwapIdNode Data { get; set; }
    }

    public class SwapFeesBySwapIdNode
    {
        [JsonPropertyName("type")] 
        public string FeeType { get; set; }
        
        [JsonPropertyName("amount")] 
        public string FeeAmount { get; set; }
        
        [JsonPropertyName("asset")] 
        public string FeeAsset { get; set; }
        
        [JsonPropertyName("valueUsd")] 
        public double? FeeValueUsd { get; set; }
    }
}