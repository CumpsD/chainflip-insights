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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.address
{
    
    
    /// <summary>
    /// >> ForeignChainAddress
    /// </summary>
    public enum ForeignChainAddress
    {
        
        /// <summary>
        /// >> Eth
        /// </summary>
        Eth = 0,
        
        /// <summary>
        /// >> Dot
        /// </summary>
        Dot = 1,
        
        /// <summary>
        /// >> Btc
        /// </summary>
        Btc = 2,
        
        /// <summary>
        /// >> Arb
        /// </summary>
        Arb = 3,
    }
    
    /// <summary>
    /// >> 241 - Variant[cf_chains.address.ForeignChainAddress]
    /// </summary>
    public sealed class EnumForeignChainAddress : BaseEnumExt<ForeignChainAddress, Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey, Substrate.NetApiExt.Generated.Model.primitive_types.H160>
    {
    }
}
