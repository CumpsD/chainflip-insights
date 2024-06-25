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
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> AuthorityIndexOutOfBounds
        /// CRITICAL: The authority index is out of bounds. This should never happen.
        /// </summary>
        AuthorityIndexOutOfBounds = 0,
        
        /// <summary>
        /// >> UnauthorisedWitness
        /// Witness is not an authority.
        /// </summary>
        UnauthorisedWitness = 1,
        
        /// <summary>
        /// >> DuplicateWitness
        /// A witness vote was cast twice by the same authority.
        /// </summary>
        DuplicateWitness = 2,
        
        /// <summary>
        /// >> EpochExpired
        /// The epoch has expired
        /// </summary>
        EpochExpired = 3,
        
        /// <summary>
        /// >> InvalidEpoch
        /// Invalid epoch
        /// </summary>
        InvalidEpoch = 4,
    }
    
    /// <summary>
    /// >> 404 - Variant[pallet_cf_witnesser.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
