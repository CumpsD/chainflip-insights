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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_reputation.pallet
{
    
    
    /// <summary>
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> update_accrual_ratio
        /// See [`Pallet::update_accrual_ratio`].
        /// </summary>
        update_accrual_ratio = 0,
        
        /// <summary>
        /// >> update_missed_heartbeat_penalty
        /// See [`Pallet::update_missed_heartbeat_penalty`].
        /// </summary>
        update_missed_heartbeat_penalty = 1,
        
        /// <summary>
        /// >> set_penalty
        /// See [`Pallet::set_penalty`].
        /// </summary>
        set_penalty = 2,
        
        /// <summary>
        /// >> heartbeat
        /// See [`Pallet::heartbeat`].
        /// </summary>
        heartbeat = 3,
    }
    
    /// <summary>
    /// >> 133 - Variant[pallet_cf_reputation.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<Substrate.NetApi.Model.Types.Primitive.I32, Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Primitive.I32, BaseTuple<Substrate.NetApiExt.Generated.Model.state_chain_runtime.chainflip.offences.EnumOffence, Substrate.NetApiExt.Generated.Model.pallet_cf_reputation.Penalty>, BaseVoid>
    {
    }
}
