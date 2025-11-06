using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects
{
    /// <summary>
    /// Value object tracking approval status for a task instance
    /// </summary>
    public class TaskApproval : ValueObject
    {
        public Guid? ApprovalRequestId { get; private set; }
        public bool RequiresApproval { get; private set; }
        public ApprovalStatusEnum Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private TaskApproval() { } // EF Core & serialization

        public TaskApproval(
            Guid? approvalRequestId,
            bool requiresApproval,
            ApprovalStatusEnum status = ApprovalStatusEnum.NotRequired,
            DateTime? completedAt = null)
        {
            ApprovalRequestId = approvalRequestId;
            RequiresApproval = requiresApproval;
            Status = status;
            CompletedAt = completedAt;
        }

        // Factory methods for immutability and intent clarity
        public static TaskApproval NoApprovalRequired()
        {
            return new TaskApproval(null, false, ApprovalStatusEnum.NotRequired);
        }

        public static TaskApproval PendingApproval(Guid approvalRequestId)
        {
            return new TaskApproval(approvalRequestId, true, ApprovalStatusEnum.Pending);
        }

        // Update methods returning new instances
        public TaskApproval WithApprovalRequest(Guid approvalRequestId)
        {
            return new TaskApproval(approvalRequestId, true, ApprovalStatusEnum.Pending);
        }

        public TaskApproval MarkAsNotRequired()
        {
            return new TaskApproval(null, false, ApprovalStatusEnum.NotRequired);
        }

        public TaskApproval MarkAsApproved(DateTime approvedAt, Guid? requestId = null)
        {
            return new TaskApproval(
                requestId ?? ApprovalRequestId,
                RequiresApproval,
                ApprovalStatusEnum.Approved,
                approvedAt);
        }

        public TaskApproval MarkAsRejected(DateTime rejectedAt, Guid? requestId = null)
        {
            return new TaskApproval(
                requestId ?? ApprovalRequestId,
                RequiresApproval,
                ApprovalStatusEnum.Rejected,
                rejectedAt);
        }

        // Convenience flags
        public bool IsPending => Status == ApprovalStatusEnum.Pending;
        public bool IsApproved => Status == ApprovalStatusEnum.Approved;
        public bool IsRejected => Status == ApprovalStatusEnum.Rejected;
        public bool IsLinkedToApproval => ApprovalRequestId.HasValue;
        public bool IsCompleted => IsApproved || IsRejected;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ApprovalRequestId;
            yield return RequiresApproval;
            yield return Status;
            yield return CompletedAt;
        }
    }

    /// <summary>
    /// Status of task approval
    /// </summary>
    public enum ApprovalStatusEnum
    {
        NotRequired = 0,
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
}
