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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_lp.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> InsufficientBalance
        /// The user does not have enough funds.
        /// </summary>
        InsufficientBalance = 0,
        
        /// <summary>
        /// >> BalanceOverflow
        /// The user has reached the maximum balance.
        /// </summary>
        BalanceOverflow = 1,
        
        /// <summary>
        /// >> UnauthorisedToModify
        /// The caller is not authorized to modify the trading position.
        /// </summary>
        UnauthorisedToModify = 2,
        
        /// <summary>
        /// >> InvalidEgressAddress
        /// The Asset cannot be egressed because the destination address is not invalid.
        /// </summary>
        InvalidEgressAddress = 3,
        
        /// <summary>
        /// >> InvalidEncodedAddress
        /// Then given encoded address cannot be decoded into a valid ForeignChainAddress.
        /// </summary>
        InvalidEncodedAddress = 4,
        
        /// <summary>
        /// >> NoLiquidityRefundAddressRegistered
        /// A liquidity refund address must be set by the user for the chain before a
        /// deposit address can be requested.
        /// </summary>
        NoLiquidityRefundAddressRegistered = 5,
        
        /// <summary>
        /// >> LiquidityDepositDisabled
        /// Liquidity deposit is disabled due to Safe Mode.
        /// </summary>
        LiquidityDepositDisabled = 6,
        
        /// <summary>
        /// >> WithdrawalsDisabled
        /// Withdrawals are disabled due to Safe Mode.
        /// </summary>
        WithdrawalsDisabled = 7,
    }
    
    /// <summary>
    /// >> 446 - Variant[pallet_cf_lp.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
