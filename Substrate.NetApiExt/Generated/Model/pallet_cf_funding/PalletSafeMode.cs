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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_funding
{
    
    
    /// <summary>
    /// >> 38 - Composite[pallet_cf_funding.PalletSafeMode]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PalletSafeMode : BaseType
    {
        
        /// <summary>
        /// >> redeem_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool RedeemEnabled { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "PalletSafeMode";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(RedeemEnabled.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            RedeemEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            RedeemEnabled.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
