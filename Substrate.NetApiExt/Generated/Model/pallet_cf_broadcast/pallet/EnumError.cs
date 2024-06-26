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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> InvalidPayload
        /// The provided payload is invalid.
        /// </summary>
        InvalidPayload = 0,
        
        /// <summary>
        /// >> InvalidBroadcastId
        /// The provided broadcast id is invalid.
        /// </summary>
        InvalidBroadcastId = 1,
        
        /// <summary>
        /// >> ThresholdSignatureUnavailable
        /// A threshold signature was expected but not available.
        /// </summary>
        ThresholdSignatureUnavailable = 2,
    }
    
    /// <summary>
    /// >> 659 - Variant[pallet_cf_broadcast.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
