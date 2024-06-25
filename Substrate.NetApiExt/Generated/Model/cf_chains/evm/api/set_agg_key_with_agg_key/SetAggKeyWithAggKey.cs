//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.set_agg_key_with_agg_key
{
    
    
    /// <summary>
    /// >> 171 - Composite[cf_chains.evm.api.set_agg_key_with_agg_key.SetAggKeyWithAggKey]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SetAggKeyWithAggKey : BaseType
    {
        
        /// <summary>
        /// >> new_key
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.evm.AggKey NewKey { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SetAggKeyWithAggKey";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(NewKey.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            NewKey = new Substrate.NetApiExt.Generated.Model.cf_chains.evm.AggKey();
            NewKey.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
