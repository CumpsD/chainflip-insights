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
    /// >> ArbitrumVaultStorage
    /// </summary>
    public sealed class ArbitrumVaultStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> ArbitrumVaultStorage Constructor
        /// </summary>
        public ArbitrumVaultStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("ArbitrumVault", "VaultStartBlockNumbers"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("ArbitrumVault", "PendingVaultActivation"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.EnumVaultActivationStatus)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("ArbitrumVault", "ChainInitialized"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.Bool)));
        }
        
        /// <summary>
        /// >> VaultStartBlockNumbersParams
        ///  A map of starting block number of vaults by epoch.
        /// </summary>
        public static string VaultStartBlockNumbersParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("ArbitrumVault", "VaultStartBlockNumbers", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> VaultStartBlockNumbersDefault
        /// Default value as hex string
        /// </summary>
        public static string VaultStartBlockNumbersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> VaultStartBlockNumbers
        ///  A map of starting block number of vaults by epoch.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> VaultStartBlockNumbers(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = ArbitrumVaultStorage.VaultStartBlockNumbersParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PendingVaultActivationParams
        ///  Vault activation status for the current epoch rotation.
        /// </summary>
        public static string PendingVaultActivationParams()
        {
            return RequestGenerator.GetStorage("ArbitrumVault", "PendingVaultActivation", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> PendingVaultActivationDefault
        /// Default value as hex string
        /// </summary>
        public static string PendingVaultActivationDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PendingVaultActivation
        ///  Vault activation status for the current epoch rotation.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.EnumVaultActivationStatus> PendingVaultActivation(string blockhash, CancellationToken token)
        {
            string parameters = ArbitrumVaultStorage.PendingVaultActivationParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_vaults.EnumVaultActivationStatus>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ChainInitializedParams
        ///  Whether this chain is initialized.
        /// </summary>
        public static string ChainInitializedParams()
        {
            return RequestGenerator.GetStorage("ArbitrumVault", "ChainInitialized", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ChainInitializedDefault
        /// Default value as hex string
        /// </summary>
        public static string ChainInitializedDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> ChainInitialized
        ///  Whether this chain is initialized.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.Bool> ChainInitialized(string blockhash, CancellationToken token)
        {
            string parameters = ArbitrumVaultStorage.ChainInitializedParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.Bool>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> ArbitrumVaultCalls
    /// </summary>
    public sealed class ArbitrumVaultCalls
    {
        
        /// <summary>
        /// >> vault_key_rotated_externally
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method VaultKeyRotatedExternally(Substrate.NetApiExt.Generated.Model.cf_chains.evm.AggKey new_public_key, Substrate.NetApi.Model.Types.Primitive.U64 block_number, Substrate.NetApiExt.Generated.Model.primitive_types.H256 tx_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(new_public_key.Encode());
            byteArray.AddRange(block_number.Encode());
            byteArray.AddRange(tx_id.Encode());
            return new Method(38, "ArbitrumVault", 4, "vault_key_rotated_externally", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> initialize_chain
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method InitializeChain()
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            return new Method(38, "ArbitrumVault", 5, "initialize_chain", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> ArbitrumVaultConstants
    /// </summary>
    public sealed class ArbitrumVaultConstants
    {
    }
    
    /// <summary>
    /// >> ArbitrumVaultErrors
    /// </summary>
    public enum ArbitrumVaultErrors
    {
        
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
    }
}
