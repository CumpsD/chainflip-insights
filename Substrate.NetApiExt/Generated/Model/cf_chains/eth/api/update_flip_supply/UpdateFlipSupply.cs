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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.eth.api.update_flip_supply
{
    
    
    /// <summary>
    /// >> 164 - Composite[cf_chains.eth.api.update_flip_supply.UpdateFlipSupply]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class UpdateFlipSupply : BaseType
    {
        
        /// <summary>
        /// >> new_total_supply
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 NewTotalSupply { get; set; }
        /// <summary>
        /// >> state_chain_block_number
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 StateChainBlockNumber { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "UpdateFlipSupply";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(NewTotalSupply.Encode());
            result.AddRange(StateChainBlockNumber.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            NewTotalSupply = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            NewTotalSupply.Decode(byteArray, ref p);
            StateChainBlockNumber = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            StateChainBlockNumber.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}