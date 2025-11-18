using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance
{
    /// <summary>
    /// Manages factors that control or interrupt a task's workflow, like dependencies, blocks, and approvals.
    /// </summary>
    public class TaskFlowControlAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskInstanceId { get; private set; }

        // Dependencies
        private readonly List<TaskDependency> _dependencies = new();
        public IReadOnlyCollection<TaskDependency> Dependencies => _dependencies.AsReadOnly();

        // Approval
        public Guid? ApprovalRequestId { get; private set; }

        // Blocking/Flagging
        private string _blockingReason;
        private DateTime? _blockedAt;
        public bool IsBlocked => !string.IsNullOrEmpty(_blockingReason);
        
        private string _flagReason;
        public bool IsFlagged => !string.IsNullOrEmpty(_flagReason);

        protected TaskFlowControlAggregate() { }

        public TaskFlowControlAggregate(Guid id, Guid taskInstanceId) : base(id)
        {
            TaskInstanceId = taskInstanceId;
        }

        // === Dependency Methods ===
        public void AddDependency(Guid dependsOnTaskInstanceId, DependencyType type, int lagMinutes = 0, bool isBlocking = true)
        {
            if (dependsOnTaskInstanceId == TaskInstanceId) throw new BusinessException("TaskManagement:CannotDependOnSelf");
            if (_dependencies.Any(d => d.DependsOnTaskInstanceId == dependsOnTaskInstanceId)) throw new BusinessException("TaskManagement:DependencyAlreadyExists");

            _dependencies.Add(TaskDependency.CreateInstanceDependency(Id, dependsOnTaskInstanceId, type, lagMinutes, isBlocking));
            AddDistributedEvent(new TaskDependencyAddedEto { TaskFlowControlAggregateId = Id, DependsOnTaskInstanceId = dependsOnTaskInstanceId });
        }

        public void RemoveDependency(Guid dependsOnTaskInstanceId)
        {
            var dependency = _dependencies.FirstOrDefault(d => d.DependsOnTaskInstanceId == dependsOnTaskInstanceId);
            if (dependency != null)
            {
                _dependencies.Remove(dependency);
                AddDistributedEvent(new TaskDependencyRemovedEto { TaskFlowControlAggregateId = Id, DependsOnTaskInstanceId = dependsOnTaskInstanceId });
            }
        }

        // === Approval Methods ===
        public void LinkApprovalRequest(Guid approvalRequestId)
        {
            ApprovalRequestId = approvalRequestId;
            AddDistributedEvent(new TaskApprovalLinkedEto { TaskFlowControlAggregateId = Id, ApprovalRequestId = approvalRequestId });
        }

        public void ClearApprovalLink()
        {
            ApprovalRequestId = null;
        }

        // === Blocking/Flagging Methods ===
        public void Block(string reason)
        {
            _blockingReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));
            _blockedAt = DateTime.UtcNow;
            AddDistributedEvent(new TaskBlockedEto { TaskFlowControlAggregateId = Id, Reason = reason });
        }

        public void Unblock()
        {
            if (!IsBlocked) return;
            _blockingReason = null;
            _blockedAt = null;
            AddDistributedEvent(new TaskUnblockedEto { TaskFlowControlAggregateId = Id });
        }

        public void Flag(string reason)
        {
            _flagReason = reason;
            AddDistributedEvent(new TaskFlaggedEto { TaskFlowControlAggregateId = Id, Reason = reason });
        }

        public void Unflag()
        {
            _flagReason = null;
        }
    }
}