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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.execute_x_swap_and_call
{
    
    
    /// <summary>
    /// >> 178 - Composite[cf_chains.evm.api.execute_x_swap_and_call.ExecutexSwapAndCall]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ExecutexSwapAndCall : BaseType
    {
        
        /// <summary>
        /// >> transfer_param
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.common.EncodableTransferAssetParams TransferParam { get; set; }
        /// <summary>
        /// >> source_chain
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 SourceChain { get; set; }
        /// <summary>
        /// >> source_address
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> SourceAddress { get; set; }
        /// <summary>
        /// >> gas_budget
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 GasBudget { get; set; }
        /// <summary>
        /// >> message
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> Message { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ExecutexSwapAndCall";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(TransferParam.Encode());
            result.AddRange(SourceChain.Encode());
            result.AddRange(SourceAddress.Encode());
            result.AddRange(GasBudget.Encode());
            result.AddRange(Message.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            TransferParam = new Substrate.NetApiExt.Generated.Model.cf_chains.evm.api.common.EncodableTransferAssetParams();
            TransferParam.Decode(byteArray, ref p);
            SourceChain = new Substrate.NetApi.Model.Types.Primitive.U32();
            SourceChain.Decode(byteArray, ref p);
            SourceAddress = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>();
            SourceAddress.Decode(byteArray, ref p);
            GasBudget = new Substrate.NetApi.Model.Types.Primitive.U128();
            GasBudget.Decode(byteArray, ref p);
            Message = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>();
            Message.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
