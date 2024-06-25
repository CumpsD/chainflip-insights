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


namespace Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool
{
    
    
    /// <summary>
    /// >> 576 - Composite[pallet_cf_ingress_egress.boost_pool.BoostPoolT3]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class BoostPoolT3 : BaseType
    {
        
        /// <summary>
        /// >> fee_bps
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 FeeBps { get; set; }
        /// <summary>
        /// >> available_amount
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool.ScaledAmountT3 AvailableAmount { get; set; }
        /// <summary>
        /// >> amounts
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT17 Amounts { get; set; }
        /// <summary>
        /// >> pending_boosts
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT18 PendingBoosts { get; set; }
        /// <summary>
        /// >> pending_withdrawals
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT13 PendingWithdrawals { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "BoostPoolT3";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(FeeBps.Encode());
            result.AddRange(AvailableAmount.Encode());
            result.AddRange(Amounts.Encode());
            result.AddRange(PendingBoosts.Encode());
            result.AddRange(PendingWithdrawals.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            FeeBps = new Substrate.NetApi.Model.Types.Primitive.U16();
            FeeBps.Decode(byteArray, ref p);
            AvailableAmount = new Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.boost_pool.ScaledAmountT3();
            AvailableAmount.Decode(byteArray, ref p);
            Amounts = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT17();
            Amounts.Decode(byteArray, ref p);
            PendingBoosts = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT18();
            PendingBoosts.Decode(byteArray, ref p);
            PendingWithdrawals = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT13();
            PendingWithdrawals.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
