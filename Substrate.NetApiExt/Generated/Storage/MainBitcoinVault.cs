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
    /// >> BitcoinVaultStorage
    /// </summary>
    public sealed class BitcoinVaultStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> BitcoinVaultStorage Constructor
        /// </summary>
        public BitcoinVaultStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "Vaults"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.VaultT3)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "CurrentVaultEpoch"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "PendingVaultRotation"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.EnumVaultRotationStatus)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeygenSuccessVoters"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Identity}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeygenFailureVoters"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeyHandoverSuccessVoters"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Identity}, typeof(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeyHandoverFailureVoters"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeygenResolutionPendingSince"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeyHandoverResolutionPendingSince"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeygenResponseTimeout"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "KeygenSlashAmount"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("BitcoinVault", "CeremonyIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
        }
        
        /// <summary>
        /// >> VaultsParams
        ///  A map of vaults by epoch.
        /// </summary>
        public static string VaultsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("BitcoinVault", "Vaults", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> VaultsDefault
        /// Default value as hex string
        /// </summary>
        public static string VaultsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Vaults
        ///  A map of vaults by epoch.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.VaultT3> Vaults(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.VaultsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.VaultT3>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> CurrentVaultEpochParams
        ///  The epoch whose authorities control the current vault key.
        /// </summary>
        public static string CurrentVaultEpochParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "CurrentVaultEpoch", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> CurrentVaultEpochDefault
        /// Default value as hex string
        /// </summary>
        public static string CurrentVaultEpochDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> CurrentVaultEpoch
        ///  The epoch whose authorities control the current vault key.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> CurrentVaultEpoch(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.CurrentVaultEpochParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingVaultRotationParams
        ///  Vault rotation statuses for the current epoch rotation.
        /// </summary>
        public static string PendingVaultRotationParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "PendingVaultRotation", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> PendingVaultRotationDefault
        /// Default value as hex string
        /// </summary>
        public static string PendingVaultRotationDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PendingVaultRotation
        ///  Vault rotation statuses for the current epoch rotation.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.EnumVaultRotationStatus> PendingVaultRotation(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.PendingVaultRotationParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.EnumVaultRotationStatus>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeygenSuccessVotersParams
        ///  The voters who voted for success for a particular agg key rotation
        /// </summary>
        public static string KeygenSuccessVotersParams(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey key)
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeygenSuccessVoters", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Identity}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> KeygenSuccessVotersDefault
        /// Default value as hex string
        /// </summary>
        public static string KeygenSuccessVotersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> KeygenSuccessVoters
        ///  The voters who voted for success for a particular agg key rotation
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>> KeygenSuccessVoters(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeygenSuccessVotersParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeygenFailureVotersParams
        ///  The voters who voted for failure for a particular agg key rotation
        /// </summary>
        public static string KeygenFailureVotersParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeygenFailureVoters", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> KeygenFailureVotersDefault
        /// Default value as hex string
        /// </summary>
        public static string KeygenFailureVotersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> KeygenFailureVoters
        ///  The voters who voted for failure for a particular agg key rotation
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>> KeygenFailureVoters(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeygenFailureVotersParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeyHandoverSuccessVotersParams
        ///  The voters who voted for success for a particular key handover ceremony
        /// </summary>
        public static string KeyHandoverSuccessVotersParams(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey key)
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeyHandoverSuccessVoters", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Identity}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> KeyHandoverSuccessVotersDefault
        /// Default value as hex string
        /// </summary>
        public static string KeyHandoverSuccessVotersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> KeyHandoverSuccessVoters
        ///  The voters who voted for success for a particular key handover ceremony
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>> KeyHandoverSuccessVoters(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey key, string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeyHandoverSuccessVotersParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeyHandoverFailureVotersParams
        ///  The voters who voted for failure for a particular key handover ceremony
        /// </summary>
        public static string KeyHandoverFailureVotersParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeyHandoverFailureVoters", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> KeyHandoverFailureVotersDefault
        /// Default value as hex string
        /// </summary>
        public static string KeyHandoverFailureVotersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> KeyHandoverFailureVoters
        ///  The voters who voted for failure for a particular key handover ceremony
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>> KeyHandoverFailureVoters(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeyHandoverFailureVotersParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeygenResolutionPendingSinceParams
        ///  The block since which we have been waiting for keygen to be resolved.
        /// </summary>
        public static string KeygenResolutionPendingSinceParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeygenResolutionPendingSince", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> KeygenResolutionPendingSinceDefault
        /// Default value as hex string
        /// </summary>
        public static string KeygenResolutionPendingSinceDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> KeygenResolutionPendingSince
        ///  The block since which we have been waiting for keygen to be resolved.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> KeygenResolutionPendingSince(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeygenResolutionPendingSinceParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeyHandoverResolutionPendingSinceParams
        ///  The block since which we have been waiting for key handover to be resolved.
        /// </summary>
        public static string KeyHandoverResolutionPendingSinceParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeyHandoverResolutionPendingSince", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> KeyHandoverResolutionPendingSinceDefault
        /// Default value as hex string
        /// </summary>
        public static string KeyHandoverResolutionPendingSinceDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> KeyHandoverResolutionPendingSince
        ///  The block since which we have been waiting for key handover to be resolved.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> KeyHandoverResolutionPendingSince(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeyHandoverResolutionPendingSinceParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeygenResponseTimeoutParams
        /// </summary>
        public static string KeygenResponseTimeoutParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeygenResponseTimeout", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> KeygenResponseTimeoutDefault
        /// Default value as hex string
        /// </summary>
        public static string KeygenResponseTimeoutDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> KeygenResponseTimeout
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> KeygenResponseTimeout(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeygenResponseTimeoutParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> KeygenSlashAmountParams
        ///  The amount of FLIP that is slashed for an agreed reported party expressed in Flipperinos
        ///  (2/3 must agree the node was an offender) on keygen failure.
        /// </summary>
        public static string KeygenSlashAmountParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "KeygenSlashAmount", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> KeygenSlashAmountDefault
        /// Default value as hex string
        /// </summary>
        public static string KeygenSlashAmountDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> KeygenSlashAmount
        ///  The amount of FLIP that is slashed for an agreed reported party expressed in Flipperinos
        ///  (2/3 must agree the node was an offender) on keygen failure.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> KeygenSlashAmount(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.KeygenSlashAmountParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> CeremonyIdCounterParams
        ///  Counter for generating unique ceremony ids.
        /// </summary>
        public static string CeremonyIdCounterParams()
        {
            return RequestGenerator.GetStorage("BitcoinVault", "CeremonyIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> CeremonyIdCounterDefault
        /// Default value as hex string
        /// </summary>
        public static string CeremonyIdCounterDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> CeremonyIdCounter
        ///  Counter for generating unique ceremony ids.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> CeremonyIdCounter(string blockhash, CancellationToken token)
        {
            string parameters = BitcoinVaultStorage.CeremonyIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> BitcoinVaultCalls
    /// </summary>
    public sealed class BitcoinVaultCalls
    {
        
        /// <summary>
        /// >> report_keygen_outcome
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ReportKeygenOutcome(Substrate.NetApi.Model.Types.Primitive.U64 ceremony_id, Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult reported_outcome)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(ceremony_id.Encode());
            byteArray.AddRange(reported_outcome.Encode());
            return new Method(23, "BitcoinVault", 0, "report_keygen_outcome", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> report_key_handover_outcome
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ReportKeyHandoverOutcome(Substrate.NetApi.Model.Types.Primitive.U64 ceremony_id, Substrate.NetApiExt.Generated.Types.Base.EnumDispatchResult reported_outcome)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(ceremony_id.Encode());
            byteArray.AddRange(reported_outcome.Encode());
            return new Method(23, "BitcoinVault", 1, "report_key_handover_outcome", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> on_keygen_verification_result
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method OnKeygenVerificationResult(Substrate.NetApi.Model.Types.Primitive.U64 keygen_ceremony_id, Substrate.NetApi.Model.Types.Primitive.U32 threshold_request_id, Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey new_public_key)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(keygen_ceremony_id.Encode());
            byteArray.AddRange(threshold_request_id.Encode());
            byteArray.AddRange(new_public_key.Encode());
            return new Method(23, "BitcoinVault", 2, "on_keygen_verification_result", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> on_handover_verification_result
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method OnHandoverVerificationResult(Substrate.NetApi.Model.Types.Primitive.U64 handover_ceremony_id, Substrate.NetApi.Model.Types.Primitive.U32 threshold_request_id, Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey new_public_key)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(handover_ceremony_id.Encode());
            byteArray.AddRange(threshold_request_id.Encode());
            byteArray.AddRange(new_public_key.Encode());
            return new Method(23, "BitcoinVault", 7, "on_handover_verification_result", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> vault_key_rotated
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method VaultKeyRotated(Substrate.NetApi.Model.Types.Primitive.U64 block_number, Substrate.NetApiExt.Generated.Types.Base.Arr32U8 tx_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(block_number.Encode());
            byteArray.AddRange(tx_id.Encode());
            return new Method(23, "BitcoinVault", 3, "vault_key_rotated", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> vault_key_rotated_externally
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method VaultKeyRotatedExternally(Substrate.NetApiExt.Generated.Model.cf_chains.btc.AggKey new_public_key, Substrate.NetApi.Model.Types.Primitive.U64 block_number, Substrate.NetApiExt.Generated.Types.Base.Arr32U8 tx_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(new_public_key.Encode());
            byteArray.AddRange(block_number.Encode());
            byteArray.AddRange(tx_id.Encode());
            return new Method(23, "BitcoinVault", 4, "vault_key_rotated_externally", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_keygen_response_timeout
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetKeygenResponseTimeout(Substrate.NetApi.Model.Types.Primitive.U32 new_timeout)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(new_timeout.Encode());
            return new Method(23, "BitcoinVault", 5, "set_keygen_response_timeout", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_keygen_slash_amount
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetKeygenSlashAmount(Substrate.NetApi.Model.Types.Primitive.U128 amount_to_slash)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(amount_to_slash.Encode());
            return new Method(23, "BitcoinVault", 6, "set_keygen_slash_amount", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> BitcoinVaultConstants
    /// </summary>
    public sealed class BitcoinVaultConstants
    {
    }
    
    /// <summary>
    /// >> BitcoinVaultErrors
    /// </summary>
    public enum BitcoinVaultErrors
    {
        
        /// <summary>
        /// >> InvalidCeremonyId
        /// An invalid ceremony id
        /// </summary>
        InvalidCeremonyId,
        
        /// <summary>
        /// >> NoActiveRotation
        /// There is currently no vault rotation in progress for this chain.
        /// </summary>
        NoActiveRotation,
        
        /// <summary>
        /// >> InvalidRotationStatus
        /// The requested call is invalid based on the current rotation state.
        /// </summary>
        InvalidRotationStatus,
        
        /// <summary>
        /// >> InvalidRespondent
        /// An authority sent a response for a ceremony in which they weren't involved, or to which
        /// they have already submitted a response.
        /// </summary>
        InvalidRespondent,
        
        /// <summary>
        /// >> ThresholdSignatureUnavailable
        /// There is no threshold signature available
        /// </summary>
        ThresholdSignatureUnavailable,
    }
}
