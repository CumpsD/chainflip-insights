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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_swapping
{
    
    
    /// <summary>
    /// >> 507 - Composite[pallet_cf_swapping.CcmSwapOutput]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class CcmSwapOutput : BaseType
    {
        
        /// <summary>
        /// >> principal
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> Principal { get; set; }
        /// <summary>
        /// >> gas
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> Gas { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "CcmSwapOutput";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Principal.Encode());
            result.AddRange(Gas.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Principal = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128>();
            Principal.Decode(byteArray, ref p);
            Gas = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128>();
            Gas.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
