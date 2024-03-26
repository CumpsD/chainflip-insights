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
    /// >> 508 - Composite[cf_amm.limit_orders.FloatBetweenZeroAndOne]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class FloatBetweenZeroAndOne : BaseType
    {
        
        /// <summary>
        /// >> normalised_mantissa
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 NormalisedMantissa { get; set; }
        /// <summary>
        /// >> negative_exponent
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 NegativeExponent { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "FloatBetweenZeroAndOne";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(NormalisedMantissa.Encode());
            result.AddRange(NegativeExponent.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            NormalisedMantissa = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            NormalisedMantissa.Decode(byteArray, ref p);
            NegativeExponent = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            NegativeExponent.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
