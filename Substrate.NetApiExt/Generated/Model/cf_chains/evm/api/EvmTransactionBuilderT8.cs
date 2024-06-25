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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm.api
{
    
    
    /// <summary>
    /// >> 196 - Composite[cf_chains.evm.api.EvmTransactionBuilderT8]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class EvmTransactionBuilderT8 : BaseType
    {
        
        /// <summary>
        /// >> sig_data
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.SigData> SigData { get; set; }
        /// <summary>
        /// >> replay_protection
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.EvmReplayProtection ReplayProtection { get; set; }
        /// <summary>
        /// >> call
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.transfer_fallback.TransferFallback Call { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "EvmTransactionBuilderT8";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(SigData.Encode());
            result.AddRange(ReplayProtection.Encode());
            result.AddRange(Call.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            SigData = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.SigData>();
            SigData.Decode(byteArray, ref p);
            ReplayProtection = new Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.EvmReplayProtection();
            ReplayProtection.Decode(byteArray, ref p);
            Call = new Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.transfer_fallback.TransferFallback();
            Call.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
