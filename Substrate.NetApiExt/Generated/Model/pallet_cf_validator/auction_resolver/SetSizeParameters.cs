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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_validator.auction_resolver
{
    
    
    /// <summary>
    /// >> 103 - Composite[pallet_cf_validator.auction_resolver.SetSizeParameters]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SetSizeParameters : BaseType
    {
        
        /// <summary>
        /// >> min_size
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MinSize { get; set; }
        /// <summary>
        /// >> max_size
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxSize { get; set; }
        /// <summary>
        /// >> max_expansion
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxExpansion { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SetSizeParameters";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(MinSize.Encode());
            result.AddRange(MaxSize.Encode());
            result.AddRange(MaxExpansion.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            MinSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            MinSize.Decode(byteArray, ref p);
            MaxSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxSize.Decode(byteArray, ref p);
            MaxExpansion = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxExpansion.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
