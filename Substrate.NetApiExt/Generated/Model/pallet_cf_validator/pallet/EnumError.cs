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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_validator.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> InvalidEpochDuration
        /// Epoch duration supplied is invalid.
        /// </summary>
        InvalidEpochDuration = 0,
        
        /// <summary>
        /// >> RotationInProgress
        /// A rotation is in progress.
        /// </summary>
        RotationInProgress = 1,
        
        /// <summary>
        /// >> AccountPeerMappingOverlap
        /// Validator Peer mapping overlaps with an existing mapping.
        /// </summary>
        AccountPeerMappingOverlap = 2,
        
        /// <summary>
        /// >> InvalidAccountPeerMappingSignature
        /// Invalid signature.
        /// </summary>
        InvalidAccountPeerMappingSignature = 3,
        
        /// <summary>
        /// >> InvalidRedemptionPeriod
        /// Invalid redemption period.
        /// </summary>
        InvalidRedemptionPeriod = 4,
        
        /// <summary>
        /// >> NameTooLong
        /// Vanity name length exceeds the limit of 64 characters.
        /// </summary>
        NameTooLong = 5,
        
        /// <summary>
        /// >> InvalidCharactersInName
        /// Invalid characters in the name.
        /// </summary>
        InvalidCharactersInName = 6,
        
        /// <summary>
        /// >> InvalidAuthoritySetMinSize
        /// Invalid minimum authority set size.
        /// </summary>
        InvalidAuthoritySetMinSize = 7,
        
        /// <summary>
        /// >> InvalidAuctionParameters
        /// Auction parameters are invalid.
        /// </summary>
        InvalidAuctionParameters = 8,
        
        /// <summary>
        /// >> InconsistentRanges
        /// The dynamic set size ranges are inconsistent.
        /// </summary>
        InconsistentRanges = 9,
        
        /// <summary>
        /// >> NotEnoughBidders
        /// Not enough bidders were available to resolve the auction.
        /// </summary>
        NotEnoughBidders = 10,
        
        /// <summary>
        /// >> NotEnoughFunds
        /// Not enough funds to register as a validator.
        /// </summary>
        NotEnoughFunds = 11,
        
        /// <summary>
        /// >> RotationsDisabled
        /// Rotations are currently disabled through SafeMode.
        /// </summary>
        RotationsDisabled = 12,
    }
    
    /// <summary>
    /// >> 344 - Variant[pallet_cf_validator.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
