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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.btc.utxo_selection
{
    
    
    /// <summary>
    /// >> 57 - Composite[cf_chains.btc.utxo_selection.ConsolidationParameters]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ConsolidationParameters : BaseType
    {
        
        /// <summary>
        /// >> consolidation_threshold
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ConsolidationThreshold { get; set; }
        /// <summary>
        /// >> consolidation_size
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ConsolidationSize { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ConsolidationParameters";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(ConsolidationThreshold.Encode());
            result.AddRange(ConsolidationSize.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            ConsolidationThreshold = new Substrate.NetApi.Model.Types.Primitive.U32();
            ConsolidationThreshold.Decode(byteArray, ref p);
            ConsolidationSize = new Substrate.NetApi.Model.Types.Primitive.U32();
            ConsolidationSize.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}