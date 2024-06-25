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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm
{
    
    
    /// <summary>
    /// >> 198 - Composite[cf_chains.evm.TransactionFee]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class TransactionFee : BaseType
    {
        
        /// <summary>
        /// >> effective_gas_price
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 EffectiveGasPrice { get; set; }
        /// <summary>
        /// >> gas_used
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 GasUsed { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "TransactionFee";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(EffectiveGasPrice.Encode());
            result.AddRange(GasUsed.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            EffectiveGasPrice = new Substrate.NetApi.Model.Types.Primitive.U128();
            EffectiveGasPrice.Decode(byteArray, ref p);
            GasUsed = new Substrate.NetApi.Model.Types.Primitive.U128();
            GasUsed.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
