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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.pallet
{
    
    
    /// <summary>
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> report_keygen_outcome
        /// See [`Pallet::report_keygen_outcome`].
        /// </summary>
        report_keygen_outcome = 0,
        
        /// <summary>
        /// >> report_key_handover_outcome
        /// See [`Pallet::report_key_handover_outcome`].
        /// </summary>
        report_key_handover_outcome = 1,
        
        /// <summary>
        /// >> on_keygen_verification_result
        /// See [`Pallet::on_keygen_verification_result`].
        /// </summary>
        on_keygen_verification_result = 2,
        
        /// <summary>
        /// >> on_handover_verification_result
        /// See [`Pallet::on_handover_verification_result`].
        /// </summary>
        on_handover_verification_result = 7,
        
        /// <summary>
        /// >> vault_key_rotated
        /// See [`Pallet::vault_key_rotated`].
        /// </summary>
        vault_key_rotated = 3,
        
        /// <summary>
        /// >> vault_key_rotated_externally
        /// See [`Pallet::vault_key_rotated_externally`].
        /// </summary>
        vault_key_rotated_externally = 4,
        
        /// <summary>
        /// >> set_keygen_response_timeout
        /// See [`Pallet::set_keygen_response_timeout`].
        /// </summary>
        set_keygen_response_timeout = 5,
        
        /// <summary>
        /// >> set_keygen_slash_amount
        /// See [`Pallet::set_keygen_slash_amount`].
        /// </summary>
        set_keygen_slash_amount = 6,
    }
    
    /// <summary>
    /// >> 141 - Variant[pallet_cf_vaults.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey>, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>, BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey, Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U128, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey>>
    {
    }
}
