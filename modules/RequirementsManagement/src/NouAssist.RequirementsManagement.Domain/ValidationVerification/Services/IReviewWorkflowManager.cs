namespace NouAssist.RequirementsManagement.ValidationVerification.Services
{
    /// <summary>
    /// Interface for review workflow management domain service
    /// </summary>
    public interface IReviewWorkflowManager : IDomainService
    {
        /// <summary>
        /// Creates a new review cycle for a requirement
        /// </summary>
        Task<RequirementReviewCycle> CreateReviewCycleAsync(
            string name,
            string description,
            Guid requirementId,
            WorkflowDefinitionRef workflowDefinition,
            List<ReviewerAssignment> reviewers,
            DateTime? targetCompletionDate = null,
            Guid? tenantId = null);

        /// <summary>
        /// Starts a review cycle and initializes the workflow
        /// </summary>
        Task StartReviewCycleAsync(Guid reviewCycleId, DateTime startTime);

        /// <summary>
        /// Submits a review decision for a requirement
        /// </summary>
        Task SubmitReviewDecisionAsync(
            Guid reviewCycleId,
            Guid reviewerId,
            ReviewDecisionType decisionType,
            string reason,
            string comment,
            DateTime submitTime);

        /// <summary>
        /// Gets review cycle statistics and status
        /// </summary>
        Task<ReviewCycleStatistics> GetReviewCycleStatisticsAsync(Guid reviewCycleId);

        /// <summary>
        /// Gets overdue review cycles within a date range
        /// </summary>
        Task<List<RequirementReviewCycle>> GetOverdueReviewCyclesAsync(
            DateTime currentTime,
            Guid? tenantId = null);

        /// <summary>
        /// Cancels a review cycle
        /// </summary>
        Task CancelReviewCycleAsync(Guid reviewCycleId, string reason, DateTime cancellationTime);

        /// <summary>
        /// Updates reviewer assignments for a draft review cycle
        /// </summary>
        Task UpdateReviewerAssignmentsAsync(Guid reviewCycleId, List<ReviewerAssignment> newReviewers);
    }
}
