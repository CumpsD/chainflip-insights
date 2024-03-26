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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel
{
    
    
    /// <summary>
    /// >> 475 - Composite[cf_chains.deposit_channel.DepositChannelT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class DepositChannelT3 : BaseType
    {
        
        /// <summary>
        /// >> channel_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 ChannelId { get; set; }
        /// <summary>
        /// >> address
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey Address { get; set; }
        /// <summary>
        /// >> asset
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset Asset { get; set; }
        /// <summary>
        /// >> state
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address.DepositAddress State { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "DepositChannelT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(ChannelId.Encode());
            result.AddRange(Address.Encode());
            result.AddRange(Asset.Encode());
            result.AddRange(State.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            ChannelId = new Substrate.NetApi.Model.Types.Primitive.U64();
            ChannelId.Decode(byteArray, ref p);
            Address = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey();
            Address.Decode(byteArray, ref p);
            Asset = new Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset();
            Asset.Decode(byteArray, ref p);
            State = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address.DepositAddress();
            State.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
