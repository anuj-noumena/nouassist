using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Entities;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Events;
using NouAssist.SharedKernel.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Aggregates
{
    /// <summary>
    /// Represents an approval request initiated against a specific entity (e.g., document, task, purchase order),
    /// which progresses through workflow steps until approved, rejected, or cancelled.
    /// </summary>
    public class ApprovalRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Identifier of the workflow definition associated with this request.
        /// </summary>
        public Guid WorkflowId { get; private set; }

        /// <summary>
        /// The type of the entity under approval (e.g., "Invoice", "Task", "PurchaseOrder").
        /// </summary>
        public string EntityType { get; private set; }

        /// <summary>
        /// The unique identifier of the specific entity instance being approved.
        /// </summary>
        public Guid EntityId { get; private set; }

        /// <summary>
        /// The user who initiated or submitted this approval request.
        /// </summary>
        public Guid RequestedBy { get; private set; }

        /// <summary>
        /// The display title or subject of the approval request.
        /// Useful for dashboards and notifications.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Optional detailed description providing additional context for the approval.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Current approval status of the request (e.g., Pending, InProgress, Approved, Rejected, Cancelled).
        /// </summary>
        public ApprovalStatus Status { get; private set; }

        /// <summary>
        /// Identifier of the current workflow step being processed.
        /// Null when not yet started or already completed.
        /// </summary>
        public Guid? CurrentStepId { get; private set; }

        /// <summary>
        /// The UTC timestamp when the request was initially submitted.
        /// </summary>
        public DateTime SubmittedAt { get; private set; }

        /// <summary>
        /// The UTC timestamp when the request was completed (approved, rejected, or otherwise finalized).
        /// </summary>
        public DateTime? CompletedAt { get; private set; }

        /// <summary>
        /// The UTC timestamp when the request was cancelled, if applicable.
        /// </summary>
        public DateTime? CancelledAt { get; private set; }

        /// <summary>
        /// The user who cancelled this request, if cancellation occurred.
        /// </summary>
        public Guid? CancelledBy { get; private set; }

        /// <summary>
        /// Textual reason explaining why the request was cancelled.
        /// </summary>
        public string CancellationReason { get; private set; }

        /// <summary>
        /// Indicates the priority level of this request (0–10).
        /// Higher values can signal urgent or high-importance approvals.
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// The deadline by which the request is expected to be processed or acted upon.
        /// Used to determine overdue or time-sensitive requests.
        /// </summary>
        public DateTime? DueDate { get; private set; }

        /// <summary>
        /// A dictionary of dynamic, user-defined key–value pairs 
        /// representing additional contextual data (e.g., “Department”: “Finance”).
        /// </summary>
        public CustomFieldValues CustomFields { get; internal set; }

        /// <summary>
        /// Arbitrary metadata for internal system or integration use, 
        /// such as source system identifiers or audit tags.
        /// </summary>
        public Metadata Metadata { get; internal set; }

        /// <summary>
        /// Identifier of the tenant to which this approval request belongs.
        /// Enables multi-tenant isolation.
        /// </summary>
        public Guid? TenantId { get; private set; }

        /// <summary>
        /// List of workflow step IDs that have been completed for this request.
        /// Maintains progression state.
        /// </summary>
        private readonly List<Guid> _completedStepIds = new();

        /// <summary>
        /// Read-only collection of all completed step identifiers in this request.
        /// </summary>
        public IReadOnlyCollection<Guid> CompletedStepIds => _completedStepIds.AsReadOnly();

        /// <summary>
        /// Collection of approval decisions made throughout the request’s lifecycle.
        /// </summary>
        private readonly List<ApprovalDecision> _decisions = new();

        /// <summary>
        /// Read-only collection of all recorded decisions (approvals/rejections) in this request.
        /// </summary>
        public IReadOnlyCollection<ApprovalDecision> Decisions => _decisions.AsReadOnly();

        /// <summary>
        /// Protected constructor required by ORM frameworks.
        /// </summary>
        protected ApprovalRequest() { /* ORM */ }

        /// <summary>
        /// Creates a new approval request linked to a specific workflow and entity.
        /// </summary>
        public ApprovalRequest(
            Guid id,
            Guid workflowId,
            string entityType,
            Guid entityId,
            Guid requestedBy,
            Guid? tenantId,
            string title = null,
            string description = null
        ) : base(id)
        {
            WorkflowId = workflowId;
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = entityId;
            RequestedBy = requestedBy;
            TenantId = tenantId;
            Title = title;
            Description = description;
            Status = ApprovalStatus.Pending;
            SubmittedAt = DateTime.UtcNow;
            Priority = 0;
            CustomFields = CustomFieldValues.Create();
            Metadata = Metadata.Create();
            
            // Emit domain event for request creation
            AddDistributedEvent(new ApprovalRequestCreatedEto
            {
                RequestId = Id,
                WorkflowId = workflowId,
                EntityType = entityType,
                EntityId = entityId,
                RequestedBy = requestedBy,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        #region Derived State Properties

        /// <summary> Returns true if the request is in "Pending" status. </summary>
        public bool IsPending => Status == ApprovalStatus.Pending;

        /// <summary> Returns true if the request is currently being processed (InProgress). </summary>
        public bool IsInProgress => Status == ApprovalStatus.InProgress;

        /// <summary> Returns true if the request has been approved. </summary>
        public bool IsApproved => Status == ApprovalStatus.Approved;

        /// <summary> Returns true if the request has been rejected. </summary>
        public bool IsRejected => Status == ApprovalStatus.Rejected;

        /// <summary> Returns true if the request has been cancelled. </summary>
        public bool IsCancelled => Status == ApprovalStatus.Cancelled;

        /// <summary> Returns true if the request has reached any terminal state (Approved, Rejected, Cancelled). </summary>
        public bool IsComplete => IsApproved || IsRejected || IsCancelled;

        /// <summary>
        /// Indicates whether the request is past its due date and still incomplete.
        /// </summary>
        public bool IsOverdue => DueDate.HasValue &&
                                 DateTime.UtcNow > DueDate.Value &&
                                 !IsComplete;

        #endregion

        #region Workflow Step Management

        /// <summary>
        /// Moves the request to a specific workflow step.
        /// Automatically transitions from Pending to InProgress.
        /// </summary>
        public void MoveToStep(Guid stepId)
        {
            if (IsComplete)
            {
                throw new BusinessException("Approval:CannotModifyCompletedRequest");
            }

            CurrentStepId = stepId;
            if (Status == ApprovalStatus.Pending)
            {
                Status = ApprovalStatus.InProgress;
            }
        }

        /// <summary>
        /// Marks a workflow step as completed.
        /// </summary>
        public void CompleteStep(Guid stepId)
        {
            if (!_completedStepIds.Contains(stepId))
            {
                _completedStepIds.Add(stepId);
            }
        }

        /// <summary>
        /// Checks if a given step has been completed for this request.
        /// </summary>
        public bool IsStepCompleted(Guid stepId)
        {
            return _completedStepIds.Contains(stepId);
        }

        #endregion

        #region Decision Management

        /// <summary>
        /// Records an approval or rejection decision for this request.
        /// </summary>
        public void AddDecision(ApprovalDecision decision)
        {
            if (IsComplete)
            {
                throw new BusinessException("Approval:CannotModifyCompletedRequest");
            }

            _decisions.Add(decision);
        }

        /// <summary>
        /// Approves the request and marks it as completed.
        /// Triggers a distributed event for system-wide notification.
        /// </summary>
        public void Approve(Guid userId)
        {
            if (!IsPending && !IsInProgress)
            {
                throw new BusinessException("Approval:InvalidStatusForApproval")
                    .WithData("CurrentStatus", Status);
            }

            Status = ApprovalStatus.Approved;
            CompletedAt = DateTime.UtcNow;

            AddDistributedEvent(new ApprovalRequestProcessedEto
            {
                RequestId = Id,
                TenantId = TenantId,
                Status = Status,
                ProcessedAt = CompletedAt.Value,
                ProcessedBy = userId,
                EntityType = EntityType,
                EntityId = EntityId
            });
        }

        /// <summary>
        /// Rejects the request and marks it as completed.
        /// Triggers a distributed event for downstream handling.
        /// </summary>
        public void Reject(Guid userId)
        {
            if (!IsPending && !IsInProgress)
            {
                throw new BusinessException("Approval:InvalidStatusForRejection")
                    .WithData("CurrentStatus", Status);
            }

            Status = ApprovalStatus.Rejected;
            CompletedAt = DateTime.UtcNow;

            AddDistributedEvent(new ApprovalRequestProcessedEto
            {
                RequestId = Id,
                TenantId = TenantId,
                Status = Status,
                ProcessedAt = CompletedAt.Value,
                ProcessedBy = userId,
                EntityType = EntityType,
                EntityId = EntityId
            });
        }

        /// <summary>
        /// Cancels the request, providing the reason and user responsible for cancellation.
        /// </summary>
        public void Cancel(Guid cancelledBy, string reason)
        {
            if (IsComplete)
            {
                throw new BusinessException("Approval:CannotCancelCompletedRequest");
            }

            Status = ApprovalStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
            CancelledBy = cancelledBy;
            CancellationReason = reason;
        }

        #endregion
        
        #region Metrics and Calculations

        /// <summary>
        /// Sets a numeric priority for this request (0–10 scale).
        /// </summary>
        public void SetPriority(int priority)
        {
            if (priority < 0 || priority > 10)
            {
                throw new BusinessException("Approval:InvalidPriority")
                    .WithData("ValidRange", "0-10");
            }
            Priority = priority;
        }

        /// <summary>
        /// Assigns a due date for the request. Must be in the future.
        /// </summary>
        public void SetDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.UtcNow)
            {
                throw new BusinessException("Approval:DueDateInPast");
            }
            DueDate = dueDate;
        }

        /// <summary>
        /// Retrieves all approval decisions recorded for a specific step.
        /// </summary>
        public List<ApprovalDecision> GetDecisionsForStep(Guid stepId)
        {
            return _decisions.Where(d => d.StepId == stepId).ToList();
        }

        /// <summary>
        /// Gets the count of approvals made for a particular step.
        /// </summary>
        public int GetApprovalCountForStep(Guid stepId)
        {
            return _decisions.Count(d => d.StepId == stepId &&
                                         d.Action == ApprovalAction.Approved);
        }

        /// <summary>
        /// Gets the count of rejections made for a particular step.
        /// </summary>
        public int GetRejectionCountForStep(Guid stepId)
        {
            return _decisions.Count(d => d.StepId == stepId &&
                                         d.Action == ApprovalAction.Rejected);
        }

        /// <summary>
        /// Gets the remaining time until the request’s due date, or null if not applicable.
        /// </summary>
        public TimeSpan? TimeRemaining
        {
            get
            {
                if (!DueDate.HasValue || IsComplete)
                {
                    return null;
                }

                var remaining = DueDate.Value - DateTime.UtcNow;
                return remaining.TotalSeconds > 0 ? remaining : TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Determines if the request is approaching its deadline within a given threshold (default 24 hours).
        /// Useful for pre-deadline alerts.
        /// </summary>
        public bool IsNearingDeadline(int hoursThreshold = 24)
        {
            var remaining = TimeRemaining;
            return remaining.HasValue &&
                   remaining.Value.TotalHours > 0 &&
                   remaining.Value.TotalHours <= hoursThreshold;
        }

        #endregion
    }
}
