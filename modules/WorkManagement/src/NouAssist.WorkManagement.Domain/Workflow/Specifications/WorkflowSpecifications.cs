using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;

namespace NouAssist.WorkManagement.Workflow.Specifications
{
    public class ActiveWorkflowsSpecification : BaseSpecification<Aggregates.Workflow>
    {
        public override Expression<Func<Aggregates.Workflow, bool>> ToExpression() =>
            w => w.IsActive;
    }

    public class DefaultWorkflowSpecification : BaseSpecification<Aggregates.Workflow>
    {
        public override Expression<Func<Aggregates.Workflow, bool>> ToExpression() =>
            w => w.IsDefault && w.IsActive;
    }
}
