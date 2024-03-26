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


namespace Substrate.NetApiExt.Generated.Model.cfe_events
{
    
    
    /// <summary>
    /// >> 540 - Composite[cfe_events.TxBroadcastRequestT2]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class TxBroadcastRequestT2 : BaseType
    {
        
        /// <summary>
        /// >> broadcast_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 BroadcastId { get; set; }
        /// <summary>
        /// >> nominee
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 Nominee { get; set; }
        /// <summary>
        /// >> payload
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotTransactionData Payload { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "TxBroadcastRequestT2";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(BroadcastId.Encode());
            result.AddRange(Nominee.Encode());
            result.AddRange(Payload.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            BroadcastId = new Substrate.NetApi.Model.Types.Primitive.U32();
            BroadcastId.Decode(byteArray, ref p);
            Nominee = new Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
            Nominee.Decode(byteArray, ref p);
            Payload = new Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotTransactionData();
            Payload.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
