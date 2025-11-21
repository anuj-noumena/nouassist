using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces
{
    /// <summary>
    /// Interface for the branching and merging domain service
    /// </summary>
    public interface IBranchingAndMergingService : IDomainService
    {
        /// <summary>
        /// Creates a new branch from a baseline
        /// </summary>
        Task<RequirementBranch> CreateBranchFromBaselineAsync(
            string branchName,
            string description,
            Guid baselineId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates a new branch from another branch
        /// </summary>
        Task<RequirementBranch> CreateBranchFromBranchAsync(
            string branchName,
            string description,
            Guid sourceBranchId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Initiates a merge operation from a source branch to a target branch
        /// </summary>
        Task<MergeOperation> InitiateMergeAsync(
            Guid sourceBranchId,
            Guid targetBranchId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Completes a merge operation
        /// </summary>
        Task<MergeOperation> CompleteMergeAsync(
            Guid mergeOperationId,
            MergeResult mergeResult,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Aborts a merge operation
        /// </summary>
        Task<MergeOperation> AbortMergeAsync(
            Guid mergeOperationId,
            string reason,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Abandons a branch
        /// </summary>
        Task AbandonBranchAsync(
            Guid branchId,
            CancellationToken cancellationToken = default);
    }
}
