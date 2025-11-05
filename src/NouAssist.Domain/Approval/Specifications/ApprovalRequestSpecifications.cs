using System;
using System.Linq.Expressions;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Approval.Specifications
{
    public class PendingApprovalRequestsSpecification : Specification<ApprovalRequest>
    {
        public override Expression<Func<ApprovalRequest, bool>> ToExpression() =>
            r => r.Status == ApprovalStatus.Pending || 
                 r.Status == ApprovalStatus.InProgress;
    }

    public class OverdueApprovalRequestsSpecification : Specification<ApprovalRequest>
    {
        public override Expression<Func<ApprovalRequest, bool>> ToExpression() =>
            r => r.DueDate.HasValue && 
                 r.DueDate.Value < DateTime.UtcNow &&
                 (r.Status == ApprovalStatus.Pending || 
                  r.Status == ApprovalStatus.InProgress);
    }

    public class ApprovalRequestsByWorkflowSpecification : Specification<ApprovalRequest>
    {
        private readonly Guid _workflowId;

        public ApprovalRequestsByWorkflowSpecification(Guid workflowId)
        {
            _workflowId = workflowId;
        }

        public override Expression<Func<ApprovalRequest, bool>> ToExpression() =>
            r => r.WorkflowId == _workflowId;
    }

    public class ApprovalRequestsByEntitySpecification : Specification<ApprovalRequest>
    {
        private readonly string _entityType;
        private readonly Guid _entityId;

        public ApprovalRequestsByEntitySpecification(string entityType, Guid entityId)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<ApprovalRequest, bool>> ToExpression() =>
            r => r.EntityType == _entityType && r.EntityId == _entityId;
    }

    public class UserPendingApprovalsSpecification : Specification<ApprovalRequest>
    {
        private readonly Guid _userId;

        public UserPendingApprovalsSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<ApprovalRequest, bool>> ToExpression() =>
            r => (r.Status == ApprovalStatus.Pending || 
                  r.Status == ApprovalStatus.InProgress) &&
                 r.RequestedBy == _userId;
    }
}
