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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress
{
    
    
    /// <summary>
    /// >> 257 - Composite[pallet_cf_ingress_egress.BoostPoolIdT1]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class BoostPoolIdT1 : BaseType
    {
        
        /// <summary>
        /// >> asset
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.eth.EnumAsset Asset { get; set; }
        /// <summary>
        /// >> tier
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 Tier { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "BoostPoolIdT1";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Asset.Encode());
            result.AddRange(Tier.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Asset = new Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.eth.EnumAsset();
            Asset.Decode(byteArray, ref p);
            Tier = new Substrate.NetApi.Model.Types.Primitive.U16();
            Tier.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
