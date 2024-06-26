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
    /// >> 647 - Composite[cfe_events.ThresholdSignatureRequestT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ThresholdSignatureRequestT3 : BaseType
    {
        
        /// <summary>
        /// >> ceremony_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 CeremonyId { get; set; }
        /// <summary>
        /// >> epoch_index
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 EpochIndex { get; set; }
        /// <summary>
        /// >> key
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey Key { get; set; }
        /// <summary>
        /// >> signatories
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 Signatories { get; set; }
        /// <summary>
        /// >> payload
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumPreviousOrCurrent, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>> Payload { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ThresholdSignatureRequestT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(CeremonyId.Encode());
            result.AddRange(EpochIndex.Encode());
            result.AddRange(Key.Encode());
            result.AddRange(Signatories.Encode());
            result.AddRange(Payload.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            CeremonyId = new Substrate.NetApi.Model.Types.Primitive.U64();
            CeremonyId.Decode(byteArray, ref p);
            EpochIndex = new Substrate.NetApi.Model.Types.Primitive.U32();
            EpochIndex.Decode(byteArray, ref p);
            Key = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey();
            Key.Decode(byteArray, ref p);
            Signatories = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            Signatories.Decode(byteArray, ref p);
            Payload = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumPreviousOrCurrent, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>>();
            Payload.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
