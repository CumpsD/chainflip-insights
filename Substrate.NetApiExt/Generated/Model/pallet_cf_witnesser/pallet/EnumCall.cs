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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_witnesser.pallet
{
    
    
    /// <summary>
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> witness_at_epoch
        /// See [`Pallet::witness_at_epoch`].
        /// </summary>
        witness_at_epoch = 0,
        
        /// <summary>
        /// >> force_witness
        /// See [`Pallet::force_witness`].
        /// </summary>
        force_witness = 1,
        
        /// <summary>
        /// >> prewitness
        /// See [`Pallet::prewitness`].
        /// </summary>
        prewitness = 2,
    }
    
    /// <summary>
    /// >> 83 - Variant[pallet_cf_witnesser.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall, Substrate.NetApi.Model.Types.Primitive.U32>, BaseTuple<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall, Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>
    {
    }
}
