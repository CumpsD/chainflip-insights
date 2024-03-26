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


namespace Substrate.NetApiExt.Generated.Model.cf_amm.limit_orders
{
    
    
    /// <summary>
    /// >> 507 - Composite[cf_amm.limit_orders.FixedPool]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class FixedPool : BaseType
    {
        
        /// <summary>
        /// >> pool_instance
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 PoolInstance { get; set; }
        /// <summary>
        /// >> available
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 Available { get; set; }
        /// <summary>
        /// >> percent_remaining
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_amm.limit_orders.FloatBetweenZeroAndOne PercentRemaining { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "FixedPool";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(PoolInstance.Encode());
            result.AddRange(Available.Encode());
            result.AddRange(PercentRemaining.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            PoolInstance = new Substrate.NetApi.Model.Types.Primitive.U128();
            PoolInstance.Decode(byteArray, ref p);
            Available = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            Available.Decode(byteArray, ref p);
            PercentRemaining = new Substrate.NetApiExt.Generated.Model.cf_amm.limit_orders.FloatBetweenZeroAndOne();
            PercentRemaining.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
