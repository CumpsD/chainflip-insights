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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.common
{
    
    
    /// <summary>
    /// >> 189 - Composite[cf_chains.evm.api.common.EncodableFetchDeployAssetParams]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class EncodableFetchDeployAssetParams : BaseType
    {
        
        /// <summary>
        /// >> channel_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 ChannelId { get; set; }
        /// <summary>
        /// >> asset
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.H160 Asset { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "EncodableFetchDeployAssetParams";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(ChannelId.Encode());
            result.AddRange(Asset.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            ChannelId = new Substrate.NetApi.Model.Types.Primitive.U64();
            ChannelId.Decode(byteArray, ref p);
            Asset = new Substrate.NetApiExt.Generated.Model.primitive_types.H160();
            Asset.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
