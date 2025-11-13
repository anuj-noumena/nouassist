using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions
{
    /// <summary>
    /// Base class for condition evaluators
    /// </summary>
    public abstract class ConditionEvaluatorBase : DomainService, IWorkflowConditionEvaluator
    {
        protected abstract string[] SupportedOperators { get; }

        public virtual bool CanEvaluate(WorkflowCondition condition)
        {
            return condition?.Operator != null &&
                   SupportedOperators.Any(op => op.Equals(condition.Operator, StringComparison.OrdinalIgnoreCase));
        }

        public abstract Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context);

        protected ConditionEvaluationResult Success()
        {
            return ConditionEvaluationResult.Success();
        }

        protected ConditionEvaluationResult Failure(string reason)
        {
            return ConditionEvaluationResult.Failure(reason);
        }
    }




}