using System;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.ValidationVerification.Entities
{
    /// <summary>
    /// Represents an instance of a review workflow for a specific review cycle
    /// </summary>
    public class ReviewWorkflowInstance : Entity<Guid>
    {
        public Guid ReviewCycleId { get; private set; }
        public WorkflowDefinitionRef WorkflowDefinition { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public ReviewCycleStatus? FinalStatus { get; private set; }
        public string CompletionReason { get; private set; }
        public string WorkflowState { get; private set; }
        public string WorkflowContext { get; private set; }

        protected ReviewWorkflowInstance() { }

        internal static ReviewWorkflowInstance Create(
            Guid id,
            Guid reviewCycleId,
            WorkflowDefinitionRef workflowDefinition,
            DateTime startTime)
        {
            return new ReviewWorkflowInstance(id, reviewCycleId, workflowDefinition, startTime);
        }

        private ReviewWorkflowInstance(
            Guid id,
            Guid reviewCycleId,
            WorkflowDefinitionRef workflowDefinition,
            DateTime startTime) : base(id)
        {
            ReviewCycleId = reviewCycleId;
            WorkflowDefinition = Check.NotNull(workflowDefinition, nameof(workflowDefinition));
            StartedAt = startTime;
            WorkflowState = "STARTED";
        }

        internal void UpdateState(string state, string context = null)
        {
            WorkflowState = Check.NotNullOrWhiteSpace(state, nameof(state));
            WorkflowContext = context;
        }

        internal void Complete(DateTime completionTime, ReviewCycleStatus finalStatus, string reason = null)
        {
            if (CompletedAt.HasValue)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:WorkflowAlreadyCompleted");
            }

            CompletedAt = completionTime;
            FinalStatus = finalStatus;
            CompletionReason = reason;
            WorkflowState = "COMPLETED";
        }

        internal void Cancel(DateTime cancellationTime, string reason)
        {
            if (CompletedAt.HasValue)
            {
                throw new BusinessException("ValidationVerification:ErrorMessage:WorkflowAlreadyCompleted");
            }

            CompletedAt = cancellationTime;
            FinalStatus = ReviewCycleStatus.Cancelled;
            CompletionReason = reason;
            WorkflowState = "CANCELLED";
        }

        public bool IsActive()
        {
            return !CompletedAt.HasValue;
        }

        public TimeSpan? GetDuration()
        {
            if (CompletedAt.HasValue)
            {
                return CompletedAt.Value - StartedAt;
            }
            return null;
        }
    }
}
