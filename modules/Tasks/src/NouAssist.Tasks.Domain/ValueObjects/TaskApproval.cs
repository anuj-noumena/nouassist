using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects
{
    public class TaskApproval : ValueObject
    {
        public Guid? ApprovalRequestId { get; private set; }
        public bool RequiresApproval { get; private set; }

        private TaskApproval() { } // EF Core & serialization

        public TaskApproval(Guid? approvalRequestId, bool requiresApproval)
        {
            if (requiresApproval && approvalRequestId == null)
                throw new ArgumentException("ApprovalRequestId must be provided when approval is required.");

            ApprovalRequestId = approvalRequestId;
            RequiresApproval = requiresApproval;
        }

        // Factory methods for immutability and intent clarity
        public static TaskApproval NoApprovalRequired()
        {
            return new TaskApproval(null, false);
        }

        public static TaskApproval PendingApproval(Guid approvalRequestId)
        {
            return new TaskApproval(approvalRequestId, true);
        }

        // Update methods returning new instances
        public TaskApproval WithApprovalRequest(Guid approvalRequestId)
        {
            return new TaskApproval(approvalRequestId, true);
        }

        public TaskApproval MarkAsNotRequired()
        {
            return new TaskApproval(null, false);
        }

        // Convenience flags
        public bool IsPending => RequiresApproval && !ApprovalRequestId.HasValue;
        public bool IsLinkedToApproval => ApprovalRequestId.HasValue;
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ApprovalRequestId;
            yield return RequiresApproval;
        }
    }
}