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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_validator.rotation_state
{
    
    
    /// <summary>
    /// >> 260 - Composite[pallet_cf_validator.rotation_state.RotationState]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class RotationState : BaseType
    {
        
        /// <summary>
        /// >> primary_candidates
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> PrimaryCandidates { get; set; }
        /// <summary>
        /// >> secondary_candidates
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> SecondaryCandidates { get; set; }
        /// <summary>
        /// >> banned
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 Banned { get; set; }
        /// <summary>
        /// >> bond
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 Bond { get; set; }
        /// <summary>
        /// >> new_epoch_index
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 NewEpochIndex { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "RotationState";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(PrimaryCandidates.Encode());
            result.AddRange(SecondaryCandidates.Encode());
            result.AddRange(Banned.Encode());
            result.AddRange(Bond.Encode());
            result.AddRange(NewEpochIndex.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            PrimaryCandidates = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>();
            PrimaryCandidates.Decode(byteArray, ref p);
            SecondaryCandidates = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>();
            SecondaryCandidates.Decode(byteArray, ref p);
            Banned = new Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1();
            Banned.Decode(byteArray, ref p);
            Bond = new Substrate.NetApi.Model.Types.Primitive.U128();
            Bond.Decode(byteArray, ref p);
            NewEpochIndex = new Substrate.NetApi.Model.Types.Primitive.U32();
            NewEpochIndex.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
