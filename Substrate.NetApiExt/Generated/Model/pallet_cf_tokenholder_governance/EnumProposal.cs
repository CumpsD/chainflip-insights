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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_tokenholder_governance
{
    
    
    /// <summary>
    /// >> Proposal
    /// </summary>
    public enum Proposal
    {
        
        /// <summary>
        /// >> SetGovernanceKey
        /// </summary>
        SetGovernanceKey = 0,
        
        /// <summary>
        /// >> SetCommunityKey
        /// </summary>
        SetCommunityKey = 1,
    }
    
    /// <summary>
    /// >> 115 - Variant[pallet_cf_tokenholder_governance.Proposal]
    /// </summary>
    public sealed class EnumProposal : BaseEnumExt<Proposal, BaseTuple<Substrate.NetApiExt.Generated.Model.cf_primitives.chains.EnumForeignChain, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>, Substrate.NetApiExt.Generated.Model.primitive_types.H160>
    {
    }
}
