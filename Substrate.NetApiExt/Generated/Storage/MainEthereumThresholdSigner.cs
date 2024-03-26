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
    /// >> EthereumThresholdSignerStorage
    /// </summary>
    public sealed class EthereumThresholdSignerStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> EthereumThresholdSignerStorage Constructor
        /// </summary>
        public EthereumThresholdSignerStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "ThresholdSignatureRequestIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "PendingCeremonies"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U64), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.CeremonyContextT1)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "PendingRequestInstructions"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestInstructionT1)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "RequestCallback"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "Signature"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.cf_traits.async_result.EnumAsyncResult)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "CeremonyRetryQueues"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U64>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "RequestRetryQueue"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EthereumThresholdSigner", "ThresholdSignatureResponseTimeout"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
        }
        
        /// <summary>
        /// >> ThresholdSignatureRequestIdCounterParams
        ///  A counter to generate fresh request ids.
        /// </summary>
        public static string ThresholdSignatureRequestIdCounterParams()
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "ThresholdSignatureRequestIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ThresholdSignatureRequestIdCounterDefault
        /// Default value as hex string
        /// </summary>
        public static string ThresholdSignatureRequestIdCounterDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> ThresholdSignatureRequestIdCounter
        ///  A counter to generate fresh request ids.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> ThresholdSignatureRequestIdCounter(string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.ThresholdSignatureRequestIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingCeremoniesParams
        ///  Stores the context required for processing live ceremonies.
        /// </summary>
        public static string PendingCeremoniesParams(Substrate.NetApi.Model.Types.Primitive.U64 key)
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "PendingCeremonies", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> PendingCeremoniesDefault
        /// Default value as hex string
        /// </summary>
        public static string PendingCeremoniesDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PendingCeremonies
        ///  Stores the context required for processing live ceremonies.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.CeremonyContextT1> PendingCeremonies(Substrate.NetApi.Model.Types.Primitive.U64 key, string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.PendingCeremoniesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.CeremonyContextT1>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingRequestInstructionsParams
        /// </summary>
        public static string PendingRequestInstructionsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "PendingRequestInstructions", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> PendingRequestInstructionsDefault
        /// Default value as hex string
        /// </summary>
        public static string PendingRequestInstructionsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PendingRequestInstructions
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestInstructionT1> PendingRequestInstructions(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.PendingRequestInstructionsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_threshold_signature.pallet.RequestInstructionT1>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> RequestCallbackParams
        ///  Callbacks to be dispatched when a request is fulfilled.
        /// </summary>
        public static string RequestCallbackParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "RequestCallback", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> RequestCallbackDefault
        /// Default value as hex string
        /// </summary>
        public static string RequestCallbackDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> RequestCallback
        ///  Callbacks to be dispatched when a request is fulfilled.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall> RequestCallback(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.RequestCallbackParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> SignatureParams
        ///  State of the threshold signature requested.
        /// </summary>
        public static string SignatureParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "Signature", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> SignatureDefault
        /// Default value as hex string
        /// </summary>
        public static string SignatureDefault()
        {
            return "0x02";
        }
        
        /// <summary>
        /// >> Signature
        ///  State of the threshold signature requested.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.cf_traits.async_result.EnumAsyncResult> Signature(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.SignatureParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.cf_traits.async_result.EnumAsyncResult>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> CeremonyRetryQueuesParams
        ///  A map containing lists of ceremony ids that should be retried at the block stored in the
        ///  key.
        /// </summary>
        public static string CeremonyRetryQueuesParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "CeremonyRetryQueues", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> CeremonyRetryQueuesDefault
        /// Default value as hex string
        /// </summary>
        public static string CeremonyRetryQueuesDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> CeremonyRetryQueues
        ///  A map containing lists of ceremony ids that should be retried at the block stored in the
        ///  key.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U64>> CeremonyRetryQueues(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.CeremonyRetryQueuesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U64>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> RequestRetryQueueParams
        /// </summary>
        public static string RequestRetryQueueParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "RequestRetryQueue", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> RequestRetryQueueDefault
        /// Default value as hex string
        /// </summary>
        public static string RequestRetryQueueDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> RequestRetryQueue
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>> RequestRetryQueue(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.RequestRetryQueueParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U32>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ThresholdSignatureResponseTimeoutParams
        ///  Maximum duration of a threshold signing ceremony before it is timed out and retried
        /// </summary>
        public static string ThresholdSignatureResponseTimeoutParams()
        {
            return RequestGenerator.GetStorage("EthereumThresholdSigner", "ThresholdSignatureResponseTimeout", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ThresholdSignatureResponseTimeoutDefault
        /// Default value as hex string
        /// </summary>
        public static string ThresholdSignatureResponseTimeoutDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> ThresholdSignatureResponseTimeout
        ///  Maximum duration of a threshold signing ceremony before it is timed out and retried
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> ThresholdSignatureResponseTimeout(string blockhash, CancellationToken token)
        {
            string parameters = EthereumThresholdSignerStorage.ThresholdSignatureResponseTimeoutParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> EthereumThresholdSignerCalls
    /// </summary>
    public sealed class EthereumThresholdSignerCalls
    {
        
        /// <summary>
        /// >> signature_success
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SignatureSuccess(Substrate.NetApi.Model.Types.Primitive.U64 ceremony_id, Substrate.NetApiExt.Generated.Model.cf_chains.evm.SchnorrVerificationComponents signature)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(ceremony_id.Encode());
            byteArray.AddRange(signature.Encode());
            return new Method(24, "EthereumThresholdSigner", 0, "signature_success", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> report_signature_failed
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ReportSignatureFailed(Substrate.NetApi.Model.Types.Primitive.U64 ceremony_id, Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1 offenders)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(ceremony_id.Encode());
            byteArray.AddRange(offenders.Encode());
            return new Method(24, "EthereumThresholdSigner", 1, "report_signature_failed", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_threshold_signature_timeout
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetThresholdSignatureTimeout(Substrate.NetApi.Model.Types.Primitive.U32 new_timeout)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(new_timeout.Encode());
            return new Method(24, "EthereumThresholdSigner", 2, "set_threshold_signature_timeout", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> EthereumThresholdSignerConstants
    /// </summary>
    public sealed class EthereumThresholdSignerConstants
    {
        
        /// <summary>
        /// >> CeremonyRetryDelay
        ///  In case not enough live nodes were available to begin a threshold signing ceremony: The
        ///  number of blocks to wait before retrying with a new set.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 CeremonyRetryDelay()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x01000000");
            return result;
        }
    }
    
    /// <summary>
    /// >> EthereumThresholdSignerErrors
    /// </summary>
    public enum EthereumThresholdSignerErrors
    {
        
        /// <summary>
        /// >> InvalidCeremonyId
        /// The provided ceremony id is invalid.
        /// </summary>
        InvalidCeremonyId,
        
        /// <summary>
        /// >> InvalidThresholdSignature
        /// The provided threshold signature is invalid.
        /// </summary>
        InvalidThresholdSignature,
        
        /// <summary>
        /// >> InvalidRespondent
        /// The reporting party is not one of the signatories for this ceremony, or has already
        /// responded.
        /// </summary>
        InvalidRespondent,
        
        /// <summary>
        /// >> InvalidRequestId
        /// The request Id is stale or not yet valid.
        /// </summary>
        InvalidRequestId,
    }
}
