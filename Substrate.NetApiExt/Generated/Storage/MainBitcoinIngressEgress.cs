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
    /// >> BitcoinIngressEgressStorage
    /// </summary>
    public sealed class BitcoinIngressEgressStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> BitcoinIngressEgressStorage Constructor
        /// </summary>
        public BitcoinIngressEgressStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "DepositChannelLookup"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositChannelDetailsT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "ChannelIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "EgressIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "ScheduledEgressFetchOrTransfer"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.EnumFetchOrTransfer>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "ScheduledEgressCcm"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.CrossChainMessageT3>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "DisabledEgressAssets"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "DepositChannelPool"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U64), typeof(Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel.DepositChannelT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "MinimumDeposit"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset), typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "EgressDustLimit"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "DepositChannelLifetime"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "FailedForeignChainCalls"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.FailedForeignChainCall>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "DepositBalances"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositTrackerT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "DepositChannelRecycleBlocks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey>>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "WitnessSafetyMargin"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinIngressEgress", "WithheldTransactionFees"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset), typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
        }
        
        /// <summary>
        /// >> DepositChannelLookupParams
        ///  Lookup table for addresses to corresponding deposit channels.
        /// </summary>
        public static string DepositChannelLookupParams(Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "DepositChannelLookup", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> DepositChannelLookupDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositChannelLookupDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> DepositChannelLookup
        ///  Lookup table for addresses to corresponding deposit channels.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositChannelDetailsT3> DepositChannelLookup(Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.DepositChannelLookupParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositChannelDetailsT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ChannelIdCounterParams
        ///  Stores the latest channel id used to generate an address.
        /// </summary>
        public static string ChannelIdCounterParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "ChannelIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ChannelIdCounterDefault
        /// Default value as hex string
        /// </summary>
        public static string ChannelIdCounterDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> ChannelIdCounter
        ///  Stores the latest channel id used to generate an address.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> ChannelIdCounter(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.ChannelIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> EgressIdCounterParams
        ///  Stores the latest egress id used to generate an address.
        /// </summary>
        public static string EgressIdCounterParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "EgressIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> EgressIdCounterDefault
        /// Default value as hex string
        /// </summary>
        public static string EgressIdCounterDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> EgressIdCounter
        ///  Stores the latest egress id used to generate an address.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> EgressIdCounter(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.EgressIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ScheduledEgressFetchOrTransferParams
        ///  Scheduled fetch and egress for the Ethereum chain.
        /// </summary>
        public static string ScheduledEgressFetchOrTransferParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "ScheduledEgressFetchOrTransfer", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ScheduledEgressFetchOrTransferDefault
        /// Default value as hex string
        /// </summary>
        public static string ScheduledEgressFetchOrTransferDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> ScheduledEgressFetchOrTransfer
        ///  Scheduled fetch and egress for the Ethereum chain.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.EnumFetchOrTransfer>> ScheduledEgressFetchOrTransfer(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.ScheduledEgressFetchOrTransferParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.EnumFetchOrTransfer>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ScheduledEgressCcmParams
        ///  Scheduled cross chain messages for the Ethereum chain.
        /// </summary>
        public static string ScheduledEgressCcmParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "ScheduledEgressCcm", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ScheduledEgressCcmDefault
        /// Default value as hex string
        /// </summary>
        public static string ScheduledEgressCcmDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> ScheduledEgressCcm
        ///  Scheduled cross chain messages for the Ethereum chain.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.CrossChainMessageT3>> ScheduledEgressCcm(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.ScheduledEgressCcmParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.CrossChainMessageT3>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DisabledEgressAssetsParams
        ///  Stores the list of assets that are not allowed to be egressed.
        /// </summary>
        public static string DisabledEgressAssetsParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "DisabledEgressAssets", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> DisabledEgressAssetsDefault
        /// Default value as hex string
        /// </summary>
        public static string DisabledEgressAssetsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> DisabledEgressAssets
        ///  Stores the list of assets that are not allowed to be egressed.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple> DisabledEgressAssets(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.DisabledEgressAssetsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositChannelPoolParams
        ///  Stores address ready for use.
        /// </summary>
        public static string DepositChannelPoolParams(Substrate.NetApi.Model.Types.Primitive.U64 key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "DepositChannelPool", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> DepositChannelPoolDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositChannelPoolDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> DepositChannelPool
        ///  Stores address ready for use.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel.DepositChannelT3> DepositChannelPool(Substrate.NetApi.Model.Types.Primitive.U64 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.DepositChannelPoolParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel.DepositChannelT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> MinimumDepositParams
        ///  Defines the minimum amount of Deposit allowed for each asset.
        /// </summary>
        public static string MinimumDepositParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "MinimumDeposit", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> MinimumDepositDefault
        /// Default value as hex string
        /// </summary>
        public static string MinimumDepositDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> MinimumDeposit
        ///  Defines the minimum amount of Deposit allowed for each asset.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> MinimumDeposit(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.MinimumDepositParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> EgressDustLimitParams
        ///  Defines the minimum amount aka. dust limit for a single egress i.e. *not* of a batch, but
        ///  the outputs of each individual egress within that batch. If not set, defaults to 1.
        /// 
        ///  This is required for bitcoin, for example, where any amount below 600 satoshis is considered
        ///  dust and will be rejected by miners.
        /// </summary>
        public static string EgressDustLimitParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "EgressDustLimit", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> EgressDustLimitDefault
        /// Default value as hex string
        /// </summary>
        public static string EgressDustLimitDefault()
        {
            return "0x01000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> EgressDustLimit
        ///  Defines the minimum amount aka. dust limit for a single egress i.e. *not* of a batch, but
        ///  the outputs of each individual egress within that batch. If not set, defaults to 1.
        /// 
        ///  This is required for bitcoin, for example, where any amount below 600 satoshis is considered
        ///  dust and will be rejected by miners.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> EgressDustLimit(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.EgressDustLimitParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositChannelLifetimeParams
        /// </summary>
        public static string DepositChannelLifetimeParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "DepositChannelLifetime", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> DepositChannelLifetimeDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositChannelLifetimeDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> DepositChannelLifetime
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> DepositChannelLifetime(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.DepositChannelLifetimeParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> FailedForeignChainCallsParams
        ///  Stores information about Calls to external chains that have failed to be broadcasted.
        ///  These calls are signed and stored on-chain so that the user can broadcast the call
        ///  themselves. These messages will be re-threshold-signed once during the next epoch, and
        ///  removed from storage in the epoch after that.
        ///  Hashmap: last_signed_epoch -> Vec<FailedForeignChainCall>
        /// </summary>
        public static string FailedForeignChainCallsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "FailedForeignChainCalls", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> FailedForeignChainCallsDefault
        /// Default value as hex string
        /// </summary>
        public static string FailedForeignChainCallsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> FailedForeignChainCalls
        ///  Stores information about Calls to external chains that have failed to be broadcasted.
        ///  These calls are signed and stored on-chain so that the user can broadcast the call
        ///  themselves. These messages will be re-threshold-signed once during the next epoch, and
        ///  removed from storage in the epoch after that.
        ///  Hashmap: last_signed_epoch -> Vec<FailedForeignChainCall>
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.FailedForeignChainCall>> FailedForeignChainCalls(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.FailedForeignChainCallsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.FailedForeignChainCall>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositBalancesParams
        /// </summary>
        public static string DepositBalancesParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "DepositBalances", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> DepositBalancesDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositBalancesDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> DepositBalances
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositTrackerT3> DepositBalances(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.DepositBalancesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositTrackerT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositChannelRecycleBlocksParams
        /// </summary>
        public static string DepositChannelRecycleBlocksParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "DepositChannelRecycleBlocks", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> DepositChannelRecycleBlocksDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositChannelRecycleBlocksDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> DepositChannelRecycleBlocks
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey>>> DepositChannelRecycleBlocks(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.DepositChannelRecycleBlocksParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey>>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> WitnessSafetyMarginParams
        /// </summary>
        public static string WitnessSafetyMarginParams()
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "WitnessSafetyMargin", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> WitnessSafetyMarginDefault
        /// Default value as hex string
        /// </summary>
        public static string WitnessSafetyMarginDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> WitnessSafetyMargin
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> WitnessSafetyMargin(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.WitnessSafetyMarginParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> WithheldTransactionFeesParams
        ///  Tracks fees withheld from ingresses and egresses.
        /// </summary>
        public static string WithheldTransactionFeesParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key)
        {
            return RequestGenerator.GetStorage("BitcoinIngressEgress", "WithheldTransactionFees", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> WithheldTransactionFeesDefault
        /// Default value as hex string
        /// </summary>
        public static string WithheldTransactionFeesDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> WithheldTransactionFees
        ///  Tracks fees withheld from ingresses and egresses.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> WithheldTransactionFees(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinIngressEgressStorage.WithheldTransactionFeesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> BitcoinIngressEgressCalls
    /// </summary>
    public sealed class BitcoinIngressEgressCalls
    {
        
        /// <summary>
        /// >> finalise_ingress
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method FinaliseIngress(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey> addresses)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(addresses.Encode());
            return new Method(34, "BitcoinIngressEgress", 0, "finalise_ingress", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> enable_or_disable_egress
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method EnableOrDisableEgress(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset asset, Substrate.NetApi.Model.Types.Primitive.Bool set_disabled)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset.Encode());
            byteArray.AddRange(set_disabled.Encode());
            return new Method(34, "BitcoinIngressEgress", 1, "enable_or_disable_egress", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> process_deposits
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ProcessDeposits(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositWitnessT3> deposit_witnesses, Substrate.NetApi.Model.Types.Primitive.U64 block_height)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(deposit_witnesses.Encode());
            byteArray.AddRange(block_height.Encode());
            return new Method(34, "BitcoinIngressEgress", 2, "process_deposits", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_minimum_deposit
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetMinimumDeposit(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset asset, Substrate.NetApi.Model.Types.Primitive.U64 minimum_deposit)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset.Encode());
            byteArray.AddRange(minimum_deposit.Encode());
            return new Method(34, "BitcoinIngressEgress", 3, "set_minimum_deposit", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> vault_transfer_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method VaultTransferFailed(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.btc.EnumAsset asset, Substrate.NetApi.Model.Types.Primitive.U64 amount, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey destination_address)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset.Encode());
            byteArray.AddRange(amount.Encode());
            byteArray.AddRange(destination_address.Encode());
            return new Method(34, "BitcoinIngressEgress", 4, "vault_transfer_failed", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> ccm_broadcast_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method CcmBroadcastFailed(Substrate.NetApi.Model.Types.Primitive.U32 broadcast_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(broadcast_id.Encode());
            return new Method(34, "BitcoinIngressEgress", 5, "ccm_broadcast_failed", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> BitcoinIngressEgressConstants
    /// </summary>
    public sealed class BitcoinIngressEgressConstants
    {
    }
    
    /// <summary>
    /// >> BitcoinIngressEgressErrors
    /// </summary>
    public enum BitcoinIngressEgressErrors
    {
        
        /// <summary>
        /// >> InvalidDepositAddress
        /// The deposit address is not valid. It may have expired or may never have been issued.
        /// </summary>
        InvalidDepositAddress,
        
        /// <summary>
        /// >> AssetMismatch
        /// A deposit was made using the wrong asset.
        /// </summary>
        AssetMismatch,
        
        /// <summary>
        /// >> ChannelIdsExhausted
        /// Channel ID has reached maximum
        /// </summary>
        ChannelIdsExhausted,
        
        /// <summary>
        /// >> MissingPolkadotVault
        /// Polkadot's Vault Account does not exist in storage.
        /// </summary>
        MissingPolkadotVault,
        
        /// <summary>
        /// >> MissingBitcoinVault
        /// Bitcoin's Vault key does not exist for the current epoch.
        /// </summary>
        MissingBitcoinVault,
        
        /// <summary>
        /// >> BitcoinChannelIdTooLarge
        /// Channel ID is too large for Bitcoin address derivation
        /// </summary>
        BitcoinChannelIdTooLarge,
        
        /// <summary>
        /// >> BelowEgressDustLimit
        /// The amount is below the minimum egress amount.
        /// </summary>
        BelowEgressDustLimit,
    }
}
