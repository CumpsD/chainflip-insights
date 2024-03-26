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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> TransactionBroadcastRequest
        /// A request to a specific authority to sign a transaction.
        /// </summary>
        TransactionBroadcastRequest = 0,
        
        /// <summary>
        /// >> BroadcastRetryScheduled
        /// A failed broadcast has been scheduled for retry.
        /// </summary>
        BroadcastRetryScheduled = 1,
        
        /// <summary>
        /// >> BroadcastTimeout
        /// A broadcast has timed out.
        /// </summary>
        BroadcastTimeout = 2,
        
        /// <summary>
        /// >> BroadcastAborted
        /// A broadcast has been aborted after all authorities have failed to broadcast it.
        /// </summary>
        BroadcastAborted = 3,
        
        /// <summary>
        /// >> BroadcastSuccess
        /// A broadcast has successfully been completed.
        /// </summary>
        BroadcastSuccess = 4,
        
        /// <summary>
        /// >> ThresholdSignatureInvalid
        /// A broadcast's threshold signature is invalid, we will attempt to re-sign it.
        /// </summary>
        ThresholdSignatureInvalid = 5,
        
        /// <summary>
        /// >> BroadcastCallbackExecuted
        /// A signature accepted event on the target chain has been witnessed and the callback was
        /// executed.
        /// </summary>
        BroadcastCallbackExecuted = 6,
        
        /// <summary>
        /// >> TransactionFeeDeficitRecorded
        /// The fee paid for broadcasting a transaction has been recorded.
        /// </summary>
        TransactionFeeDeficitRecorded = 7,
        
        /// <summary>
        /// >> TransactionFeeDeficitRefused
        /// The fee paid for broadcasting a transaction has been refused.
        /// </summary>
        TransactionFeeDeficitRefused = 8,
        
        /// <summary>
        /// >> CallResigned
        /// A Call has been re-threshold-signed, and its signature data is inserted into storage.
        /// </summary>
        CallResigned = 9,
    }
    
    /// <summary>
    /// >> 281 - Variant[pallet_cf_broadcast.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApiExt.Generated.Model.cf_chains.btc.BitcoinTransactionData, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>, Substrate.NetApi.Model.Types.Primitive.U32, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Types.Base.EnumResult>, BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey, Substrate.NetApi.Model.Types.Primitive.U64>, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey, Substrate.NetApi.Model.Types.Primitive.U32>
    {
    }
}
