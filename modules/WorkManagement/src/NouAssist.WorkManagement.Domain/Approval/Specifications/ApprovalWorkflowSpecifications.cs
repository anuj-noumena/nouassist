using System;
using System.Linq.Expressions;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Approval.Specifications
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
