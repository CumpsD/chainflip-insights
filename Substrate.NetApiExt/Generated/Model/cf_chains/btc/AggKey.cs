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


namespace Substrate.NetApiExt.Generated.Model.cf_chains.btc
{
    
    
    /// <summary>
    /// >> 74 - Composite[cf_chains.btc.AggKey]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class AggKey : BaseType
    {
        
        /// <summary>
        /// >> previous
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Types.Base.Arr32U8> Previous { get; set; }
        /// <summary>
        /// >> current
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.Arr32U8 Current { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "AggKey";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Previous.Encode());
            result.AddRange(Current.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Previous = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Types.Base.Arr32U8>();
            Previous.Decode(byteArray, ref p);
            Current = new Substrate.NetApiExt.Generated.Types.Base.Arr32U8();
            Current.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
