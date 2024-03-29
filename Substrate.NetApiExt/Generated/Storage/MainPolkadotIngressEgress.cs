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
    /// >> PolkadotIngressEgressStorage
    /// </summary>
    public sealed class PolkadotIngressEgressStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> PolkadotIngressEgressStorage Constructor
        /// </summary>
        public PolkadotIngressEgressStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "DepositChannelLookup"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositChannelDetailsT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "ChannelIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "EgressIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "ScheduledEgressFetchOrTransfer"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.EnumFetchOrTransfer>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "ScheduledEgressCcm"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.CrossChainMessageT2>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "DisabledEgressAssets"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "DepositChannelPool"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U64), typeof(Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel.DepositChannelT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "MinimumDeposit"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "EgressDustLimit"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "DepositChannelLifetime"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "FailedForeignChainCalls"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.FailedForeignChainCall>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "DepositBalances"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositTrackerT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "DepositChannelRecycleBlocks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId>>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "WitnessSafetyMargin"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotIngressEgress", "WithheldTransactionFees"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
        }
        
        /// <summary>
        /// >> DepositChannelLookupParams
        ///  Lookup table for addresses to corresponding deposit channels.
        /// </summary>
        public static string DepositChannelLookupParams(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "DepositChannelLookup", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositChannelDetailsT2> DepositChannelLookup(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.DepositChannelLookupParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositChannelDetailsT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ChannelIdCounterParams
        ///  Stores the latest channel id used to generate an address.
        /// </summary>
        public static string ChannelIdCounterParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "ChannelIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
            string parameters = PolkadotIngressEgressStorage.ChannelIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> EgressIdCounterParams
        ///  Stores the latest egress id used to generate an address.
        /// </summary>
        public static string EgressIdCounterParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "EgressIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
            string parameters = PolkadotIngressEgressStorage.EgressIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ScheduledEgressFetchOrTransferParams
        ///  Scheduled fetch and egress for the Ethereum chain.
        /// </summary>
        public static string ScheduledEgressFetchOrTransferParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "ScheduledEgressFetchOrTransfer", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
            string parameters = PolkadotIngressEgressStorage.ScheduledEgressFetchOrTransferParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.EnumFetchOrTransfer>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ScheduledEgressCcmParams
        ///  Scheduled cross chain messages for the Ethereum chain.
        /// </summary>
        public static string ScheduledEgressCcmParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "ScheduledEgressCcm", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.CrossChainMessageT2>> ScheduledEgressCcm(string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.ScheduledEgressCcmParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.CrossChainMessageT2>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DisabledEgressAssetsParams
        ///  Stores the list of assets that are not allowed to be egressed.
        /// </summary>
        public static string DisabledEgressAssetsParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "DisabledEgressAssets", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple> DisabledEgressAssets(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.DisabledEgressAssetsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositChannelPoolParams
        ///  Stores address ready for use.
        /// </summary>
        public static string DepositChannelPoolParams(Substrate.NetApi.Model.Types.Primitive.U64 key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "DepositChannelPool", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel.DepositChannelT2> DepositChannelPool(Substrate.NetApi.Model.Types.Primitive.U64 key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.DepositChannelPoolParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.cf_chains.deposit_channel.DepositChannelT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> MinimumDepositParams
        ///  Defines the minimum amount of Deposit allowed for each asset.
        /// </summary>
        public static string MinimumDepositParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "MinimumDeposit", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> MinimumDepositDefault
        /// Default value as hex string
        /// </summary>
        public static string MinimumDepositDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> MinimumDeposit
        ///  Defines the minimum amount of Deposit allowed for each asset.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> MinimumDeposit(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.MinimumDepositParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
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
        public static string EgressDustLimitParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "EgressDustLimit", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> EgressDustLimit(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.EgressDustLimitParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositChannelLifetimeParams
        /// </summary>
        public static string DepositChannelLifetimeParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "DepositChannelLifetime", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> DepositChannelLifetimeDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositChannelLifetimeDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> DepositChannelLifetime
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> DepositChannelLifetime(string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.DepositChannelLifetimeParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
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
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "FailedForeignChainCalls", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
            string parameters = PolkadotIngressEgressStorage.FailedForeignChainCallsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.FailedForeignChainCall>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositBalancesParams
        /// </summary>
        public static string DepositBalancesParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "DepositBalances", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> DepositBalancesDefault
        /// Default value as hex string
        /// </summary>
        public static string DepositBalancesDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> DepositBalances
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositTrackerT2> DepositBalances(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.DepositBalancesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositTrackerT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DepositChannelRecycleBlocksParams
        /// </summary>
        public static string DepositChannelRecycleBlocksParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "DepositChannelRecycleBlocks", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId>>> DepositChannelRecycleBlocks(string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.DepositChannelRecycleBlocksParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId>>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> WitnessSafetyMarginParams
        /// </summary>
        public static string WitnessSafetyMarginParams()
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "WitnessSafetyMargin", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> WitnessSafetyMargin(string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.WitnessSafetyMarginParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> WithheldTransactionFeesParams
        ///  Tracks fees withheld from ingresses and egresses.
        /// </summary>
        public static string WithheldTransactionFeesParams(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key)
        {
            return RequestGenerator.GetStorage("PolkadotIngressEgress", "WithheldTransactionFees", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> WithheldTransactionFeesDefault
        /// Default value as hex string
        /// </summary>
        public static string WithheldTransactionFeesDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> WithheldTransactionFees
        ///  Tracks fees withheld from ingresses and egresses.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> WithheldTransactionFees(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotIngressEgressStorage.WithheldTransactionFeesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> PolkadotIngressEgressCalls
    /// </summary>
    public sealed class PolkadotIngressEgressCalls
    {
        
        /// <summary>
        /// >> finalise_ingress
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method FinaliseIngress(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId> addresses)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(addresses.Encode());
            return new Method(33, "PolkadotIngressEgress", 0, "finalise_ingress", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> enable_or_disable_egress
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method EnableOrDisableEgress(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset asset, Substrate.NetApi.Model.Types.Primitive.Bool set_disabled)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset.Encode());
            byteArray.AddRange(set_disabled.Encode());
            return new Method(33, "PolkadotIngressEgress", 1, "enable_or_disable_egress", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> process_deposits
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ProcessDeposits(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_ingress_egress.pallet.DepositWitnessT2> deposit_witnesses, Substrate.NetApi.Model.Types.Primitive.U32 block_height)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(deposit_witnesses.Encode());
            byteArray.AddRange(block_height.Encode());
            return new Method(33, "PolkadotIngressEgress", 2, "process_deposits", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_minimum_deposit
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetMinimumDeposit(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset asset, Substrate.NetApi.Model.Types.Primitive.U128 minimum_deposit)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset.Encode());
            byteArray.AddRange(minimum_deposit.Encode());
            return new Method(33, "PolkadotIngressEgress", 3, "set_minimum_deposit", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> vault_transfer_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method VaultTransferFailed(Substrate.NetApiExt.Generated.Model.cf_primitives.chains.assets.dot.EnumAsset asset, Substrate.NetApi.Model.Types.Primitive.U128 amount, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId destination_address)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset.Encode());
            byteArray.AddRange(amount.Encode());
            byteArray.AddRange(destination_address.Encode());
            return new Method(33, "PolkadotIngressEgress", 4, "vault_transfer_failed", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> ccm_broadcast_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method CcmBroadcastFailed(Substrate.NetApi.Model.Types.Primitive.U32 broadcast_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(broadcast_id.Encode());
            return new Method(33, "PolkadotIngressEgress", 5, "ccm_broadcast_failed", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> PolkadotIngressEgressConstants
    /// </summary>
    public sealed class PolkadotIngressEgressConstants
    {
    }
    
    /// <summary>
    /// >> PolkadotIngressEgressErrors
    /// </summary>
    public enum PolkadotIngressEgressErrors
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
