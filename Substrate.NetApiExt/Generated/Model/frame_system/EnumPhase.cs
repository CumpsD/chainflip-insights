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


namespace Substrate.NetApiExt.Generated.Model.frame_system
{
    
    
    /// <summary>
    /// >> Phase
    /// </summary>
    public enum Phase
    {
        
        /// <summary>
        /// >> ApplyExtrinsic
        /// </summary>
        ApplyExtrinsic = 0,
        
        /// <summary>
        /// >> Finalization
        /// </summary>
        Finalization = 1,
        
        /// <summary>
        /// >> Initialization
        /// </summary>
        Initialization = 2,
    }
    
    /// <summary>
    /// >> 366 - Variant[frame_system.Phase]
    /// </summary>
    public sealed class EnumPhase : BaseEnumExt<Phase, Substrate.NetApi.Model.Types.Primitive.U32, BaseVoid, BaseVoid>
    {
    }
}
