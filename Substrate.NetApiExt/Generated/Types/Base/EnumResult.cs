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


namespace Substrate.NetApiExt.Generated.Types.Base
{
    
    
    /// <summary>
    /// >> Result
    /// </summary>
    public enum Result
    {
        
        /// <summary>
        /// >> Ok
        /// </summary>
        Ok = 0,
        
        /// <summary>
        /// >> Err
        /// </summary>
        Err = 1,
    }
    
    /// <summary>
    /// >> 424 - Variant[Result]
    /// </summary>
    public sealed class EnumResult : BaseEnumExt<Result, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Types.Base.Arr64U8>, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>
    {
    }
}