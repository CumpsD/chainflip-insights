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


namespace Substrate.NetApiExt.Generated.Model.cf_primitives
{
    
    
    /// <summary>
    /// >> 88 - Composite[cf_primitives.SemVer]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SemVer : BaseType
    {
        
        /// <summary>
        /// >> major
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 Major { get; set; }
        /// <summary>
        /// >> minor
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 Minor { get; set; }
        /// <summary>
        /// >> patch
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 Patch { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SemVer";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Major.Encode());
            result.AddRange(Minor.Encode());
            result.AddRange(Patch.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Major = new Substrate.NetApi.Model.Types.Primitive.U8();
            Major.Decode(byteArray, ref p);
            Minor = new Substrate.NetApi.Model.Types.Primitive.U8();
            Minor.Decode(byteArray, ref p);
            Patch = new Substrate.NetApi.Model.Types.Primitive.U8();
            Patch.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
