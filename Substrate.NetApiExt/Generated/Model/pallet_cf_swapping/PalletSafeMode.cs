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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_swapping
{
    
    
    /// <summary>
    /// >> 38 - Composite[pallet_cf_swapping.PalletSafeMode]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PalletSafeMode : BaseType
    {
        
        /// <summary>
        /// >> swaps_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool SwapsEnabled { get; set; }
        /// <summary>
        /// >> withdrawals_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool WithdrawalsEnabled { get; set; }
        /// <summary>
        /// >> deposits_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool DepositsEnabled { get; set; }
        /// <summary>
        /// >> broker_registration_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool BrokerRegistrationEnabled { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "PalletSafeMode";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(SwapsEnabled.Encode());
            result.AddRange(WithdrawalsEnabled.Encode());
            result.AddRange(DepositsEnabled.Encode());
            result.AddRange(BrokerRegistrationEnabled.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            SwapsEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            SwapsEnabled.Decode(byteArray, ref p);
            WithdrawalsEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            WithdrawalsEnabled.Decode(byteArray, ref p);
            DepositsEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            DepositsEnabled.Decode(byteArray, ref p);
            BrokerRegistrationEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            BrokerRegistrationEnabled.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
