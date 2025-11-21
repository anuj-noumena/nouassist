using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories
{
    /// <summary>
    /// Repository interface for PrioritizationActivity aggregate
    /// </summary>
    public interface IPrioritizationActivityRepository : IRepository<PrioritizationActivity, Guid>
    {
        /// <summary>
        /// Gets activities matching the given specification
        /// </summary>
        Task<List<PrioritizationActivity>> GetListAsync(
            Specification<PrioritizationActivity> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets activities by status
        /// </summary>
        Task<List<PrioritizationActivity>> GetByStatusAsync(
            ActivityStatus status,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets activities where a stakeholder is participating
        /// </summary>
        Task<List<PrioritizationActivity>> GetByParticipantAsync(
            Guid stakeholderId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets activities that contain a specific requirement
        /// </summary>
        Task<List<PrioritizationActivity>> GetByRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets activities by prioritization method
        /// </summary>
        Task<List<PrioritizationActivity>> GetByMethodAsync(
            PrioritizationMethod method,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets active activities (In Progress or Voting)
        /// </summary>
        Task<List<PrioritizationActivity>> GetActiveActivitiesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets completed activities within a date range
        /// </summary>
        Task<List<PrioritizationActivity>> GetCompletedActivitiesAsync(
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets overdue activities
        /// </summary>
        Task<List<PrioritizationActivity>> GetOverdueActivitiesAsync(
            DateTime currentDate,
            CancellationToken cancellationToken = default);
    }
}
