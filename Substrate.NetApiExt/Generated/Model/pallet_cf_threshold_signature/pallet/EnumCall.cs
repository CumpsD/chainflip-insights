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
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> signature_success
        /// See [`Pallet::signature_success`].
        /// </summary>
        signature_success = 0,
        
        /// <summary>
        /// >> report_signature_failed
        /// See [`Pallet::report_signature_failed`].
        /// </summary>
        report_signature_failed = 1,
        
        /// <summary>
        /// >> set_threshold_signature_timeout
        /// See [`Pallet::set_threshold_signature_timeout`].
        /// </summary>
        set_threshold_signature_timeout = 2,
        
        /// <summary>
        /// >> report_keygen_outcome
        /// See [`Pallet::report_keygen_outcome`].
        /// </summary>
        report_keygen_outcome = 3,
        
        /// <summary>
        /// >> report_key_handover_outcome
        /// See [`Pallet::report_key_handover_outcome`].
        /// </summary>
        report_key_handover_outcome = 4,
        
        /// <summary>
        /// >> on_keygen_verification_result
        /// See [`Pallet::on_keygen_verification_result`].
        /// </summary>
        on_keygen_verification_result = 5,
        
        /// <summary>
        /// >> on_handover_verification_result
        /// See [`Pallet::on_handover_verification_result`].
        /// </summary>
        on_handover_verification_result = 6,
        
        /// <summary>
        /// >> set_keygen_response_timeout
        /// See [`Pallet::set_keygen_response_timeout`].
        /// </summary>
        set_keygen_response_timeout = 7,
        
        /// <summary>
        /// >> set_keygen_slash_amount
        /// See [`Pallet::set_keygen_slash_amount`].
        /// </summary>
        set_keygen_slash_amount = 8,
    }
    
    /// <summary>
    /// >> 164 - Variant[pallet_cf_threshold_signature.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Types.Base.Arr64U8>>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1>, Substrate.NetApi.Model.Types.Primitive.U32, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.EnumResult>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.EnumResult>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey>, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U128>
    {
    }
}
