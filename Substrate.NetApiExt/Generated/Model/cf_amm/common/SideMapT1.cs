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


namespace Substrate.NetApiExt.Generated.Model.cf_amm.common
{
    
    
    /// <summary>
    /// >> 505 - Composite[cf_amm.common.SideMapT1]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SideMapT1 : BaseType
    {
        
        /// <summary>
        /// >> zero
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT12 Zero { get; set; }
        /// <summary>
        /// >> one
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT12 One { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SideMapT1";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Zero.Encode());
            result.AddRange(One.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Zero = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT12();
            Zero.Decode(byteArray, ref p);
            One = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT12();
            One.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
