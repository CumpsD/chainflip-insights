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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address
{
    
    
    /// <summary>
    /// >> 212 - Composite[cf_chains.btc.deposit_address.DepositAddress]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class DepositAddress : BaseType
    {
        
        /// <summary>
        /// >> pubkey_x
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.Arr32U8 PubkeyX { get; set; }
        /// <summary>
        /// >> script_path
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address.TapscriptPath> ScriptPath { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "DepositAddress";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(PubkeyX.Encode());
            result.AddRange(ScriptPath.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            PubkeyX = new Substrate.NetApiExt.Generated.Types.Base.Arr32U8();
            PubkeyX.Decode(byteArray, ref p);
            ScriptPath = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.cf_chains.btc.deposit_address.TapscriptPath>();
            ScriptPath.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
