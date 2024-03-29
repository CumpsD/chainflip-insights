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
    /// >> 156 - Composite[cf_chains.evm.api.SigData]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SigData : BaseType
    {
        
        /// <summary>
        /// >> sig
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 Sig { get; set; }
        /// <summary>
        /// >> nonce
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 Nonce { get; set; }
        /// <summary>
        /// >> k_times_g_address
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.H160 KTimesGAddress { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SigData";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Sig.Encode());
            result.AddRange(Nonce.Encode());
            result.AddRange(KTimesGAddress.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Sig = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            Sig.Decode(byteArray, ref p);
            Nonce = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            Nonce.Decode(byteArray, ref p);
            KTimesGAddress = new Substrate.NetApiExt.Generated.Model.primitive_types.H160();
            KTimesGAddress.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
