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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_witnesser
{
    
    
    /// <summary>
    /// >> PalletSafeMode
    /// </summary>
    public enum PalletSafeMode
    {
        
        /// <summary>
        /// >> CodeGreen
        /// </summary>
        CodeGreen = 0,
        
        /// <summary>
        /// >> CodeRed
        /// </summary>
        CodeRed = 1,
        
        /// <summary>
        /// >> CodeAmber
        /// </summary>
        CodeAmber = 2,
    }
    
    /// <summary>
    /// >> 49 - Variant[pallet_cf_witnesser.PalletSafeMode]
    /// </summary>
    public sealed class EnumPalletSafeMode : BaseEnumExt<PalletSafeMode, BaseVoid, BaseVoid, Substrate.NetApiExt.Generated.Model.state_chain_runtime.safe_mode.WitnesserCallPermission>
    {
    }
}
