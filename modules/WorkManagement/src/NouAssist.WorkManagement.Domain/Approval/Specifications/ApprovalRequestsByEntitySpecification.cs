using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Approval.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Specifications
{
    public class ApprovalRequestsByEntitySpecification : Specification<ApprovalRequest>
    {
        private readonly string _entityType;
        private readonly Guid _entityId;

        public ApprovalRequestsByEntitySpecification(string entityType, Guid entityId)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<ApprovalRequest, bool>> ToExpression()
        {
            return request => request.EntityType == _entityType && 
                              request.EntityId == _entityId;
        }
    }
}