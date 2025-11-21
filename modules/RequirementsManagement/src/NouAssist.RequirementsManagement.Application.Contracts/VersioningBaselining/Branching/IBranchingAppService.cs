using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching
{
    /// <summary>
    /// Application service for managing branches and merges
    /// </summary>
    public interface IBranchingAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new branch
        /// </summary>
        Task<BranchDto> CreateAsync(CreateBranchDto input);
        
        /// <summary>
        /// Updates an existing branch
        /// </summary>
        Task<BranchDto> UpdateAsync(Guid id, UpdateBranchDto input);
        
        /// <summary>
        /// Gets a branch by ID
        /// </summary>
        Task<BranchDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of branches
        /// </summary>
        Task<PagedResultDto<BranchDto>> GetListAsync(GetBranchListDto input);
        
        /// <summary>
        /// Abandons a branch
        /// </summary>
        Task<BranchDto> AbandonAsync(Guid id);
        
        /// <summary>
        /// Initiates a merge from a source branch to a target branch
        /// </summary>
        Task<MergeOperationDto> InitiateMergeAsync(Guid sourceBranchId, InitiateMergeDto input);
        
        /// <summary>
        /// Completes a merge operation
        /// </summary>
        Task<MergeOperationDto> CompleteMergeAsync(Guid mergeOperationId, CompleteMergeDto input);
        
        /// <summary>
        /// Aborts a merge operation
        /// </summary>
        Task<MergeOperationDto> AbortMergeAsync(Guid mergeOperationId, AbortMergeDto input);
    }
}
