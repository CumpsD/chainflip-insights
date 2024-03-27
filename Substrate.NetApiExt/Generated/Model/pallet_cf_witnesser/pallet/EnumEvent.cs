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
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> WitnessExecutionFailed
        /// A witness call has failed.
        /// </summary>
        WitnessExecutionFailed = 0,
        
        /// <summary>
        /// >> Prewitnessed
        /// A an external event has been pre-witnessed.
        /// </summary>
        Prewitnessed = 1,
    }
    
    /// <summary>
    /// >> 64 - Variant[pallet_cf_witnesser.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, BaseTuple<Substrate.NetApiExt.Generated.Model.pallet_cf_witnesser.pallet.CallHash, Substrate.NetApiExt.Generated.Model.sp_runtime.EnumDispatchError>, Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>
    {
    }
}