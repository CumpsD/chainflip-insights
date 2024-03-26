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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.transfer_fallback
{
    
    
    /// <summary>
    /// >> 180 - Composite[cf_chains.evm.api.transfer_fallback.TransferFallback]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class TransferFallback : BaseType
    {
        
        /// <summary>
        /// >> transfer_param
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.common.EncodableTransferAssetParams TransferParam { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "TransferFallback";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(TransferParam.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            TransferParam = new Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.common.EncodableTransferAssetParams();
            TransferParam.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
