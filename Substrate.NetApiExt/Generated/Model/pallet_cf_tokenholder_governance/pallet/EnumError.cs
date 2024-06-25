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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_tokenholder_governance.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> AlreadyBacked
        /// Proposal is already backed by the same account.
        /// </summary>
        AlreadyBacked = 0,
        
        /// <summary>
        /// >> ProposalDoesntExist
        /// Proposal doesn't exist.
        /// </summary>
        ProposalDoesntExist = 1,
        
        /// <summary>
        /// >> IncompatibleGovkey
        /// The proposed governance key is incompatible with the proposed chain.
        /// </summary>
        IncompatibleGovkey = 2,
    }
    
    /// <summary>
    /// >> 432 - Variant[pallet_cf_tokenholder_governance.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
