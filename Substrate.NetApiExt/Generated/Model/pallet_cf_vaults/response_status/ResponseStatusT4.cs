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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.response_status
{
    
    
    /// <summary>
    /// >> 393 - Composite[pallet_cf_vaults.response_status.ResponseStatusT4]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ResponseStatusT4 : BaseType
    {
        
        /// <summary>
        /// >> candidates
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 Candidates { get; set; }
        /// <summary>
        /// >> remaining_candidates
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 RemainingCandidates { get; set; }
        /// <summary>
        /// >> success_votes
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT6 SuccessVotes { get; set; }
        /// <summary>
        /// >> blame_votes
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT5 BlameVotes { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ResponseStatusT4";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Candidates.Encode());
            result.AddRange(RemainingCandidates.Encode());
            result.AddRange(SuccessVotes.Encode());
            result.AddRange(BlameVotes.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Candidates = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            Candidates.Decode(byteArray, ref p);
            RemainingCandidates = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            RemainingCandidates.Decode(byteArray, ref p);
            SuccessVotes = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT6();
            SuccessVotes.Decode(byteArray, ref p);
            BlameVotes = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT5();
            BlameVotes.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}