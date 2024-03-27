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
    /// >> GovernanceStorage
    /// </summary>
    public sealed class GovernanceStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> GovernanceStorage Constructor
        /// </summary>
        public GovernanceStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "Proposals"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.Proposal)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "ActiveProposals"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.ActiveProposal>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "GovKeyWhitelistedCallHash"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Types.Base.Arr32U8)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "PreAuthorisedGovCalls"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "NextGovKeyCallHashNonce"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "ProposalIdCounter"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "ExecutionPipeline"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Model.Types.Primitive.U32>>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "ExpiryTime"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U64)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Governance", "Members"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1)));
        }
        
        /// <summary>
        /// >> ProposalsParams
        ///  Proposals.
        /// </summary>
        public static string ProposalsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("Governance", "Proposals", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> ProposalsDefault
        /// Default value as hex string
        /// </summary>
        public static string ProposalsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Proposals
        ///  Proposals.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.Proposal> Proposals(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.ProposalsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.Proposal>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ActiveProposalsParams
        ///  Active proposals.
        /// </summary>
        public static string ActiveProposalsParams()
        {
            return RequestGenerator.GetStorage("Governance", "ActiveProposals", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ActiveProposalsDefault
        /// Default value as hex string
        /// </summary>
        public static string ActiveProposalsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> ActiveProposals
        ///  Active proposals.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.ActiveProposal>> ActiveProposals(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.ActiveProposalsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.ActiveProposal>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> GovKeyWhitelistedCallHashParams
        ///  Call hash that has been committed to by the Governance Key.
        /// </summary>
        public static string GovKeyWhitelistedCallHashParams()
        {
            return RequestGenerator.GetStorage("Governance", "GovKeyWhitelistedCallHash", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> GovKeyWhitelistedCallHashDefault
        /// Default value as hex string
        /// </summary>
        public static string GovKeyWhitelistedCallHashDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> GovKeyWhitelistedCallHash
        ///  Call hash that has been committed to by the Governance Key.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.Arr32U8> GovKeyWhitelistedCallHash(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.GovKeyWhitelistedCallHashParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.Arr32U8>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> PreAuthorisedGovCallsParams
        ///  Pre authorised governance calls.
        /// </summary>
        public static string PreAuthorisedGovCallsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("Governance", "PreAuthorisedGovCalls", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> PreAuthorisedGovCallsDefault
        /// Default value as hex string
        /// </summary>
        public static string PreAuthorisedGovCallsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> PreAuthorisedGovCalls
        ///  Pre authorised governance calls.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>> PreAuthorisedGovCalls(Substrate.NetApi.Model.Types.Primitive.U32 key, string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.PreAuthorisedGovCallsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> NextGovKeyCallHashNonceParams
        ///  Any nonces before this have been consumed.
        /// </summary>
        public static string NextGovKeyCallHashNonceParams()
        {
            return RequestGenerator.GetStorage("Governance", "NextGovKeyCallHashNonce", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> NextGovKeyCallHashNonceDefault
        /// Default value as hex string
        /// </summary>
        public static string NextGovKeyCallHashNonceDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> NextGovKeyCallHashNonce
        ///  Any nonces before this have been consumed.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> NextGovKeyCallHashNonce(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.NextGovKeyCallHashNonceParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ProposalIdCounterParams
        ///  Number of proposals that have been submitted.
        /// </summary>
        public static string ProposalIdCounterParams()
        {
            return RequestGenerator.GetStorage("Governance", "ProposalIdCounter", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ProposalIdCounterDefault
        /// Default value as hex string
        /// </summary>
        public static string ProposalIdCounterDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> ProposalIdCounter
        ///  Number of proposals that have been submitted.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> ProposalIdCounter(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.ProposalIdCounterParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ExecutionPipelineParams
        ///  Pipeline of proposals which will get executed in the next block.
        /// </summary>
        public static string ExecutionPipelineParams()
        {
            return RequestGenerator.GetStorage("Governance", "ExecutionPipeline", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ExecutionPipelineDefault
        /// Default value as hex string
        /// </summary>
        public static string ExecutionPipelineDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> ExecutionPipeline
        ///  Pipeline of proposals which will get executed in the next block.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Model.Types.Primitive.U32>>> ExecutionPipeline(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.ExecutionPipelineParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Model.Types.Primitive.U32>>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> ExpiryTimeParams
        ///  Time in seconds until a proposal expires.
        /// </summary>
        public static string ExpiryTimeParams()
        {
            return RequestGenerator.GetStorage("Governance", "ExpiryTime", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ExpiryTimeDefault
        /// Default value as hex string
        /// </summary>
        public static string ExpiryTimeDefault()
        {
            return "0x0000000000000000";
        }
        
        /// <summary>
        /// >> ExpiryTime
        ///  Time in seconds until a proposal expires.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U64> ExpiryTime(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.ExpiryTimeParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U64>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> MembersParams
        ///  Accounts in the current governance set.
        /// </summary>
        public static string MembersParams()
        {
            return RequestGenerator.GetStorage("Governance", "Members", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> MembersDefault
        /// Default value as hex string
        /// </summary>
        public static string MembersDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Members
        ///  Accounts in the current governance set.
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1> Members(string blockhash, CancellationToken token)
        {
            string parameters = GovernanceStorage.MembersParams();
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Types.Base.BTreeSetT1>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> GovernanceCalls
    /// </summary>
    public sealed class GovernanceCalls
    {
        
        /// <summary>
        /// >> propose_governance_extrinsic
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ProposeGovernanceExtrinsic(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall call, Substrate.NetApiExt.Generated.Model.pallet_cf_governance.pallet.EnumExecutionMode execution)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(call.Encode());
            byteArray.AddRange(execution.Encode());
            return new Method(15, "Governance", 0, "propose_governance_extrinsic", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> new_membership_set
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method NewMembershipSet(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> accounts)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(accounts.Encode());
            return new Method(15, "Governance", 1, "new_membership_set", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> chainflip_runtime_upgrade
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ChainflipRuntimeUpgrade(Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.cf_primitives.SemVer, Substrate.NetApiExt.Generated.Model.sp_arithmetic.per_things.Percent>> cfe_version_restriction, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> code)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(cfe_version_restriction.Encode());
            byteArray.AddRange(code.Encode());
            return new Method(15, "Governance", 2, "chainflip_runtime_upgrade", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> approve
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Approve(Substrate.NetApi.Model.Types.Primitive.U32 approved_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(approved_id.Encode());
            return new Method(15, "Governance", 3, "approve", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> call_as_sudo
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method CallAsSudo(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall call)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(call.Encode());
            return new Method(15, "Governance", 4, "call_as_sudo", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_whitelisted_call_hash
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetWhitelistedCallHash(Substrate.NetApiExt.Generated.Types.Base.Arr32U8 call_hash)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(call_hash.Encode());
            return new Method(15, "Governance", 5, "set_whitelisted_call_hash", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> submit_govkey_call
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SubmitGovkeyCall(Substrate.NetApiExt.Generated.Model.state_chain_runtime.EnumRuntimeCall call)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(call.Encode());
            return new Method(15, "Governance", 6, "submit_govkey_call", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> dispatch_whitelisted_call
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method DispatchWhitelistedCall(Substrate.NetApi.Model.Types.Primitive.U32 approved_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(approved_id.Encode());
            return new Method(15, "Governance", 7, "dispatch_whitelisted_call", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> GovernanceConstants
    /// </summary>
    public sealed class GovernanceConstants
    {
    }
    
    /// <summary>
    /// >> GovernanceErrors
    /// </summary>
    public enum GovernanceErrors
    {
        
        /// <summary>
        /// >> AlreadyApproved
        /// An account already approved a proposal
        /// </summary>
        AlreadyApproved,
        
        /// <summary>
        /// >> NotMember
        /// The signer of an extrinsic is no member of the current governance
        /// </summary>
        NotMember,
        
        /// <summary>
        /// >> ProposalNotFound
        /// The proposal was not found - it may have expired or it may already be executed
        /// </summary>
        ProposalNotFound,
        
        /// <summary>
        /// >> DecodeOfCallFailed
        /// Decode of call failed
        /// </summary>
        DecodeOfCallFailed,
        
        /// <summary>
        /// >> DecodeMembersLenFailed
        /// Decoding Members len failed.
        /// </summary>
        DecodeMembersLenFailed,
        
        /// <summary>
        /// >> UpgradeConditionsNotMet
        /// A runtime upgrade has failed because the upgrade conditions were not satisfied
        /// </summary>
        UpgradeConditionsNotMet,
        
        /// <summary>
        /// >> CallHashNotWhitelisted
        /// The call hash was not whitelisted
        /// </summary>
        CallHashNotWhitelisted,
        
        /// <summary>
        /// >> NotEnoughAuthoritiesCfesAtTargetVersion
        /// Insufficient number of CFEs are at the target version to receive the runtime upgrade.
        /// </summary>
        NotEnoughAuthoritiesCfesAtTargetVersion,
    }
}