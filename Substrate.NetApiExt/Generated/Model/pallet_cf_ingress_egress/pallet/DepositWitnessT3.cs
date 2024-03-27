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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet
{
    
    
    /// <summary>
    /// >> 247 - Composite[pallet_cf_ingress_egress.pallet.DepositWitnessT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class DepositWitnessT3 : BaseType
    {
        
        /// <summary>
        /// >> deposit_address
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey DepositAddress { get; set; }
        /// <summary>
        /// >> asset
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset Asset { get; set; }
        /// <summary>
        /// >> amount
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 Amount { get; set; }
        /// <summary>
        /// >> deposit_details
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.UtxoId DepositDetails { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "DepositWitnessT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(DepositAddress.Encode());
            result.AddRange(Asset.Encode());
            result.AddRange(Amount.Encode());
            result.AddRange(DepositDetails.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            DepositAddress = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey();
            DepositAddress.Decode(byteArray, ref p);
            Asset = new Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset();
            Asset.Decode(byteArray, ref p);
            Amount = new Substrate.NetApi.Model.Types.Primitive.U64();
            Amount.Decode(byteArray, ref p);
            DepositDetails = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.UtxoId();
            DepositDetails.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}