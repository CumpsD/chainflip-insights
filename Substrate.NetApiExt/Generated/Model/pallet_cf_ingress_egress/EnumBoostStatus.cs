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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress
{
    
    
    /// <summary>
    /// >> BoostStatus
    /// </summary>
    public enum BoostStatus
    {
        
        /// <summary>
        /// >> Boosted
        /// </summary>
        Boosted = 0,
        
        /// <summary>
        /// >> NotBoosted
        /// </summary>
        NotBoosted = 1,
    }
    
    /// <summary>
    /// >> 516 - Variant[pallet_cf_ingress_egress.BoostStatus]
    /// </summary>
    public sealed class EnumBoostStatus : BaseEnumExt<BoostStatus, BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U16>>, BaseVoid>
    {
    }
}
