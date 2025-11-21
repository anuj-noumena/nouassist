using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Specifications
{
    /// <summary>
    /// Base specification class for workflow instances
    /// </summary>
    public abstract class WorkflowInstanceSpecification : Specification<WorkflowInstance>
    {
    }

    /// <summary>
    /// Specification for active workflow instances
    /// </summary>
    public class ActiveWorkflowInstanceSpecification : WorkflowInstanceSpecification
    {
        public override Expression<Func<WorkflowInstance, bool>> ToExpression()
        {
            return instance => instance.Status == InstanceStatus.Running || 
                             instance.Status == InstanceStatus.Waiting;
        }
    }

    /// <summary>
    /// Specification for completed workflow instances
    /// </summary>
    public class CompletedWorkflowInstanceSpecification : WorkflowInstanceSpecification
    {
        public override Expression<Func<WorkflowInstance, bool>> ToExpression()
        {
            return instance => instance.Status == InstanceStatus.Completed;
        }
    }

    /// <summary>
    /// Specification for workflow instances based on a specific workflow definition
    /// </summary>
    public class WorkflowDefinitionInstanceSpecification : WorkflowInstanceSpecification
    {
        private readonly Guid _workflowDefinitionId;

        public WorkflowDefinitionInstanceSpecification(Guid workflowDefinitionId)
        {
            _workflowDefinitionId = workflowDefinitionId;
        }

        public override Expression<Func<WorkflowInstance, bool>> ToExpression()
        {
            return instance => instance.WorkflowDefinitionId == _workflowDefinitionId;
        }
    }

    /// <summary>
    /// Specification for workflow instances started within a date range
    /// </summary>
    public class DateRangeWorkflowInstanceSpecification : WorkflowInstanceSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public DateRangeWorkflowInstanceSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate.Date;
            _endDate = endDate.Date.AddDays(1).AddTicks(-1);
        }

        public override Expression<Func<WorkflowInstance, bool>> ToExpression()
        {
            return instance => instance.StartTime >= _startDate && instance.StartTime <= _endDate;
        }
    }
}
