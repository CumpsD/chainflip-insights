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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_pools
{
    
    
    /// <summary>
    /// >> 487 - Composite[pallet_cf_pools.AssetsMapT2]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class AssetsMapT2 : BaseType
    {
        
        /// <summary>
        /// >> base
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.any.EnumAsset Base { get; set; }
        /// <summary>
        /// >> quote
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.any.EnumAsset Quote { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "AssetsMapT2";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Base.Encode());
            result.AddRange(Quote.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Base = new Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.any.EnumAsset();
            Base.Decode(byteArray, ref p);
            Quote = new Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.any.EnumAsset();
            Quote.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
