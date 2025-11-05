using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Approval.Entities;
using NouAssist.Approval.Enums;
using NouAssist.Approval.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Approval.Aggregates
{
    public class ApprovalRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid WorkflowId { get; private set; }
        public string EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public Guid RequestedBy { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ApprovalStatus Status { get; private set; }
        public Guid? CurrentStepId { get; private set; }
        public DateTime SubmittedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public Guid? CancelledBy { get; private set; }
        public string CancellationReason { get; private set; }
        public int Priority { get; private set; }
        public DateTime? DueDate { get; private set; }
        public Dictionary<string, object> CustomFieldValues { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<Guid> _completedStepIds = new();
        public IReadOnlyCollection<Guid> CompletedStepIds => _completedStepIds.AsReadOnly();

        private readonly List<ApprovalDecision> _decisions = new();
        public IReadOnlyCollection<ApprovalDecision> Decisions => _decisions.AsReadOnly();

        protected ApprovalRequest() { /* ORM */ }

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
            CustomFieldValues = new Dictionary<string, object>();
            Metadata = new Dictionary<string, object>();
            
            
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

        public bool IsPending => Status == ApprovalStatus.Pending;
        public bool IsInProgress => Status == ApprovalStatus.InProgress;
        public bool IsApproved => Status == ApprovalStatus.Approved;
        public bool IsRejected => Status == ApprovalStatus.Rejected;
        public bool IsCancelled => Status == ApprovalStatus.Cancelled;
        public bool IsComplete => IsApproved || IsRejected || IsCancelled;

        public bool IsOverdue => DueDate.HasValue && 
                                 DateTime.UtcNow > DueDate.Value && 
                                 !IsComplete;

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

        public void CompleteStep(Guid stepId)
        {
            if (!_completedStepIds.Contains(stepId))
            {
                _completedStepIds.Add(stepId);
            }
        }

        public bool IsStepCompleted(Guid stepId)
        {
            return _completedStepIds.Contains(stepId);
        }

        public void AddDecision(ApprovalDecision decision)
        {
            if (IsComplete)
            {
                throw new BusinessException("Approval:CannotModifyCompletedRequest");
            }

            _decisions.Add(decision);
        }

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

        public void SetPriority(int priority)
        {
            if (priority < 0 || priority > 10)
            {
                throw new BusinessException("Approval:InvalidPriority")
                    .WithData("ValidRange", "0-10");
            }
            Priority = priority;
        }

        public void SetDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.UtcNow)
            {
                throw new BusinessException("Approval:DueDateInPast");
            }
            DueDate = dueDate;
        }

        public void SetCustomFieldValue(string key, object value)
        {
            CustomFieldValues[key] = value;
        }

        public List<ApprovalDecision> GetDecisionsForStep(Guid stepId)
        {
            return _decisions.Where(d => d.StepId == stepId).ToList();
        }

        public int GetApprovalCountForStep(Guid stepId)
        {
            return _decisions.Count(d => d.StepId == stepId && 
                                        d.Action == ApprovalAction.Approved);
        }

        public int GetRejectionCountForStep(Guid stepId)
        {
            return _decisions.Count(d => d.StepId == stepId && 
                                        d.Action == ApprovalAction.Rejected);
        }
        
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

        public bool IsNearingDeadline(int hoursThreshold = 24)
        {
            var remaining = TimeRemaining;
            return remaining.HasValue && 
                   remaining.Value.TotalHours > 0 && 
                   remaining.Value.TotalHours <= hoursThreshold;
        }
    }
}
