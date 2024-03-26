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
    /// >> 538 - Composite[cfe_events.KeyHandoverRequest]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class KeyHandoverRequest : BaseType
    {
        
        /// <summary>
        /// >> ceremony_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 CeremonyId { get; set; }
        /// <summary>
        /// >> from_epoch
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 FromEpoch { get; set; }
        /// <summary>
        /// >> to_epoch
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ToEpoch { get; set; }
        /// <summary>
        /// >> key_to_share
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey KeyToShare { get; set; }
        /// <summary>
        /// >> sharing_participants
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 SharingParticipants { get; set; }
        /// <summary>
        /// >> receiving_participants
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 ReceivingParticipants { get; set; }
        /// <summary>
        /// >> new_key
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey NewKey { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "KeyHandoverRequest";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(CeremonyId.Encode());
            result.AddRange(FromEpoch.Encode());
            result.AddRange(ToEpoch.Encode());
            result.AddRange(KeyToShare.Encode());
            result.AddRange(SharingParticipants.Encode());
            result.AddRange(ReceivingParticipants.Encode());
            result.AddRange(NewKey.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            CeremonyId = new Substrate.NetApi.Model.Types.Primitive.U64();
            CeremonyId.Decode(byteArray, ref p);
            FromEpoch = new Substrate.NetApi.Model.Types.Primitive.U32();
            FromEpoch.Decode(byteArray, ref p);
            ToEpoch = new Substrate.NetApi.Model.Types.Primitive.U32();
            ToEpoch.Decode(byteArray, ref p);
            KeyToShare = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey();
            KeyToShare.Decode(byteArray, ref p);
            SharingParticipants = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            SharingParticipants.Decode(byteArray, ref p);
            ReceivingParticipants = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            ReceivingParticipants.Decode(byteArray, ref p);
            NewKey = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey();
            NewKey.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
