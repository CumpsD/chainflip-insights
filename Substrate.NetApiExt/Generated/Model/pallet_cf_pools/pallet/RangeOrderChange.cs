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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_pools.pallet
{
    
    
    /// <summary>
    /// >> 300 - Composite[pallet_cf_pools.pallet.RangeOrderChange]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class RangeOrderChange : BaseType
    {
        
        /// <summary>
        /// >> liquidity
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 Liquidity { get; set; }
        /// <summary>
        /// >> amounts
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_pools.AssetsMapT1 Amounts { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "RangeOrderChange";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Liquidity.Encode());
            result.AddRange(Amounts.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Liquidity = new Substrate.NetApi.Model.Types.Primitive.U128();
            Liquidity.Decode(byteArray, ref p);
            Amounts = new Substrate.NetApiExt.Generated.Model.pallet_cf_pools.AssetsMapT1();
            Amounts.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
