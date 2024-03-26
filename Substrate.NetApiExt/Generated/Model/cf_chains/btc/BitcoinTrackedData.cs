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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.btc
{
    
    
    /// <summary>
    /// >> 132 - Composite[cf_chains.btc.BitcoinTrackedData]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class BitcoinTrackedData : BaseType
    {
        
        /// <summary>
        /// >> btc_fee_info
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.btc.BitcoinFeeInfo BtcFeeInfo { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "BitcoinTrackedData";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(BtcFeeInfo.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            BtcFeeInfo = new Substrate.NetApiExt.Generated.Model.cf_chains.btc.BitcoinFeeInfo();
            BtcFeeInfo.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
