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


namespace Substrate.NetApiExt.Generated.Model.cf_amm.range_orders
{
    
    
    /// <summary>
    /// >> 629 - Composite[cf_amm.range_orders.PoolState]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PoolState : BaseType
    {
        
        /// <summary>
        /// >> fee_hundredth_pips
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 FeeHundredthPips { get; set; }
        /// <summary>
        /// >> current_sqrt_price
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 CurrentSqrtPrice { get; set; }
        /// <summary>
        /// >> current_tick
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.I32 CurrentTick { get; set; }
        /// <summary>
        /// >> current_liquidity
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 CurrentLiquidity { get; set; }
        /// <summary>
        /// >> global_fee_growth
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6 GlobalFeeGrowth { get; set; }
        /// <summary>
        /// >> liquidity_map
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT26 LiquidityMap { get; set; }
        /// <summary>
        /// >> positions
        /// </summary>
        public Substrate.NetApiExt.Generated.Types.Base.BTreeMapT27 Positions { get; set; }
        /// <summary>
        /// >> total_fees_earned
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6 TotalFeesEarned { get; set; }
        /// <summary>
        /// >> total_swap_inputs
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6 TotalSwapInputs { get; set; }
        /// <summary>
        /// >> total_swap_outputs
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6 TotalSwapOutputs { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "PoolState";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(FeeHundredthPips.Encode());
            result.AddRange(CurrentSqrtPrice.Encode());
            result.AddRange(CurrentTick.Encode());
            result.AddRange(CurrentLiquidity.Encode());
            result.AddRange(GlobalFeeGrowth.Encode());
            result.AddRange(LiquidityMap.Encode());
            result.AddRange(Positions.Encode());
            result.AddRange(TotalFeesEarned.Encode());
            result.AddRange(TotalSwapInputs.Encode());
            result.AddRange(TotalSwapOutputs.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            FeeHundredthPips = new Substrate.NetApi.Model.Types.Primitive.U32();
            FeeHundredthPips.Decode(byteArray, ref p);
            CurrentSqrtPrice = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            CurrentSqrtPrice.Decode(byteArray, ref p);
            CurrentTick = new Substrate.NetApi.Model.Types.Primitive.I32();
            CurrentTick.Decode(byteArray, ref p);
            CurrentLiquidity = new Substrate.NetApi.Model.Types.Primitive.U128();
            CurrentLiquidity.Decode(byteArray, ref p);
            GlobalFeeGrowth = new Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6();
            GlobalFeeGrowth.Decode(byteArray, ref p);
            LiquidityMap = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT26();
            LiquidityMap.Decode(byteArray, ref p);
            Positions = new Substrate.NetApiExt.Generated.Types.Base.BTreeMapT27();
            Positions.Decode(byteArray, ref p);
            TotalFeesEarned = new Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6();
            TotalFeesEarned.Decode(byteArray, ref p);
            TotalSwapInputs = new Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6();
            TotalSwapInputs.Decode(byteArray, ref p);
            TotalSwapOutputs = new Substrate.NetApiExt.Generated.Model.cf_amm.common.PoolPairsMapT6();
            TotalSwapOutputs.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
