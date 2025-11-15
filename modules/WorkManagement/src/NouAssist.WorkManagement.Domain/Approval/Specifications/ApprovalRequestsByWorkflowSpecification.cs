using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Approval.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Specifications
{
    public class ApprovalRequestsByWorkflowSpecification : Specification<ApprovalRequest>
    {
        private readonly Guid _workflowId;

        public ApprovalRequestsByWorkflowSpecification(Guid workflowId)
        {
            _workflowId = workflowId;
        }

        public override Expression<Func<ApprovalRequest, bool>> ToExpression()
        {
            return request => request.WorkflowId == _workflowId;
        }
    }
}