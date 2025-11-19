using System;
using NouAssist.WorkManagement.Shared.ValueObjects;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance
{
    /// <summary>
    /// The core aggregate representing a task instance.
    /// Manages identity, ownership, primary status, and resolution.
    /// </summary>
    public class TaskInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid TaskId { get; private set; } // The template/definition ID
        public Guid WorkflowId { get; private set; }
        public Guid WorkflowStatusId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int EntityVersion { get; private set; }

        // Ownership
        public Guid OwnerId { get; private set; }
        public Guid? AssigneeId { get; private set; }
        public Guid? ReporterId { get; private set; }
        public Guid? TeamId { get; private set; }

        // Core Properties
        public string Notes { get; private set; }
        public CustomFieldValues CustomFields { get; private set; }

        // Completion State
        private string _resolution;
        private string _resolutionNote;
        public bool IsResolved => !string.IsNullOrEmpty(_resolution);
        public string Resolution => _resolution;
        public string ResolutionNote => _resolutionNote;

        // Cancellation State
        private bool _isCancelled;
        private string _cancellationReason;
        private DateTime? _cancelledAt;
        public bool IsCancelled => _isCancelled;
        public string CancellationReason => _cancellationReason;
        public DateTime? CancelledAt => _cancelledAt;

        // Computed Properties
        public bool IsCompleted => IsResolved;
        public bool IsActive => !IsResolved && !IsCancelled;

        protected TaskInstance() { }

        public TaskInstance(
            Guid id,
            Guid taskId,
            Guid workflowId,
            Guid workflowStatusId,
            Guid ownerId,
            Guid? tenantId = null,
            Guid? reporterId = null,
            Guid? assigneeId = null) : base(id)
        {
            TaskId = taskId;
            WorkflowId = workflowId;
            WorkflowStatusId = workflowStatusId;
            OwnerId = ownerId;
            ReporterId = reporterId ?? ownerId;
            AssigneeId = assigneeId;
            TenantId = tenantId;
            EntityVersion = 1;
            Notes = string.Empty;
            CustomFields = CustomFieldValues.Create();

            AddDistributedEvent(new TaskInstanceCreatedEto { TaskInstanceId = Id, TaskId = TaskId, OwnerId = OwnerId });
        }

        // === Workflow & Assignment Methods ===
        public void TransitionTo(Guid newWorkflowStatusId, string comment = null)
        {
            var oldStatusId = WorkflowStatusId;
            WorkflowStatusId = newWorkflowStatusId;
            AddDistributedEvent(new TaskStatusTransitionedEto { TaskInstanceId = Id, OldStatusId = oldStatusId, NewStatusId = newWorkflowStatusId, Comment = comment });
        }

        public void Assign(Guid assigneeId)
        {
            var oldAssignee = AssigneeId;
            AssigneeId = assigneeId;
            if (oldAssignee != assigneeId)
            {
                AddDistributedEvent(new TaskAssignedEto { TaskInstanceId = Id, AssignedTo = assigneeId, PreviousAssignee = oldAssignee });
            }
        }

        public void Unassign()
        {
            if (AssigneeId.HasValue)
            {
                var oldAssignee = AssigneeId;
                AssigneeId = null;
                AddDistributedEvent(new TaskUnassignedEto { TaskInstanceId = Id, PreviousAssignee = oldAssignee });
            }
        }

        public void UpdateOwner(Guid newOwnerId)
        {
            OwnerId = newOwnerId;
            AddDistributedEvent(new TaskInstanceOwnerUpdatedEto { TaskInstanceId = Id, NewOwnerId = newOwnerId });
        }

        // === Core Data Methods ===
        public void UpdateNotes(string newNotes)
        {
            Notes = newNotes;
        }

        public void SetCustomFieldValue(string key, object value)
        {
            CustomFields = CustomFields.SetValue(key, value);
        }

        // === Completion Methods ===
        public void Complete(string resolution, string resolutionNote = null)
        {
            if (IsResolved) throw new BusinessException("TaskManagement:TaskAlreadyCompleted");
            if (IsCancelled) throw new BusinessException("TaskManagement:CannotCompleteACancelledTask");

            _resolution = Check.NotNullOrWhiteSpace(resolution, nameof(resolution));
            _resolutionNote = resolutionNote;

            AddDistributedEvent(new TaskCompletedEto
            {
                TaskInstanceId = Id,
                Resolution = resolution,
                ResolutionNote = resolutionNote,
                CompletedBy = AssigneeId
            });
        }

        // === Cancellation Methods ===
        public void Cancel(string reason)
        {
            if (IsResolved) throw new BusinessException("TaskManagement:CannotCancelACompletedTask");
            if (IsCancelled) throw new BusinessException("TaskManagement:TaskAlreadyCancelled");

            _isCancelled = true;
            _cancellationReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));
            _cancelledAt = DateTime.UtcNow;

            AddDistributedEvent(new TaskCancelledEto
            {
                TaskInstanceId = Id,
                CancellationReason = reason,
                CancelledAt = _cancelledAt.Value,
                CancelledBy = AssigneeId
            });
        }

        public void Reopen()
        {
            if (!IsResolved && !IsCancelled)
            {
                throw new BusinessException("TaskManagement:TaskIsNotCompletedOrCancelled");
            }

            // Clear completion state
            _resolution = null;
            _resolutionNote = null;

            // Clear cancellation state
            _isCancelled = false;
            _cancellationReason = null;
            _cancelledAt = null;

            AddDistributedEvent(new TaskReopenedEto
            {
                TaskInstanceId = Id,
                ReopenedAt = DateTime.UtcNow
            });
        }
    }
}