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
    /// >> 210 - Composite[cf_chains.btc.Utxo]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class Utxo : BaseType
    {
        
        /// <summary>
        /// >> id
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.UtxoId Id { get; set; }
        /// <summary>
        /// >> amount
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 Amount { get; set; }
        /// <summary>
        /// >> deposit_address
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address.DepositAddress DepositAddress { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "Utxo";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Id.Encode());
            result.AddRange(Amount.Encode());
            result.AddRange(DepositAddress.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Id = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.UtxoId();
            Id.Decode(byteArray, ref p);
            Amount = new Substrate.NetApi.Model.Types.Primitive.U64();
            Amount.Decode(byteArray, ref p);
            DepositAddress = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address.DepositAddress();
            DepositAddress.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}