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
    /// >> 583 - Composite[pallet_cf_ingress_egress.boost_pool.OwedAmountT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class OwedAmountT3 : BaseType
    {
        
        /// <summary>
        /// >> total
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool.ScaledAmountT3 Total { get; set; }
        /// <summary>
        /// >> fee
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool.ScaledAmountT3 Fee { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "OwedAmountT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Total.Encode());
            result.AddRange(Fee.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Total = new Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool.ScaledAmountT3();
            Total.Decode(byteArray, ref p);
            Fee = new Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool.ScaledAmountT3();
            Fee.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}