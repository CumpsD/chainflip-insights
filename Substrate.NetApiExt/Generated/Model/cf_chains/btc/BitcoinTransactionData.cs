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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.btc
{
    
    
    /// <summary>
    /// >> 282 - Composite[cf_chains.btc.BitcoinTransactionData]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class BitcoinTransactionData : BaseType
    {
        
        /// <summary>
        /// >> encoded_transaction
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> EncodedTransaction { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "BitcoinTransactionData";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(EncodedTransaction.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            EncodedTransaction = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>();
            EncodedTransaction.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
