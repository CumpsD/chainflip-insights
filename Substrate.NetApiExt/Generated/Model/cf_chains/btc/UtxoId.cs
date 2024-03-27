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
    /// >> 211 - Composite[cf_chains.btc.UtxoId]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class UtxoId : BaseType
    {
        
        /// <summary>
        /// >> tx_id
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.Arr32U8 TxId { get; set; }
        /// <summary>
        /// >> vout
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 Vout { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "UtxoId";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(TxId.Encode());
            result.AddRange(Vout.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            TxId = new Substrate.NetApiExt.Generated.Types.Base.Arr32U8();
            TxId.Decode(byteArray, ref p);
            Vout = new Substrate.NetApi.Model.Types.Primitive.U32();
            Vout.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}