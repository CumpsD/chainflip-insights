//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Substrate.NetApiExt.Generated.Storage
{
    
    
    /// <summary>
    /// >> FlipStorage
    /// </summary>
    public sealed class FlipStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> FlipStorage Constructor
        /// </summary>
        public FlipStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Flip", "Account"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_flip.FlipAccount)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Flip", "Reserve"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApiExt.Generated.Types.Base.Arr4U8), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Flip", "PendingRedemptionsReserve"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Flip", "TotalIssuance"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Flip", "SlashingRate"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Permill)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Flip", "OffchainFunds"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
        }
        
        /// <summary>
        /// >> AccountParams
        ///  Funds belonging to on-chain accounts.
        /// </summary>
        public static string AccountParams(Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("Flip", "Account", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AccountDefault
        /// Default value as hex string
        /// </summary>
        public static string AccountDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> Account
        ///  Funds belonging to on-chain accounts.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_flip.FlipAccount> Account(Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key, string blockhash, CancellationToken token)
        {
            string parameters = FlipStorage.AccountParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_flip.FlipAccount>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ReserveParams
        ///  Funds belonging to on-chain reserves.
        /// </summary>
        public static string ReserveParams(Substrate.NetApiExt.Generated.Types.Base.Arr4U8 key)
        {
            return RequestGenerator.GetStorage("Flip", "Reserve", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> ReserveDefault
        /// Default value as hex string
        /// </summary>
        public static string ReserveDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> Reserve
        ///  Funds belonging to on-chain reserves.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> Reserve(Substrate.NetApiExt.Generated.Types.Base.Arr4U8 key, string blockhash, CancellationToken token)
        {
            string parameters = FlipStorage.ReserveParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingRedemptionsReserveParams
        /// </summary>
        public static string PendingRedemptionsReserveParams(Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("Flip", "PendingRedemptionsReserve", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> PendingRedemptionsReserveDefault
        /// Default value as hex string
        /// </summary>
        public static string PendingRedemptionsReserveDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PendingRedemptionsReserve
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> PendingRedemptionsReserve(Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key, string blockhash, CancellationToken token)
        {
            string parameters = FlipStorage.PendingRedemptionsReserveParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TotalIssuanceParams
        ///  The total number of tokens issued.
        /// </summary>
        public static string TotalIssuanceParams()
        {
            return RequestGenerator.GetStorage("Flip", "TotalIssuance", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> TotalIssuanceDefault
        /// Default value as hex string
        /// </summary>
        public static string TotalIssuanceDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> TotalIssuance
        ///  The total number of tokens issued.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> TotalIssuance(string blockhash, CancellationToken token)
        {
            string parameters = FlipStorage.TotalIssuanceParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> SlashingRateParams
        ///  The per-day slashing rate expressed as a proportion of a validator's bond.
        /// </summary>
        public static string SlashingRateParams()
        {
            return RequestGenerator.GetStorage("Flip", "SlashingRate", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> SlashingRateDefault
        /// Default value as hex string
        /// </summary>
        public static string SlashingRateDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> SlashingRate
        ///  The per-day slashing rate expressed as a proportion of a validator's bond.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Permill> SlashingRate(string blockhash, CancellationToken token)
        {
            string parameters = FlipStorage.SlashingRateParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Permill>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> OffchainFundsParams
        ///  The number of tokens currently off-chain.
        /// </summary>
        public static string OffchainFundsParams()
        {
            return RequestGenerator.GetStorage("Flip", "OffchainFunds", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> OffchainFundsDefault
        /// Default value as hex string
        /// </summary>
        public static string OffchainFundsDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> OffchainFunds
        ///  The number of tokens currently off-chain.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> OffchainFunds(string blockhash, CancellationToken token)
        {
            string parameters = FlipStorage.OffchainFundsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> FlipCalls
    /// </summary>
    public sealed class FlipCalls
    {
        
        /// <summary>
        /// >> set_slashing_rate
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetSlashingRate(Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Permill slashing_rate)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(slashing_rate.Encode());
            return new Method(3, "Flip", 0, "set_slashing_rate", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> FlipConstants
    /// </summary>
    public sealed class FlipConstants
    {
        
        /// <summary>
        /// >> BlocksPerDay
        ///  Blocks per day.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 BlocksPerDay()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x40380000");
            return result;
        }
    }
    
    /// <summary>
    /// >> FlipErrors
    /// </summary>
    public enum FlipErrors
    {
        
        /// <summary>
        /// >> InsufficientLiquidity
        /// Not enough liquid funds.
        /// </summary>
        InsufficientLiquidity,
        
        /// <summary>
        /// >> InsufficientReserves
        /// Not enough reserves.
        /// </summary>
        InsufficientReserves,
        
        /// <summary>
        /// >> NoPendingRedemptionForThisID
        /// No pending redemption for this ID.
        /// </summary>
        NoPendingRedemptionForThisID,
    }
}
