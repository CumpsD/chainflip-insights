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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet
{
    
    
    /// <summary>
    /// >> 414 - Composite[pallet_cf_threshold_signature.pallet.RequestInstructionT2]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class RequestInstructionT2 : BaseType
    {
        
        /// <summary>
        /// >> request_context
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestContextT2 RequestContext { get; set; }
        /// <summary>
        /// >> request_type
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.EnumRequestType RequestType { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "RequestInstructionT2";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(RequestContext.Encode());
            result.AddRange(RequestType.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            RequestContext = new Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestContextT2();
            RequestContext.Decode(byteArray, ref p);
            RequestType = new Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.EnumRequestType();
            RequestType.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
