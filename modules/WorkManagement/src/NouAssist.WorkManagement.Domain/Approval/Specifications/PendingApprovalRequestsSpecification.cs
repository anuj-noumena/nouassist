using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Specifications
{
    public class PendingApprovalRequestsSpecification : Specification<ApprovalRequest>
    {
        public override Expression<Func<ApprovalRequest, bool>> ToExpression()
        {
            return request => request.Status == ApprovalStatus.Pending || 
                              request.Status == ApprovalStatus.InProgress;
        }
    }
}