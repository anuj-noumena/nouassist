using System;
using System.Collections.Generic;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Entities
{
    public class ApprovalDecision : Entity<Guid>
    {
        public Guid RequestId { get; private set; }
        public Guid StepId { get; private set; }
        public Guid ApproverId { get; private set; }
        public ApprovalAction Action { get; private set; }
        public string Comments { get; private set; }
        public string Signature { get; private set; }
        public DateTime DecidedAt { get; private set; }
        public bool IsDelegated { get; private set; }
        public Guid? DelegatedFrom { get; private set; }
        public Dictionary<string, object> CustomFieldValues { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }

        protected ApprovalDecision() { /* ORM */ }

        public ApprovalDecision(
            Guid id,
            Guid requestId,
            Guid stepId,
            Guid approverId,
            ApprovalAction action,
            string comments = null,
            string signature = null
        ) : base(id)
        {
            RequestId = requestId;
            StepId = stepId;
            ApproverId = approverId;
            Action = action;
            Comments = comments;
            Signature = signature;
            DecidedAt = DateTime.UtcNow;
            IsDelegated = false;
            CustomFieldValues = new Dictionary<string, object>();
            Metadata = new Dictionary<string, object>();
        }

        public void MarkAsDelegated(Guid delegatedFrom)
        {
            IsDelegated = true;
            DelegatedFrom = delegatedFrom;
        }

        public bool IsApproval => Action == ApprovalAction.Approved;
        public bool IsRejection => Action == ApprovalAction.Rejected;
        public bool IsAbstained => Action == ApprovalAction.Abstained;
    }
}
