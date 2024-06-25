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
    /// >> BitcoinBroadcasterStorage
    /// </summary>
    public sealed class BitcoinBroadcasterStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> BitcoinBroadcasterStorage Constructor
        /// </summary>
        public BitcoinBroadcasterStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "BroadcastIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "RequestSuccessCallbacks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "RequestFailureCallbacks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "FailedBroadcasters"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "AwaitingBroadcast"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet.BroadcastDataT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "TransactionOutIdToBroadcastId"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.primitive_types.H256), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U64>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "DelayedBroadcastRetryQueue"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "Timeouts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT4)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "ThresholdSignatureData"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.api.EnumBitcoinApi, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Types.Base.Arr64U8>>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "TransactionMetadata"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "TransactionFeeDeficit"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey), typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "BroadcastBarriers"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "PendingBroadcasts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinBroadcaster", "AbortedBroadcasts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>)));
        }
        
        /// <summary>
        /// >> BroadcastIdCounterParams
        ///  A counter for incrementing the broadcast id.
        /// </summary>
        public static string BroadcastIdCounterParams()
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "BroadcastIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> BroadcastIdCounterDefault
        /// Default value as hex string
        /// </summary>
        public static string BroadcastIdCounterDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> BroadcastIdCounter
        ///  A counter for incrementing the broadcast id.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> BroadcastIdCounter(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.BroadcastIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> RequestSuccessCallbacksParams
        ///  Callbacks to be dispatched when the SignatureAccepted event has been witnessed.
        /// </summary>
        public static string RequestSuccessCallbacksParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "RequestSuccessCallbacks", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> RequestSuccessCallbacksDefault
        /// Default value as hex string
        /// </summary>
        public static string RequestSuccessCallbacksDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> RequestSuccessCallbacks
        ///  Callbacks to be dispatched when the SignatureAccepted event has been witnessed.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall> RequestSuccessCallbacks(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.RequestSuccessCallbacksParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> RequestFailureCallbacksParams
        ///  Callbacks to be dispatched when a broadcast failure has been witnessed.
        /// </summary>
        public static string RequestFailureCallbacksParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "RequestFailureCallbacks", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> RequestFailureCallbacksDefault
        /// Default value as hex string
        /// </summary>
        public static string RequestFailureCallbacksDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> RequestFailureCallbacks
        ///  Callbacks to be dispatched when a broadcast failure has been witnessed.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall> RequestFailureCallbacks(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.RequestFailureCallbacksParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> FailedBroadcastersParams
        ///  Contains a Set of the authorities that have failed to sign a particular broadcast.
        /// </summary>
        public static string FailedBroadcastersParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "FailedBroadcasters", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> FailedBroadcastersDefault
        /// Default value as hex string
        /// </summary>
        public static string FailedBroadcastersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> FailedBroadcasters
        ///  Contains a Set of the authorities that have failed to sign a particular broadcast.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1> FailedBroadcasters(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.FailedBroadcastersParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AwaitingBroadcastParams
        ///  Live transaction broadcast requests.
        /// </summary>
        public static string AwaitingBroadcastParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "AwaitingBroadcast", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AwaitingBroadcastDefault
        /// Default value as hex string
        /// </summary>
        public static string AwaitingBroadcastDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AwaitingBroadcast
        ///  Live transaction broadcast requests.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet.BroadcastDataT3> AwaitingBroadcast(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.AwaitingBroadcastParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet.BroadcastDataT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TransactionOutIdToBroadcastIdParams
        ///  Lookup table between TransactionOutId -> Broadcast.
        ///  This storage item is used by the CFE to track which broadcasts/egresses it needs to
        ///  witness.
        /// </summary>
        public static string TransactionOutIdToBroadcastIdParams(Substrate.NetApiExt.Generated.Model.primitive_types.H256 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "TransactionOutIdToBroadcastId", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TransactionOutIdToBroadcastIdDefault
        /// Default value as hex string
        /// </summary>
        public static string TransactionOutIdToBroadcastIdDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> TransactionOutIdToBroadcastId
        ///  Lookup table between TransactionOutId -> Broadcast.
        ///  This storage item is used by the CFE to track which broadcasts/egresses it needs to
        ///  witness.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U64>> TransactionOutIdToBroadcastId(Substrate.NetApiExt.Generated.Model.primitive_types.H256 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.TransactionOutIdToBroadcastIdParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U64>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DelayedBroadcastRetryQueueParams
        ///  The list of failed broadcasts that will be retried in some future block.
        /// </summary>
        public static string DelayedBroadcastRetryQueueParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "DelayedBroadcastRetryQueue", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> DelayedBroadcastRetryQueueDefault
        /// Default value as hex string
        /// </summary>
        public static string DelayedBroadcastRetryQueueDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> DelayedBroadcastRetryQueue
        ///  The list of failed broadcasts that will be retried in some future block.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3> DelayedBroadcastRetryQueue(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.DelayedBroadcastRetryQueueParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TimeoutsParams
        ///  A mapping from block number to a list of broadcasts that expire at that
        ///  block number.
        /// </summary>
        public static string TimeoutsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "Timeouts", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TimeoutsDefault
        /// Default value as hex string
        /// </summary>
        public static string TimeoutsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Timeouts
        ///  A mapping from block number to a list of broadcasts that expire at that
        ///  block number.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT4> Timeouts(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.TimeoutsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT4>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ThresholdSignatureDataParams
        ///  Stores all needed information to be able to re-request the signature
        /// </summary>
        public static string ThresholdSignatureDataParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "ThresholdSignatureData", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> ThresholdSignatureDataDefault
        /// Default value as hex string
        /// </summary>
        public static string ThresholdSignatureDataDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> ThresholdSignatureData
        ///  Stores all needed information to be able to re-request the signature
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.api.EnumBitcoinApi, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Types.Base.Arr64U8>>> ThresholdSignatureData(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.ThresholdSignatureDataParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.api.EnumBitcoinApi, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Types.Base.Arr64U8>>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TransactionMetadataParams
        ///  Stores metadata related to a transaction.
        /// </summary>
        public static string TransactionMetadataParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "TransactionMetadata", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TransactionMetadataDefault
        /// Default value as hex string
        /// </summary>
        public static string TransactionMetadataDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> TransactionMetadata
        ///  Stores metadata related to a transaction.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple> TransactionMetadata(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.TransactionMetadataParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TransactionFeeDeficitParams
        ///  Tracks how much a signer id is owed for paying transaction fees.
        /// </summary>
        public static string TransactionFeeDeficitParams(Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey key)
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "TransactionFeeDeficit", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TransactionFeeDeficitDefault
        /// Default value as hex string
        /// </summary>
        public static string TransactionFeeDeficitDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> TransactionFeeDeficit
        ///  Tracks how much a signer id is owed for paying transaction fees.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> TransactionFeeDeficit(Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.TransactionFeeDeficitParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> BroadcastBarriersParams
        ///  Whether or not broadcasts are paused for broadcast ids greater than the given broadcast id.
        /// </summary>
        public static string BroadcastBarriersParams()
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "BroadcastBarriers", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> BroadcastBarriersDefault
        /// Default value as hex string
        /// </summary>
        public static string BroadcastBarriersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> BroadcastBarriers
        ///  Whether or not broadcasts are paused for broadcast ids greater than the given broadcast id.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3> BroadcastBarriers(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.BroadcastBarriersParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingBroadcastsParams
        ///  List of broadcasts that are initiated but not witnessed on the external chain.
        /// </summary>
        public static string PendingBroadcastsParams()
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "PendingBroadcasts", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> PendingBroadcastsDefault
        /// Default value as hex string
        /// </summary>
        public static string PendingBroadcastsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PendingBroadcasts
        ///  List of broadcasts that are initiated but not witnessed on the external chain.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3> PendingBroadcasts(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.PendingBroadcastsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AbortedBroadcastsParams
        ///  List of broadcasts that have been aborted because they were unsuccessful to be broadcast
        ///  after many retries.
        /// </summary>
        public static string AbortedBroadcastsParams()
        {
            return RequestGenerator.GetStorage("BitcoinBroadcaster", "AbortedBroadcasts", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> AbortedBroadcastsDefault
        /// Default value as hex string
        /// </summary>
        public static string AbortedBroadcastsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AbortedBroadcasts
        ///  List of broadcasts that have been aborted because they were unsuccessful to be broadcast
        ///  after many retries.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>> AbortedBroadcasts(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinBroadcasterStorage.AbortedBroadcastsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> BitcoinBroadcasterCalls
    /// </summary>
    public sealed class BitcoinBroadcasterCalls
    {
        
        /// <summary>
        /// >> transaction_signing_failure
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method TransactionSigningFailure(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32> broadcast_attempt_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(broadcast_attempt_id.Encode());
            return new Method(29, "BitcoinBroadcaster", 0, "transaction_signing_failure", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> on_signature_ready
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method OnSignatureReady(Substrate.NetApi.Model.Types.Primitive.U32 threshold_request_id, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumPreviousOrCurrent, Substrate.NetApiExt.Generated.Types.Base.Arr32U8>> threshold_signature_payload, Substrate.NetApiExt.Generated.Model.cf_chains.btc.api.EnumBitcoinApi api_call, Substrate.NetApi.Model.Types.Primitive.U32 broadcast_id, Substrate.NetApi.Model.Types.Primitive.U64 initiated_at, Substrate.NetApi.Model.Types.Primitive.Bool should_broadcast)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(threshold_request_id.Encode());
            byteArray.AddRange(threshold_signature_payload.Encode());
            byteArray.AddRange(api_call.Encode());
            byteArray.AddRange(broadcast_id.Encode());
            byteArray.AddRange(initiated_at.Encode());
            byteArray.AddRange(should_broadcast.Encode());
            return new Method(29, "BitcoinBroadcaster", 1, "on_signature_ready", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> transaction_succeeded
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method TransactionSucceeded(Substrate.NetApiExt.Generated.Model.primitive_types.H256 tx_out_id, Substrate.NetApiExt.Generated.Model.cf_chains.btc.EnumScriptPubkey signer_id, Substrate.NetApi.Model.Types.Primitive.U64 tx_fee, Substrate.NetApi.Model.Types.Base.BaseTuple tx_metadata, Substrate.NetApiExt.Generated.Model.primitive_types.H256 transaction_ref)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(tx_out_id.Encode());
            byteArray.AddRange(signer_id.Encode());
            byteArray.AddRange(tx_fee.Encode());
            byteArray.AddRange(tx_metadata.Encode());
            byteArray.AddRange(transaction_ref.Encode());
            return new Method(29, "BitcoinBroadcaster", 2, "transaction_succeeded", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> stress_test
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method StressTest(Substrate.NetApi.Model.Types.Primitive.U32 how_many)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(how_many.Encode());
            return new Method(29, "BitcoinBroadcaster", 3, "stress_test", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> transaction_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method TransactionFailed(Substrate.NetApi.Model.Types.Primitive.U32 broadcast_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(broadcast_id.Encode());
            return new Method(29, "BitcoinBroadcaster", 4, "transaction_failed", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> BitcoinBroadcasterConstants
    /// </summary>
    public sealed class BitcoinBroadcasterConstants
    {
        
        /// <summary>
        /// >> BroadcastTimeout
        ///  The timeout duration for the broadcast, measured in number of blocks.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 BroadcastTimeout()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x84030000");
            return result;
        }
    }
    
    /// <summary>
    /// >> BitcoinBroadcasterErrors
    /// </summary>
    public enum BitcoinBroadcasterErrors
    {
        
        /// <summary>
        /// >> InvalidPayload
        /// The provided payload is invalid.
        /// </summary>
        InvalidPayload,
        
        /// <summary>
        /// >> InvalidBroadcastId
        /// The provided broadcast id is invalid.
        /// </summary>
        InvalidBroadcastId,
        
        /// <summary>
        /// >> ThresholdSignatureUnavailable
        /// A threshold signature was expected but not available.
        /// </summary>
        ThresholdSignatureUnavailable,
    }
}
