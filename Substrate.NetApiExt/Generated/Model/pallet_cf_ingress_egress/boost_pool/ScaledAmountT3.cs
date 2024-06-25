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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool
{
    
    
    /// <summary>
    /// >> 577 - Composite[pallet_cf_ingress_egress.boost_pool.ScaledAmountT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ScaledAmountT3 : BaseType
    {
        
        /// <summary>
        /// >> val
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 Val { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ScaledAmountT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Val.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Val = new Substrate.NetApi.Model.Types.Primitive.U128();
            Val.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
