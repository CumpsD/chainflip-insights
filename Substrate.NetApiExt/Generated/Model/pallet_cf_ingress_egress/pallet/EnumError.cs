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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> InvalidDepositAddress
        /// The deposit address is not valid. It may have expired or may never have been issued.
        /// </summary>
        InvalidDepositAddress = 0,
        
        /// <summary>
        /// >> AssetMismatch
        /// A deposit was made using the wrong asset.
        /// </summary>
        AssetMismatch = 1,
        
        /// <summary>
        /// >> ChannelIdsExhausted
        /// Channel ID has reached maximum
        /// </summary>
        ChannelIdsExhausted = 2,
        
        /// <summary>
        /// >> MissingPolkadotVault
        /// Polkadot's Vault Account does not exist in storage.
        /// </summary>
        MissingPolkadotVault = 3,
        
        /// <summary>
        /// >> MissingBitcoinVault
        /// Bitcoin's Vault key does not exist for the current epoch.
        /// </summary>
        MissingBitcoinVault = 4,
        
        /// <summary>
        /// >> BitcoinChannelIdTooLarge
        /// Channel ID is too large for Bitcoin address derivation
        /// </summary>
        BitcoinChannelIdTooLarge = 5,
        
        /// <summary>
        /// >> BelowEgressDustLimit
        /// The amount is below the minimum egress amount.
        /// </summary>
        BelowEgressDustLimit = 6,
        
        /// <summary>
        /// >> AddBoostFundsDisabled
        /// Adding boost funds is disabled due to safe mode.
        /// </summary>
        AddBoostFundsDisabled = 7,
        
        /// <summary>
        /// >> StopBoostingDisabled
        /// Retrieving boost funds disabled due to safe mode.
        /// </summary>
        StopBoostingDisabled = 8,
        
        /// <summary>
        /// >> BoostPoolAlreadyExists
        /// Cannot create a boost pool if it already exists.
        /// </summary>
        BoostPoolAlreadyExists = 9,
        
        /// <summary>
        /// >> InvalidBoostPoolTier
        /// Cannot create a boost pool of 0 bps
        /// </summary>
        InvalidBoostPoolTier = 10,
        
        /// <summary>
        /// >> DepositChannelCreationDisabled
        /// Disabled due to safe mode for the chain
        /// </summary>
        DepositChannelCreationDisabled = 11,
        
        /// <summary>
        /// >> BoostPoolDoesNotExist
        /// The specified boost pool does not exist.
        /// </summary>
        BoostPoolDoesNotExist = 12,
    }
    
    /// <summary>
    /// >> 681 - Variant[pallet_cf_ingress_egress.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
