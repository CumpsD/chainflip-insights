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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_chain_tracking.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> ChainStateUpdated
        /// The tracked state of this chain has been updated.
        /// </summary>
        ChainStateUpdated = 0,
    }
    
    /// <summary>
    /// >> 268 - Variant[pallet_cf_chain_tracking.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEventEth : BaseEnumExt<
        Event, 
        Substrate.NetApiExt.Generated.Model.cf_chains.ChainStateT1>
    {
    }
    
    /// <summary>
    /// >> 269 - Variant[pallet_cf_chain_tracking.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEventDot : BaseEnumExt<
        Event, 
        Substrate.NetApiExt.Generated.Model.cf_chains.ChainStateT2>
    {
    }
    
    /// <summary>
    /// >> 270 - Variant[pallet_cf_chain_tracking.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEventBtc : BaseEnumExt<
        Event, 
        Substrate.NetApiExt.Generated.Model.cf_chains.ChainStateT3>
    {
    }
}
