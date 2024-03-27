//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> ThresholdSignatureRequest
        /// </summary>
        ThresholdSignatureRequest = 0,
        
        /// <summary>
        /// >> ThresholdSignatureFailed
        /// </summary>
        ThresholdSignatureFailed = 1,
        
        /// <summary>
        /// >> ThresholdSignatureSuccess
        /// The threshold signature posted back to the chain was verified.
        /// </summary>
        ThresholdSignatureSuccess = 2,
        
        /// <summary>
        /// >> ThresholdDispatchComplete
        /// We have had a signature success and we have dispatched the associated callback
        /// </summary>
        ThresholdDispatchComplete = 3,
        
        /// <summary>
        /// >> RetryRequested
        /// </summary>
        RetryRequested = 4,
        
        /// <summary>
        /// >> FailureReportProcessed
        /// </summary>
        FailureReportProcessed = 5,
        
        /// <summary>
        /// >> SignersUnavailable
        /// Not enough signers were available to reach threshold.
        /// </summary>
        SignersUnavailable = 6,
        
        /// <summary>
        /// >> ThresholdSignatureResponseTimeoutUpdated
        /// The threshold signature response timeout has been updated
        /// </summary>
        ThresholdSignatureResponseTimeoutUpdated = 7,
    }
    
    /// <summary>
    /// >> 274 - Variant[pallet_cf_threshold_signature.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEventEth : BaseEnumExt<
        Event, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApiExt.Generated.Model.cf_chains.evm.AggKey, 
            Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1, 
            Substrate.NetApiExt.Generated.Model.primitive_types.H256>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApi.Model.Types.Base.BaseVec<
                Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32,
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32,
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U32>, 
        Substrate.NetApi.Model.Types.Primitive.U32>
    {
    }
    
    /// <summary>
    /// >> 276 - Variant[pallet_cf_threshold_signature.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEventDot : BaseEnumExt<
        Event, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId, 
            Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1, 
            Substrate.NetApiExt.Generated.Model.cf_chains.dot.EncodedPolkadotPayload>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApi.Model.Types.Base.BaseVec<
                Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32,
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32,
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U32>, 
        Substrate.NetApi.Model.Types.Primitive.U32>
    {
    }
    
    /// <summary>
    /// >> 277 - Variant[pallet_cf_threshold_signature.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEventBtc : BaseEnumExt<
        Event, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey, 
            Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1, 
            Substrate.NetApi.Model.Types.Base.BaseVec<
                Substrate.NetApi.Model.Types.Base.BaseTuple<
                    Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumPreviousOrCurrent>>>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApi.Model.Types.Base.BaseVec<
                Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32,
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U64>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32,
            Substrate.NetApi.Model.Types.Primitive.U64, 
            Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, 
        BaseTuple<
            Substrate.NetApi.Model.Types.Primitive.U32, 
            Substrate.NetApi.Model.Types.Primitive.U32>, 
        Substrate.NetApi.Model.Types.Primitive.U32>
    {
    }
}
