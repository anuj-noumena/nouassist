using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Approval.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Specifications
{
    public class UserPendingApprovalsSpecification : Specification<ApprovalRequest>
    {
        private readonly Guid _userId;

        public UserPendingApprovalsSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<ApprovalRequest, bool>> ToExpression()
        {
            return request => request.RequestedBy == _userId;
        }
    }
}