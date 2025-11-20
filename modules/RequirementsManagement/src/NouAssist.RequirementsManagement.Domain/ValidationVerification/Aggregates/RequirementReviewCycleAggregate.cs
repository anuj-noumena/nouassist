using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.ValidationVerification.Aggregates
{
    /// <summary>
    /// Represents a review cycle for validating requirements through a defined workflow.
    /// Manages the complete lifecycle of requirement reviews including reviewer assignments,
    /// feedback collection, and decision consolidation.
    /// </summary>
    public class RequirementReviewCycle : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid RequirementId { get; private set; }
        public ReviewCycleStatus Status { get; private set; }
        public WorkflowDefinitionRef WorkflowDefinition { get; private set; }
        public DateTime? TargetCompletionDate { get; private set; }
        public DateTime? ActualCompletionDate { get; private set; }
        public string CompletionComments { get; private set; }

        // Controlled collections
        private readonly List<ReviewerAssignment> _reviewerAssignments;
        private readonly List<ReviewFeedbackItem> _feedbackItems;

        public IReadOnlyCollection<ReviewerAssignment> ReviewerAssignments =>
            _reviewerAssignments.AsReadOnly();
        public IReadOnlyCollection<ReviewFeedbackItem> FeedbackItems =>
            _feedbackItems.AsReadOnly();

        // Navigation property for workflow instance
        public ReviewWorkflowInstance WorkflowInstance { get; private set; }

        protected RequirementReviewCycle()
        {
            _reviewerAssignments = new List<ReviewerAssignment>();
            _feedbackItems = new List<ReviewFeedbackItem>();
        }

        /// <summary>
        /// Creates a new requirement review cycle
        /// </summary>
        public static RequirementReviewCycle Create(
            Guid id,
            string name,
            string description,
            Guid requirementId,
            WorkflowDefinitionRef workflowDefinition,
            DateTime? targetCompletionDate,
            Guid? tenantId)
        {
            var reviewCycle = new RequirementReviewCycle(
                id, name, description, requirementId, 
                workflowDefinition, targetCompletionDate, tenantId);

            return reviewCycle;
        }

        private RequirementReviewCycle(
            Guid id,
            string name,
            string description,
            Guid requirementId,
            WorkflowDefinitionRef workflowDefinition,
            DateTime? targetCompletionDate,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            RequirementId = requirementId;
            WorkflowDefinition = Check.NotNull(workflowDefinition, nameof(workflowDefinition));
            SetTargetCompletionDate(targetCompletionDate);
            TenantId = tenantId;
            Status = ReviewCycleStatus.Draft;

            _reviewerAssignments = new List<ReviewerAssignment>();
            _feedbackItems = new List<ReviewFeedbackItem>();
        }

        /// <summary>
        /// Assigns reviewers to this review cycle
        /// </summary>
        public void AssignReviewers(List<ReviewerAssignment> reviewers)
        {
            if (Status != ReviewCycleStatus.Draft)
            {
                throw new ReviewCycleAlreadyStartedException(Id);
            }

            if (reviewers == null || !reviewers.Any())
            {
                throw new BusinessException("ValidationVerification:Validation:NoReviewersAssigned");
            }

            _reviewerAssignments.Clear();
            _reviewerAssignments.AddRange(reviewers);
        }

        /// <summary>
        /// Starts the review cycle and initializes the workflow
        /// </summary>
        public void Start(DateTime startTime)
        {
            if (Status != ReviewCycleStatus.Draft)
            {
                throw new ReviewCycleAlreadyStartedException(Id);
            }

            if (!_reviewerAssignments.Any())
            {
                throw new BusinessException("ValidationVerification:Validation:NoReviewersAssigned");
            }

            Status = ReviewCycleStatus.InProgress;
            WorkflowInstance = ReviewWorkflowInstance.Create(
                Guid.NewGuid(), Id, WorkflowDefinition, startTime);

            AddDistributedEvent(new ReviewCycleStartedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                RequirementId = RequirementId,
                WorkflowName = WorkflowDefinition.Name,
                ReviewerIds = _reviewerAssignments.Select(r => r.ReviewerId).ToList(),
                TargetCompletionDate = TargetCompletionDate
            });
        }

        /// <summary>
        /// Adds feedback from a reviewer
        /// </summary>
        public void AddFeedback(
            Guid reviewerId,
            ReviewDecision decision,
            FeedbackComment comment,
            DateTime submitTime)
        {
            if (Status != ReviewCycleStatus.InProgress)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:ReviewCycleNotActive");
            }

            var reviewer = _reviewerAssignments.FirstOrDefault(r => r.ReviewerId == reviewerId);
            if (reviewer == null)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:ReviewerNotAssigned");
            }

            // Check if reviewer has already provided feedback
            if (_feedbackItems.Any(f => f.ReviewerId == reviewerId))
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:ReviewAlreadyCompleted");
            }

            var feedback = ReviewFeedbackItem.Create(
                Guid.NewGuid(), Id, reviewerId, decision, comment, submitTime);

            _feedbackItems.Add(feedback);

            AddDistributedEvent(new ReviewDecisionMadeEto
            {
                Id = Id,
                TenantId = TenantId,
                ReviewCycleId = Id,
                RequirementId = RequirementId,
                ReviewerId = reviewerId,
                DecisionType = decision.DecisionType,
                Reason = decision.Reason,
                IsAllReviewsCompleted = AreAllReviewsCompleted()
            });

            // Check if all required reviews are completed
            if (AreAllReviewsCompleted())
            {
                CompleteReviewCycle(submitTime);
            }
        }

        /// <summary>
        /// Checks if all required reviews have been completed
        /// </summary>
        private bool AreAllReviewsCompleted()
        {
            var requiredReviewers = _reviewerAssignments.Where(r => r.IsRequired).ToList();
            var completedReviews = _feedbackItems.Select(f => f.ReviewerId).ToList();

            return requiredReviewers.All(r => completedReviews.Contains(r.ReviewerId));
        }

        /// <summary>
        /// Completes the review cycle and determines final status
        /// </summary>
        private void CompleteReviewCycle(DateTime completionTime)
        {
            ActualCompletionDate = completionTime;

            // Determine final status based on decisions
            var decisions = _feedbackItems.Select(f => f.Decision.DecisionType).ToList();
            var approvedCount = decisions.Count(d => d == ReviewDecisionType.Approve);
            var rejectedCount = decisions.Count(d => d == ReviewDecisionType.Reject);
            var changesRequestedCount = decisions.Count(d => d == ReviewDecisionType.RequestChanges);

            // Simple majority rule - can be made configurable based on workflow
            if (rejectedCount > 0)
            {
                Status = ReviewCycleStatus.Cancelled;
            }
            else if (changesRequestedCount > 0)
            {
                Status = ReviewCycleStatus.RequiresChanges;
            }
            else
            {
                Status = ReviewCycleStatus.Completed;
            }

            WorkflowInstance?.Complete(completionTime, Status);

            AddDistributedEvent(new ReviewCycleCompletedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                RequirementId = RequirementId,
                FinalStatus = Status,
                TotalReviewers = _reviewerAssignments.Count,
                ApprovedCount = approvedCount,
                RejectedCount = rejectedCount,
                ChangesRequestedCount = changesRequestedCount,
                CompletedAt = completionTime
            });
        }

        /// <summary>
        /// Cancels the review cycle
        /// </summary>
        public void Cancel(string reason, DateTime cancellationTime)
        {
            if (Status == ReviewCycleStatus.Completed || Status == ReviewCycleStatus.Cancelled)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:CannotCancelCompletedReview");
            }

            Status = ReviewCycleStatus.Cancelled;
            ActualCompletionDate = cancellationTime;
            CompletionComments = reason;

            WorkflowInstance?.Cancel(cancellationTime, reason);
        }

        /// <summary>
        /// Updates the review cycle name
        /// </summary>
        public void UpdateName(string name)
        {
            if (Status != ReviewCycleStatus.Draft)
            {
                throw new ReviewCycleAlreadyStartedException(Id);
            }

            SetName(name);
        }

        /// <summary>
        /// Updates the review cycle description
        /// </summary>
        public void UpdateDescription(string description)
        {
            if (Status != ReviewCycleStatus.Draft)
            {
                throw new ReviewCycleAlreadyStartedException(Id);
            }

            SetDescription(description);
        }

        /// <summary>
        /// Updates the target completion date
        /// </summary>
        public void UpdateTargetCompletionDate(DateTime? targetDate)
        {
            if (Status == ReviewCycleStatus.Completed || Status == ReviewCycleStatus.Cancelled)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:CannotModifyCompletedReview");
            }

            SetTargetCompletionDate(targetDate);
        }

        /// <summary>
        /// Gets review statistics
        /// </summary>
        public ReviewCycleStatistics GetStatistics()
        {
            return new ReviewCycleStatistics
            {
                TotalReviewers = _reviewerAssignments.Count,
                RequiredReviewers = _reviewerAssignments.Count(r => r.IsRequired),
                CompletedReviews = _feedbackItems.Count,
                PendingReviews = _reviewerAssignments.Count(r => r.IsRequired) - _feedbackItems.Count,
                ApprovedCount = _feedbackItems.Count(f => f.Decision.DecisionType == ReviewDecisionType.Approve),
                RejectedCount = _feedbackItems.Count(f => f.Decision.DecisionType == ReviewDecisionType.Reject),
                ChangesRequestedCount = _feedbackItems.Count(f => f.Decision.DecisionType == ReviewDecisionType.RequestChanges),
                OverdueReviews = GetOverdueReviewCount(DateTime.UtcNow)
            };
        }

        /// <summary>
        /// Gets count of overdue reviews
        /// </summary>
        private int GetOverdueReviewCount(DateTime currentTime)
        {
            var completedReviewerIds = _feedbackItems.Select(f => f.ReviewerId).ToHashSet();
            return _reviewerAssignments
                .Where(r => r.IsRequired && !completedReviewerIds.Contains(r.ReviewerId))
                .Count(r => r.IsOverdue(currentTime));
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ValidationVerificationConsts.RequirementReviewCycle.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ValidationVerificationConsts.RequirementReviewCycle.MaxDescriptionLength
            );
        }

        private void SetTargetCompletionDate(DateTime? targetDate)
        {
            if (targetDate.HasValue && targetDate.Value <= DateTime.UtcNow)
            {
                throw new BusinessException("ValidationVerification:Validation:InvalidDateRange");
            }

            TargetCompletionDate = targetDate;
        }
    }

    /// <summary>
    /// Review cycle statistics data structure
    /// </summary>
    public class ReviewCycleStatistics
    {
        public int TotalReviewers { get; set; }
        public int RequiredReviewers { get; set; }
        public int CompletedReviews { get; set; }
        public int PendingReviews { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int ChangesRequestedCount { get; set; }
        public int OverdueReviews { get; set; }
    }
}
