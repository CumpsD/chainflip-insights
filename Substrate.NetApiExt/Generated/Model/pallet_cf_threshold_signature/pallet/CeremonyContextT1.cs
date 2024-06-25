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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet
{
    
    
    /// <summary>
    /// >> 447 - Composite[pallet_cf_threshold_signature.pallet.CeremonyContextT1]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class CeremonyContextT1 : BaseType
    {
        
        /// <summary>
        /// >> request_context
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestContextT1 RequestContext { get; set; }
        /// <summary>
        /// >> remaining_respondents
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 RemainingRespondents { get; set; }
        /// <summary>
        /// >> blame_counts
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT6 BlameCounts { get; set; }
        /// <summary>
        /// >> candidates
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 Candidates { get; set; }
        /// <summary>
        /// >> epoch
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 Epoch { get; set; }
        /// <summary>
        /// >> key
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.evm.AggKey Key { get; set; }
        /// <summary>
        /// >> threshold_ceremony_type
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.EnumThresholdCeremonyType ThresholdCeremonyType { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "CeremonyContextT1";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(RequestContext.Encode());
            result.AddRange(RemainingRespondents.Encode());
            result.AddRange(BlameCounts.Encode());
            result.AddRange(Candidates.Encode());
            result.AddRange(Epoch.Encode());
            result.AddRange(Key.Encode());
            result.AddRange(ThresholdCeremonyType.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            RequestContext = new Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestContextT1();
            RequestContext.Decode(byteArray, ref p);
            RemainingRespondents = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            RemainingRespondents.Decode(byteArray, ref p);
            BlameCounts = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT6();
            BlameCounts.Decode(byteArray, ref p);
            Candidates = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            Candidates.Decode(byteArray, ref p);
            Epoch = new Substrate.NetApi.Model.Types.Primitive.U32();
            Epoch.Decode(byteArray, ref p);
            Key = new Substrate.NetApiExt.Generated.Model.cf_chains.evm.AggKey();
            Key.Decode(byteArray, ref p);
            ThresholdCeremonyType = new Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.EnumThresholdCeremonyType();
            ThresholdCeremonyType.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
