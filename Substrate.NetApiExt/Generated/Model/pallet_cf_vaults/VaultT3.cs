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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_vaults
{
    
    
    /// <summary>
    /// >> 395 - Composite[pallet_cf_vaults.VaultT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class VaultT3 : BaseType
    {
        
        /// <summary>
        /// >> public_key
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey PublicKey { get; set; }
        /// <summary>
        /// >> active_from_block
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 ActiveFromBlock { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "VaultT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(PublicKey.Encode());
            result.AddRange(ActiveFromBlock.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            PublicKey = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey();
            PublicKey.Decode(byteArray, ref p);
            ActiveFromBlock = new Substrate.NetApi.Model.Types.Primitive.U64();
            ActiveFromBlock.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
