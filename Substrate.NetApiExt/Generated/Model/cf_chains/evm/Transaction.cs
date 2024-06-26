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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.evm
{
    
    
    /// <summary>
    /// >> 328 - Composite[cf_chains.evm.Transaction]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class Transaction : BaseType
    {
        
        /// <summary>
        /// >> chain_id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 ChainId { get; set; }
        /// <summary>
        /// >> max_priority_fee_per_gas
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> MaxPriorityFeePerGas { get; set; }
        /// <summary>
        /// >> max_fee_per_gas
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> MaxFeePerGas { get; set; }
        /// <summary>
        /// >> gas_limit
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> GasLimit { get; set; }
        /// <summary>
        /// >> contract
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.H160 Contract { get; set; }
        /// <summary>
        /// >> value
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 Value { get; set; }
        /// <summary>
        /// >> data
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> Data { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "Transaction";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(ChainId.Encode());
            result.AddRange(MaxPriorityFeePerGas.Encode());
            result.AddRange(MaxFeePerGas.Encode());
            result.AddRange(GasLimit.Encode());
            result.AddRange(Contract.Encode());
            result.AddRange(Value.Encode());
            result.AddRange(Data.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            ChainId = new Substrate.NetApi.Model.Types.Primitive.U64();
            ChainId.Decode(byteArray, ref p);
            MaxPriorityFeePerGas = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256>();
            MaxPriorityFeePerGas.Decode(byteArray, ref p);
            MaxFeePerGas = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256>();
            MaxFeePerGas.Decode(byteArray, ref p);
            GasLimit = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256>();
            GasLimit.Decode(byteArray, ref p);
            Contract = new Substrate.NetApiExt.Generated.Model.primitive_types.H160();
            Contract.Decode(byteArray, ref p);
            Value = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            Value.Decode(byteArray, ref p);
            Data = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>();
            Data.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
