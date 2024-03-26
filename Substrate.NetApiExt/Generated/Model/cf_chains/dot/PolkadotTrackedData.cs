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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.dot
{
    
    
    /// <summary>
    /// >> 127 - Composite[cf_chains.dot.PolkadotTrackedData]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PolkadotTrackedData : BaseType
    {
        
        /// <summary>
        /// >> median_tip
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 MedianTip { get; set; }
        /// <summary>
        /// >> runtime_version
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.dot.RuntimeVersion RuntimeVersion { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "PolkadotTrackedData";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(MedianTip.Encode());
            result.AddRange(RuntimeVersion.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            MedianTip = new Substrate.NetApi.Model.Types.Primitive.U128();
            MedianTip.Decode(byteArray, ref p);
            RuntimeVersion = new Substrate.NetApiExt.Generated.Model.cf_chains.dot.RuntimeVersion();
            RuntimeVersion.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
