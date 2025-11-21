using System;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;
using NouAssist.RequirementsManagement.ChangeManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.RequirementsManagement.ChangeManagement.Entities
{
    /// <summary>
    /// Represents an approval step in the change request workflow
    /// </summary>
    public class ApprovalStep : AuditedEntity<Guid>
    {
        public Guid ChangeRequestId { get; private set; }
        public ApproverRole ApproverRole { get; private set; }
        public Guid ApproverId { get; private set; }
        public int SequenceOrder { get; private set; }
        public ApprovalDecision Decision { get; private set; }
        public DecisionReason DecisionReason { get; private set; }
        public DateTime? DecisionDate { get; private set; }

        protected ApprovalStep() { }

        internal ApprovalStep(
            Guid changeRequestId,
            ApproverRole approverRole,
            Guid approverId,
            int sequenceOrder)
        {
            Id = Guid.NewGuid();
            ChangeRequestId = Check.NotEmpty(changeRequestId, nameof(changeRequestId));
            ApproverRole = Check.NotNull(approverRole, nameof(approverRole));
            ApproverId = Check.NotEmpty(approverId, nameof(approverId));
            SequenceOrder = sequenceOrder;
            Decision = ApprovalDecision.Pending;
        }

        internal void ProcessDecision(ApprovalDecision decision, DecisionReason reason, DateTime decisionDate)
        {
            if (Decision != ApprovalDecision.Pending)
            {
                throw new ChangeManagementDomainException("ChangeManagement:ErrorMessage:DecisionAlreadyMade");
            }

            Decision = decision;
            DecisionReason = Check.NotNull(reason, nameof(reason));
            DecisionDate = decisionDate;
        }

        /// <summary>
        /// Checks if this approval step is completed (has a decision)
        /// </summary>
        public bool IsCompleted => Decision != ApprovalDecision.Pending;

        /// <summary>
        /// Checks if this approval step was approved
        /// </summary>
        public bool IsApproved => Decision == ApprovalDecision.Approved;

        /// <summary>
        /// Checks if this approval step was rejected
        /// </summary>
        public bool IsRejected => Decision == ApprovalDecision.Rejected;
    }
}
