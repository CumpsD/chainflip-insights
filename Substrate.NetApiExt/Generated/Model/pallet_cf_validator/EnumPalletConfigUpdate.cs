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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_validator
{
    
    
    /// <summary>
    /// >> PalletConfigUpdate
    /// </summary>
    public enum PalletConfigUpdate
    {
        
        /// <summary>
        /// >> RegistrationBondPercentage
        /// </summary>
        RegistrationBondPercentage = 0,
        
        /// <summary>
        /// >> AuctionBidCutoffPercentage
        /// </summary>
        AuctionBidCutoffPercentage = 1,
        
        /// <summary>
        /// >> RedemptionPeriodAsPercentage
        /// </summary>
        RedemptionPeriodAsPercentage = 2,
        
        /// <summary>
        /// >> BackupRewardNodePercentage
        /// </summary>
        BackupRewardNodePercentage = 3,
        
        /// <summary>
        /// >> EpochDuration
        /// </summary>
        EpochDuration = 4,
        
        /// <summary>
        /// >> AuthoritySetMinSize
        /// </summary>
        AuthoritySetMinSize = 5,
        
        /// <summary>
        /// >> AuctionParameters
        /// </summary>
        AuctionParameters = 6,
        
        /// <summary>
        /// >> MinimumReportedCfeVersion
        /// </summary>
        MinimumReportedCfeVersion = 7,
        
        /// <summary>
        /// >> MaxAuthoritySetContractionPercentage
        /// </summary>
        MaxAuthoritySetContractionPercentage = 8,
    }
    
    /// <summary>
    /// >> 85 - Variant[pallet_cf_validator.PalletConfigUpdate]
    /// </summary>
    public sealed class EnumPalletConfigUpdate : BaseEnumExt<PalletConfigUpdate, Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Percent, Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Percent, Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Percent, Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Percent, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.pallet_cf_validator.auction_resolver.SetSizeParameters, Substrate.NetApiExt.Generated.Model.cf_primitives.SemVer, Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Percent>
    {
    }
}