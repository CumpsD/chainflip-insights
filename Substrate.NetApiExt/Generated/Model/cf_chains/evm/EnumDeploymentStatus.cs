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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm
{
    
    
    /// <summary>
    /// >> DeploymentStatus
    /// </summary>
    public enum DeploymentStatus
    {
        
        /// <summary>
        /// >> Undeployed
        /// </summary>
        Undeployed = 0,
        
        /// <summary>
        /// >> Pending
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// >> Deployed
        /// </summary>
        Deployed = 2,
    }
    
    /// <summary>
    /// >> 449 - Variant[cf_chains.evm.DeploymentStatus]
    /// </summary>
    public sealed class EnumDeploymentStatus : BaseEnum<DeploymentStatus>
    {
    }
}
