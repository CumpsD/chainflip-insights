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
    /// >> PolkadotBroadcasterStorage
    /// </summary>
    public sealed class PolkadotBroadcasterStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> PolkadotBroadcasterStorage Constructor
        /// </summary>
        public PolkadotBroadcasterStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "BroadcastIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "RequestSuccessCallbacks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "RequestFailureCallbacks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "FailedBroadcasters"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "AwaitingBroadcast"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet.BroadcastDataT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "TransactionOutIdToBroadcastId"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "DelayedBroadcastRetryQueue"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "Timeouts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "ThresholdSignatureData"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.dot.api.EnumPolkadotApi, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "TransactionMetadata"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseTuple)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "TransactionFeeDeficit"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "BroadcastBarriers"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "PendingBroadcasts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("PolkadotBroadcaster", "AbortedBroadcasts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>)));
        }
        
        /// <summary>
        /// >> BroadcastIdCounterParams
        ///  A counter for incrementing the broadcast id.
        /// </summary>
        public static string BroadcastIdCounterParams()
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "BroadcastIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
            string parameters = PolkadotBroadcasterStorage.BroadcastIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> RequestSuccessCallbacksParams
        ///  Callbacks to be dispatched when the SignatureAccepted event has been witnessed.
        /// </summary>
        public static string RequestSuccessCallbacksParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "RequestSuccessCallbacks", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
            string parameters = PolkadotBroadcasterStorage.RequestSuccessCallbacksParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> RequestFailureCallbacksParams
        ///  Callbacks to be dispatched when a broadcast failure has been witnessed.
        /// </summary>
        public static string RequestFailureCallbacksParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "RequestFailureCallbacks", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
            string parameters = PolkadotBroadcasterStorage.RequestFailureCallbacksParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> FailedBroadcastersParams
        ///  Contains a Set of the authorities that have failed to sign a particular broadcast.
        /// </summary>
        public static string FailedBroadcastersParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "FailedBroadcasters", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
            string parameters = PolkadotBroadcasterStorage.FailedBroadcastersParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AwaitingBroadcastParams
        ///  Live transaction broadcast requests.
        /// </summary>
        public static string AwaitingBroadcastParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "AwaitingBroadcast", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet.BroadcastDataT2> AwaitingBroadcast(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.AwaitingBroadcastParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_broadcast.pallet.BroadcastDataT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TransactionOutIdToBroadcastIdParams
        ///  Lookup table between TransactionOutId -> Broadcast.
        ///  This storage item is used by the CFE to track which broadcasts/egresses it needs to
        ///  witness.
        /// </summary>
        public static string TransactionOutIdToBroadcastIdParams(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "TransactionOutIdToBroadcastId", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>> TransactionOutIdToBroadcastId(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.TransactionOutIdToBroadcastIdParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> DelayedBroadcastRetryQueueParams
        ///  The list of failed broadcasts that will be retried in some future block.
        /// </summary>
        public static string DelayedBroadcastRetryQueueParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "DelayedBroadcastRetryQueue", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2> DelayedBroadcastRetryQueue(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.DelayedBroadcastRetryQueueParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TimeoutsParams
        ///  A mapping from block number to a list of broadcasts that expire at that
        ///  block number.
        /// </summary>
        public static string TimeoutsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "Timeouts", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3> Timeouts(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.TimeoutsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ThresholdSignatureDataParams
        ///  Stores all needed information to be able to re-request the signature
        /// </summary>
        public static string ThresholdSignatureDataParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "ThresholdSignatureData", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
        public async Task<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.dot.api.EnumPolkadotApi, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature>> ThresholdSignatureData(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.ThresholdSignatureDataParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_chains.dot.api.EnumPolkadotApi, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TransactionMetadataParams
        ///  Stores metadata related to a transaction.
        /// </summary>
        public static string TransactionMetadataParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "TransactionMetadata", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
            string parameters = PolkadotBroadcasterStorage.TransactionMetadataParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseTuple>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TransactionFeeDeficitParams
        ///  Tracks how much a signer id is owed for paying transaction fees.
        /// </summary>
        public static string TransactionFeeDeficitParams(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId key)
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "TransactionFeeDeficit", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TransactionFeeDeficitDefault
        /// Default value as hex string
        /// </summary>
        public static string TransactionFeeDeficitDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> TransactionFeeDeficit
        ///  Tracks how much a signer id is owed for paying transaction fees.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> TransactionFeeDeficit(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId key, string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.TransactionFeeDeficitParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> BroadcastBarriersParams
        ///  Whether or not broadcasts are paused for broadcast ids greater than the given broadcast id.
        /// </summary>
        public static string BroadcastBarriersParams()
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "BroadcastBarriers", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2> BroadcastBarriers(string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.BroadcastBarriersParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingBroadcastsParams
        ///  List of broadcasts that are initiated but not witnessed on the external chain.
        /// </summary>
        public static string PendingBroadcastsParams()
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "PendingBroadcasts", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2> PendingBroadcasts(string blockhash, CancellationToken token)
        {
            string parameters = PolkadotBroadcasterStorage.PendingBroadcastsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT2>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AbortedBroadcastsParams
        ///  List of broadcasts that have been aborted because they were unsuccessful to be broadcast
        ///  after many retries.
        /// </summary>
        public static string AbortedBroadcastsParams()
        {
            return RequestGenerator.GetStorage("PolkadotBroadcaster", "AbortedBroadcasts", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
            string parameters = PolkadotBroadcasterStorage.AbortedBroadcastsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> PolkadotBroadcasterCalls
    /// </summary>
    public sealed class PolkadotBroadcasterCalls
    {
        
        /// <summary>
        /// >> transaction_signing_failure
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method TransactionSigningFailure(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32> broadcast_attempt_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(broadcast_attempt_id.Encode());
            return new Method(28, "PolkadotBroadcaster", 0, "transaction_signing_failure", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> on_signature_ready
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method OnSignatureReady(Substrate.NetApi.Model.Types.Primitive.U32 threshold_request_id, Substrate.NetApiExt.Generated.Model.cf_chains.dot.EncodedPolkadotPayload threshold_signature_payload, Substrate.NetApiExt.Generated.Model.cf_chains.dot.api.EnumPolkadotApi api_call, Substrate.NetApi.Model.Types.Primitive.U32 broadcast_id, Substrate.NetApi.Model.Types.Primitive.U32 initiated_at, Substrate.NetApi.Model.Types.Primitive.Bool should_broadcast)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(threshold_request_id.Encode());
            byteArray.AddRange(threshold_signature_payload.Encode());
            byteArray.AddRange(api_call.Encode());
            byteArray.AddRange(broadcast_id.Encode());
            byteArray.AddRange(initiated_at.Encode());
            byteArray.AddRange(should_broadcast.Encode());
            return new Method(28, "PolkadotBroadcaster", 1, "on_signature_ready", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> transaction_succeeded
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method TransactionSucceeded(Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotSignature tx_out_id, Substrate.NetApiExt.Generated.Model.cf_chains.dot.PolkadotAccountId signer_id, Substrate.NetApi.Model.Types.Primitive.U128 tx_fee, Substrate.NetApi.Model.Types.Base.BaseTuple tx_metadata)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(tx_out_id.Encode());
            byteArray.AddRange(signer_id.Encode());
            byteArray.AddRange(tx_fee.Encode());
            byteArray.AddRange(tx_metadata.Encode());
            return new Method(28, "PolkadotBroadcaster", 2, "transaction_succeeded", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> stress_test
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method StressTest(Substrate.NetApi.Model.Types.Primitive.U32 how_many)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(how_many.Encode());
            return new Method(28, "PolkadotBroadcaster", 3, "stress_test", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> transaction_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method TransactionFailed(Substrate.NetApi.Model.Types.Primitive.U32 broadcast_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(broadcast_id.Encode());
            return new Method(28, "PolkadotBroadcaster", 4, "transaction_failed", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> PolkadotBroadcasterConstants
    /// </summary>
    public sealed class PolkadotBroadcasterConstants
    {
        
        /// <summary>
        /// >> BroadcastTimeout
        ///  The timeout duration for the broadcast, measured in number of blocks.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 BroadcastTimeout()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x64000000");
            return result;
        }
    }
    
    /// <summary>
    /// >> PolkadotBroadcasterErrors
    /// </summary>
    public enum PolkadotBroadcasterErrors
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