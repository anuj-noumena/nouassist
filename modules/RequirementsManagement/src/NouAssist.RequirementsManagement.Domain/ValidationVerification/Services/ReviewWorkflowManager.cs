using Volo.Abp.Domain.Services;
using Microsoft.Extensions.Logging;

namespace NouAssist.RequirementsManagement.ValidationVerification.Services
{
    /// <summary>
    /// Domain service for managing review workflows and coordinating review cycles
    /// </summary>
    public class ReviewWorkflowManager : DomainService, IReviewWorkflowManager
    {
        private readonly IRequirementReviewCycleRepository _reviewCycleRepository;
        private readonly ILogger<ReviewWorkflowManager> _logger;

        public ReviewWorkflowManager(
            IRequirementReviewCycleRepository reviewCycleRepository,
            ILogger<ReviewWorkflowManager> logger)
        {
            _reviewCycleRepository = reviewCycleRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new review cycle for a requirement
        /// </summary>
        public async Task<RequirementReviewCycle> CreateReviewCycleAsync(
            string name,
            string description,
            Guid requirementId,
            WorkflowDefinitionRef workflowDefinition,
            List<ReviewerAssignment> reviewers,
            DateTime? targetCompletionDate = null,
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating review cycle for requirement {RequirementId}", requirementId);

            try
            {
                // Check for existing active review cycles for the same requirement
                var existingCycles = await _reviewCycleRepository.GetActiveReviewCyclesForRequirementAsync(requirementId);
                if (existingCycles.Any())
                {
                    throw new BusinessException("ValidationVerification:ErrorMessage:ActiveReviewCycleExists")
                        .WithData("requirementId", requirementId);
                }

                // Create the review cycle
                var reviewCycle = RequirementReviewCycle.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    requirementId,
                    workflowDefinition,
                    targetCompletionDate,
                    tenantId
                );

                // Assign reviewers
                reviewCycle.AssignReviewers(reviewers);

                // Persist the review cycle
                await _reviewCycleRepository.InsertAsync(reviewCycle);

                _logger.LogInformation("Successfully created review cycle {ReviewCycleId} for requirement {RequirementId}", 
                    reviewCycle.Id, requirementId);

                return reviewCycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create review cycle for requirement {RequirementId}", requirementId);
                throw;
            }
        }

        /// <summary>
        /// Starts a review cycle and initializes the workflow
        /// </summary>
        public async Task StartReviewCycleAsync(
            Guid reviewCycleId,
            DateTime startTime)
        {
            _logger.LogInformation("Starting review cycle {ReviewCycleId}", reviewCycleId);

            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(reviewCycleId);
                
                // Validate that the review cycle can be started
                if (reviewCycle.Status != ReviewCycleStatus.Draft)
                {
                    throw new ReviewCycleAlreadyStartedException(reviewCycleId);
                }

                // Start the review cycle
                reviewCycle.Start(startTime);

                // Update the review cycle in the repository
                await _reviewCycleRepository.UpdateAsync(reviewCycle);

                _logger.LogInformation("Successfully started review cycle {ReviewCycleId}", reviewCycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start review cycle {ReviewCycleId}", reviewCycleId);
                throw;
            }
        }

        /// <summary>
        /// Submits a review decision for a requirement
        /// </summary>
        public async Task SubmitReviewDecisionAsync(
            Guid reviewCycleId,
            Guid reviewerId,
            ReviewDecisionType decisionType,
            string reason,
            string comment,
            DateTime submitTime)
        {
            _logger.LogInformation("Submitting review decision for cycle {ReviewCycleId} by reviewer {ReviewerId}", 
                reviewCycleId, reviewerId);

            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(reviewCycleId);

                // Create review decision and feedback comment
                var decision = new ReviewDecision(decisionType, reason, submitTime, reviewerId);
                var feedbackComment = !string.IsNullOrEmpty(comment) 
                    ? new FeedbackComment(comment, submitTime, reviewerId, "Reviewer")
                    : null;

                // Add feedback to the review cycle
                reviewCycle.AddFeedback(reviewerId, decision, feedbackComment, submitTime);

                // Update the review cycle in the repository
                await _reviewCycleRepository.UpdateAsync(reviewCycle);

                _logger.LogInformation("Successfully submitted review decision {DecisionType} for cycle {ReviewCycleId}", 
                    decisionType, reviewCycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to submit review decision for cycle {ReviewCycleId}", reviewCycleId);
                throw;
            }
        }

        /// <summary>
        /// Gets review cycle statistics and status
        /// </summary>
        public async Task<ReviewCycleStatistics> GetReviewCycleStatisticsAsync(Guid reviewCycleId)
        {
            var reviewCycle = await _reviewCycleRepository.GetAsync(reviewCycleId);
            return reviewCycle.GetStatistics();
        }

        /// <summary>
        /// Gets overdue review cycles within a date range
        /// </summary>
        public async Task<List<RequirementReviewCycle>> GetOverdueReviewCyclesAsync(
            DateTime currentTime,
            Guid? tenantId = null)
        {
            return await _reviewCycleRepository.GetOverdueReviewCyclesAsync(currentTime, tenantId);
        }

        /// <summary>
        /// Cancels a review cycle
        /// </summary>
        public async Task CancelReviewCycleAsync(
            Guid reviewCycleId,
            string reason,
            DateTime cancellationTime)
        {
            _logger.LogInformation("Cancelling review cycle {ReviewCycleId}", reviewCycleId);

            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(reviewCycleId);
                reviewCycle.Cancel(reason, cancellationTime);

                await _reviewCycleRepository.UpdateAsync(reviewCycle);

                _logger.LogInformation("Successfully cancelled review cycle {ReviewCycleId}", reviewCycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cancel review cycle {ReviewCycleId}", reviewCycleId);
                throw;
            }
        }

        /// <summary>
        /// Updates reviewer assignments for a draft review cycle
        /// </summary>
        public async Task UpdateReviewerAssignmentsAsync(
            Guid reviewCycleId,
            List<ReviewerAssignment> newReviewers)
        {
            _logger.LogInformation("Updating reviewer assignments for cycle {ReviewCycleId}", reviewCycleId);

            try
            {
                var reviewCycle = await _reviewCycleRepository.GetAsync(reviewCycleId);
                reviewCycle.AssignReviewers(newReviewers);

                await _reviewCycleRepository.UpdateAsync(reviewCycle);

                _logger.LogInformation("Successfully updated reviewer assignments for cycle {ReviewCycleId}", reviewCycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update reviewer assignments for cycle {ReviewCycleId}", reviewCycleId);
                throw;
            }
        }
    }
}
