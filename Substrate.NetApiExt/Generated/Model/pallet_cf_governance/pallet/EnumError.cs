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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> AlreadyApproved
        /// An account already approved a proposal
        /// </summary>
        AlreadyApproved = 0,
        
        /// <summary>
        /// >> NotMember
        /// The signer of an extrinsic is no member of the current governance
        /// </summary>
        NotMember = 1,
        
        /// <summary>
        /// >> ProposalNotFound
        /// The proposal was not found - it may have expired or it may already be executed
        /// </summary>
        ProposalNotFound = 2,
        
        /// <summary>
        /// >> DecodeOfCallFailed
        /// Decode of call failed
        /// </summary>
        DecodeOfCallFailed = 3,
        
        /// <summary>
        /// >> DecodeMembersLenFailed
        /// Decoding Members len failed.
        /// </summary>
        DecodeMembersLenFailed = 4,
        
        /// <summary>
        /// >> UpgradeConditionsNotMet
        /// A runtime upgrade has failed because the upgrade conditions were not satisfied
        /// </summary>
        UpgradeConditionsNotMet = 5,
        
        /// <summary>
        /// >> CallHashNotWhitelisted
        /// The call hash was not whitelisted
        /// </summary>
        CallHashNotWhitelisted = 6,
        
        /// <summary>
        /// >> NotEnoughAuthoritiesCfesAtTargetVersion
        /// Insufficient number of CFEs are at the target version to receive the runtime upgrade.
        /// </summary>
        NotEnoughAuthoritiesCfesAtTargetVersion = 7,
    }
    
    /// <summary>
    /// >> 363 - Variant[pallet_cf_governance.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}