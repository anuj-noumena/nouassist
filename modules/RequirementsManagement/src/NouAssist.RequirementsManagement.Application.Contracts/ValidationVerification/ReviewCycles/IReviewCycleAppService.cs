using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles
{
    /// <summary>
    /// Application service interface for managing requirement review cycles
    /// </summary>
    public interface IReviewCycleAppService : IApplicationService
    {
        /// <summary>
        /// Gets a review cycle by ID
        /// </summary>
        Task<ReviewCycleDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged list of review cycles
        /// </summary>
        Task<PagedResultDto<ReviewCycleSummaryDto>> GetListAsync(GetReviewCycleListDto input);

        /// <summary>
        /// Creates a new review cycle
        /// </summary>
        Task<ReviewCycleDto> CreateAsync(CreateReviewCycleDto input);

        /// <summary>
        /// Updates an existing review cycle
        /// </summary>
        Task<ReviewCycleDto> UpdateAsync(Guid id, UpdateReviewCycleDto input);

        /// <summary>
        /// Deletes a review cycle
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Starts a review cycle
        /// </summary>
        Task<ReviewCycleDto> StartAsync(Guid id);

        /// <summary>
        /// Submits a review decision
        /// </summary>
        Task<ReviewCycleDto> SubmitReviewDecisionAsync(Guid id, SubmitReviewDecisionDto input);

        /// <summary>
        /// Cancels a review cycle
        /// </summary>
        Task<ReviewCycleDto> CancelAsync(Guid id, string reason);

        /// <summary>
        /// Updates reviewer assignments
        /// </summary>
        Task<ReviewCycleDto> UpdateReviewerAssignmentsAsync(Guid id, UpdateReviewerAssignmentsDto input);

        /// <summary>
        /// Gets review cycles for a specific reviewer
        /// </summary>
        Task<PagedResultDto<ReviewCycleSummaryDto>> GetReviewCyclesForReviewerAsync(
            Guid reviewerId, GetReviewCycleListDto input);

        /// <summary>
        /// Gets overdue review cycles
        /// </summary>
        Task<PagedResultDto<ReviewCycleSummaryDto>> GetOverdueReviewCyclesAsync(GetReviewCycleListDto input);

        /// <summary>
        /// Gets review cycles for a specific requirement
        /// </summary>
        Task<PagedResultDto<ReviewCycleSummaryDto>> GetReviewCyclesForRequirementAsync(
            Guid requirementId, GetReviewCycleListDto input);
    }
}
