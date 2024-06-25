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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress
{
    
    
    /// <summary>
    /// >> 54 - Composite[pallet_cf_ingress_egress.PalletSafeModeT2]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PalletSafeModeT2 : BaseType
    {
        
        /// <summary>
        /// >> boost_deposits_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool BoostDepositsEnabled { get; set; }
        /// <summary>
        /// >> add_boost_funds_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool AddBoostFundsEnabled { get; set; }
        /// <summary>
        /// >> stop_boosting_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool StopBoostingEnabled { get; set; }
        /// <summary>
        /// >> deposits_enabled
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool DepositsEnabled { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "PalletSafeModeT2";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(BoostDepositsEnabled.Encode());
            result.AddRange(AddBoostFundsEnabled.Encode());
            result.AddRange(StopBoostingEnabled.Encode());
            result.AddRange(DepositsEnabled.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            BoostDepositsEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            BoostDepositsEnabled.Decode(byteArray, ref p);
            AddBoostFundsEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            AddBoostFundsEnabled.Decode(byteArray, ref p);
            StopBoostingEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            StopBoostingEnabled.Decode(byteArray, ref p);
            DepositsEnabled = new Substrate.NetApi.Model.Types.Primitive.Bool();
            DepositsEnabled.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
