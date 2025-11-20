using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities
{
    /// <summary>
    /// Application service for managing prioritization activities
    /// </summary>
    public interface IPrioritizationActivityAppService : IApplicationService
    {
        /// <summary>
        /// Gets a prioritization activity by ID
        /// </summary>
        Task<PrioritizationActivityDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a list of prioritization activities with filtering
        /// </summary>
        Task<PagedResultDto<PrioritizationActivityDto>> GetListAsync(GetPrioritizationActivityListDto input);

        /// <summary>
        /// Creates a new prioritization activity
        /// </summary>
        Task<PrioritizationActivityDto> CreateAsync(CreatePrioritizationActivityDto input);

        /// <summary>
        /// Updates a prioritization activity
        /// </summary>
        Task<PrioritizationActivityDto> UpdateAsync(Guid id, UpdatePrioritizationActivityDto input);

        /// <summary>
        /// Deletes a prioritization activity
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Starts a prioritization activity
        /// </summary>
        Task StartAsync(Guid id);

        /// <summary>
        /// Starts voting phase of an activity
        /// </summary>
        Task StartVotingAsync(Guid id);

        /// <summary>
        /// Completes a prioritization activity
        /// </summary>
        Task CompleteAsync(Guid id);

        /// <summary>
        /// Cancels a prioritization activity
        /// </summary>
        Task CancelAsync(Guid id);

        /// <summary>
        /// Adds participants to an activity
        /// </summary>
        Task AddParticipantsAsync(Guid id, ManageParticipantsDto input);

        /// <summary>
        /// Removes participants from an activity
        /// </summary>
        Task RemoveParticipantsAsync(Guid id, ManageParticipantsDto input);

        /// <summary>
        /// Adds requirements to an activity
        /// </summary>
        Task AddRequirementsAsync(Guid id, ManageRequirementsDto input);

        /// <summary>
        /// Removes requirements from an activity
        /// </summary>
        Task RemoveRequirementsAsync(Guid id, ManageRequirementsDto input);

        /// <summary>
        /// Submits a vote for a requirement
        /// </summary>
        Task SubmitVoteAsync(Guid id, SubmitVoteDto input);

        /// <summary>
        /// Gets prioritization results
        /// </summary>
        Task<PrioritizationResultDto> GetResultsAsync(Guid id);

        /// <summary>
        /// Gets activities where current user is a participant
        /// </summary>
        Task<PagedResultDto<PrioritizationActivityDto>> GetMyActivitiesAsync(
            PagedAndSortedResultRequestDto input);
    }
}
