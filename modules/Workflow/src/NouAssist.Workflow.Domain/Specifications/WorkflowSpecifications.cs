using System;
using System.Linq.Expressions;
using NouAssist.Workflow.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Workflow.Specifications
{
    public class ActiveWorkflowsSpecification : Specification<Aggregates.Workflow>
    {
        public override Expression<Func<Aggregates.Workflow, bool>> ToExpression() =>
            w => w.IsActive;
    }

    public class DefaultWorkflowSpecification : Specification<Aggregates.Workflow>
    {
        public override Expression<Func<Aggregates.Workflow, bool>> ToExpression() =>
            w => w.IsDefault && w.IsActive;
    }
}
