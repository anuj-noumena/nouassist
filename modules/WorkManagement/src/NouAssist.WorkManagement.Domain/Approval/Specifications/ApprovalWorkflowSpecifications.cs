using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Specifications
{
    public class ActiveWorkflowsSpecification : Specification<ApprovalWorkflow>
    {
        public override Expression<Func<ApprovalWorkflow, bool>> ToExpression() =>
            w => w.IsActive;
    }

    public class WorkflowsWithTimeoutSpecification : Specification<ApprovalWorkflow>
    {
        public override Expression<Func<ApprovalWorkflow, bool>> ToExpression() =>
            w => w.TimeoutDays.HasValue && w.IsActive;
    }

    public class WorkflowsByStrategySpecification : Specification<ApprovalWorkflow>
    {
        private readonly ApprovalStrategy _strategy;

        public WorkflowsByStrategySpecification(ApprovalStrategy strategy)
        {
            _strategy = strategy;
        }

        public override Expression<Func<ApprovalWorkflow, bool>> ToExpression() =>
            w => w.Strategy == _strategy && w.IsActive;
    }
}
