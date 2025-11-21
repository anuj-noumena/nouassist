using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;

namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for managing freelance projects
    /// </summary>
    public interface IFreelanceProjectManager
    {
        /// <summary>
        /// Creates a new freelance project with hourly billing
        /// </summary>
        Task<FreelanceProject> CreateHourlyProjectAsync(
            string name,
            Guid clientId,
            HourlyRate hourlyRate,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null);
        
        /// <summary>
        /// Creates a new freelance project with fixed price billing
        /// </summary>
        Task<FreelanceProject> CreateFixedPriceProjectAsync(
            string name,
            Guid clientId,
            decimal fixedPrice,
            string currency,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null);
        
        /// <summary>
        /// Creates a new freelance project with milestone-based billing
        /// </summary>
        Task<FreelanceProject> CreateMilestoneProjectAsync(
            string name,
            Guid clientId,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null);
        
        /// <summary>
        /// Adds a time entry to a project
        /// </summary>
        Task<TimeEntry> AddTimeEntryAsync(
            Guid projectId,
            DateTime date,
            TimeSpan startTime,
            int durationMinutes,
            string description,
            bool isBillable = true);
        
        /// <summary>
        /// Adds a milestone to a project
        /// </summary>
        Task<ProjectMilestone> AddMilestoneAsync(
            Guid projectId,
            MilestoneDetails details,
            DateTime? deadline = null);
        
        /// <summary>
        /// Changes the status of a project
        /// </summary>
        Task ChangeProjectStatusAsync(
            Guid projectId,
            ProjectStatus newStatus);
        
        /// <summary>
        /// Links a project to an accepted proposal
        /// </summary>
        Task LinkProjectToProposalAsync(
            Guid projectId,
            Guid proposalId);
        
        /// <summary>
        /// Calculates the total amount due for a project
        /// </summary>
        Task<decimal> CalculateTotalAmountAsync(Guid projectId);
    }
}
