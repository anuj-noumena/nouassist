using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Specifications
{
    public class OverdueApprovalRequestsSpecification : Specification<ApprovalRequest>
    {
        public override Expression<Func<ApprovalRequest, bool>> ToExpression()
        {
            var now = DateTime.UtcNow;
            return request => request.DueDate.HasValue && 
                              request.DueDate.Value < now &&
                              request.Status != ApprovalStatus.Approved &&
                              request.Status != ApprovalStatus.Rejected &&
                              request.Status != ApprovalStatus.Cancelled;
        }
    }
}