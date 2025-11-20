using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.ValidationVerification.Repositories
{
    /// <summary>
    /// Repository for RequirementReviewCycle aggregate
    /// </summary>
    public interface IRequirementReviewCycleRepository : IRepository<RequirementReviewCycle, Guid>
    {
        /// <summary>
        /// Gets active review cycles for a specific requirement
        /// </summary>
        Task<List<RequirementReviewCycle>> GetActiveReviewCyclesForRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets review cycles by status
        /// </summary>
        Task<List<RequirementReviewCycle>> GetReviewCyclesByStatusAsync(
            ReviewCycleStatus status,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets overdue review cycles
        /// </summary>
        Task<List<RequirementReviewCycle>> GetOverdueReviewCyclesAsync(
            DateTime currentTime,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets review cycles assigned to a specific reviewer
        /// </summary>
        Task<List<RequirementReviewCycle>> GetReviewCyclesForReviewerAsync(
            Guid reviewerId,
            ReviewCycleStatus? status = null,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets review cycles within a date range
        /// </summary>
        Task<List<RequirementReviewCycle>> GetReviewCyclesInDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets review cycle statistics for requirements
        /// </summary>
        Task<List<RequirementReviewSummary>> GetRequirementReviewSummariesAsync(
            List<Guid> requirementIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts review cycles by criteria
        /// </summary>
        Task<Dictionary<ReviewCycleStatus, int>> GetReviewCycleCountsByStatusAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Summary information for requirement reviews
    /// </summary>
    public class RequirementReviewSummary
    {
        public Guid RequirementId { get; set; }
        public int TotalReviewCycles { get; set; }
        public int CompletedReviewCycles { get; set; }
        public int InProgressReviewCycles { get; set; }
        public DateTime? LastReviewDate { get; set; }
        public ReviewCycleStatus? LastReviewStatus { get; set; }
        public double AverageReviewDuration { get; set; }
    }
}
