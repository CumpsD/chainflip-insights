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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> InvalidCeremonyId
        /// The provided ceremony id is invalid.
        /// </summary>
        InvalidCeremonyId = 0,
        
        /// <summary>
        /// >> InvalidThresholdSignature
        /// The provided threshold signature is invalid.
        /// </summary>
        InvalidThresholdSignature = 1,
        
        /// <summary>
        /// >> InvalidRespondent
        /// The reporting party is not one of the signatories for this ceremony, or has already
        /// responded.
        /// </summary>
        InvalidRespondent = 2,
        
        /// <summary>
        /// >> InvalidRequestId
        /// The request Id is stale or not yet valid.
        /// </summary>
        InvalidRequestId = 3,
    }
    
    /// <summary>
    /// >> 425 - Variant[pallet_cf_threshold_signature.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
