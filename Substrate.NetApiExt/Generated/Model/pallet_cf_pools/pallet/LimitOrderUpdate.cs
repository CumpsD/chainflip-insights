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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_pools.pallet
{
    
    
    /// <summary>
    /// >> 530 - Composite[pallet_cf_pools.pallet.LimitOrderUpdate]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class LimitOrderUpdate : BaseType
    {
        
        /// <summary>
        /// >> lp
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 Lp { get; set; }
        /// <summary>
        /// >> id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 Id { get; set; }
        /// <summary>
        /// >> call
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_pools.pallet.EnumCall Call { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "LimitOrderUpdate";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Lp.Encode());
            result.AddRange(Id.Encode());
            result.AddRange(Call.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Lp = new Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
            Lp.Decode(byteArray, ref p);
            Id = new Substrate.NetApi.Model.Types.Primitive.U64();
            Id.Decode(byteArray, ref p);
            Call = new Substrate.NetApiExt.Generated.Model.pallet_cf_pools.pallet.EnumCall();
            Call.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
