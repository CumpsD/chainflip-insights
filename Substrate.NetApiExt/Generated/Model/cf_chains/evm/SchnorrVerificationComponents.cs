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
    /// >> 144 - Composite[cf_chains.evm.SchnorrVerificationComponents]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SchnorrVerificationComponents : BaseType
    {
        
        /// <summary>
        /// >> s
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.Arr32U8 S { get; set; }
        /// <summary>
        /// >> k_times_g_address
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.Arr20U8 KTimesGAddress { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SchnorrVerificationComponents";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(S.Encode());
            result.AddRange(KTimesGAddress.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            S = new Substrate.NetApiExt.Generated.Types.Base.Arr32U8();
            S.Decode(byteArray, ref p);
            KTimesGAddress = new Substrate.NetApiExt.Generated.Types.Base.Arr20U8();
            KTimesGAddress.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
