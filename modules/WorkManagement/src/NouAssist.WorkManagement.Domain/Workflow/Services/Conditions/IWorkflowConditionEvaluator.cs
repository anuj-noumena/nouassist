using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.ValueObjects;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions
{
    /// <summary>
    /// Interface for evaluating workflow conditions.
    /// Follows Interface Segregation Principle - single focused interface
    /// Follows Dependency Inversion Principle - abstracts condition evaluation
    /// </summary>
    public interface IWorkflowConditionEvaluator
    {
        /// <summary>
        /// Evaluates a condition against the provided context
        /// </summary>
        Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context);

        /// <summary>
        /// Checks if this evaluator can handle the given condition
        /// </summary>
        bool CanEvaluate(WorkflowCondition condition);
    }

    /// <summary>
    /// Context containing data needed for condition evaluation
    /// </summary>
    public class WorkflowConditionContext
    {
        public System.Guid TaskInstanceId { get; set; }
        public System.Guid CurrentUserId { get; set; }
        public System.Collections.Generic.List<string> UserRoles { get; set; }
        public System.Collections.Generic.Dictionary<string, object> CustomFieldValues { get; set; }
        public System.Collections.Generic.Dictionary<string, object> AdditionalData { get; set; }

        public WorkflowConditionContext()
        {
            UserRoles = new System.Collections.Generic.List<string>();
            CustomFieldValues = new System.Collections.Generic.Dictionary<string, object>();
            AdditionalData = new System.Collections.Generic.Dictionary<string, object>();
        }
    }

    /// <summary>
    /// Result of condition evaluation
    /// </summary>
    public class ConditionEvaluationResult
    {
        public bool IsSatisfied { get; set; }
        public string FailureReason { get; set; }
        public System.Collections.Generic.Dictionary<string, object> EvaluationData { get; set; }

        public ConditionEvaluationResult()
        {
            EvaluationData = new System.Collections.Generic.Dictionary<string, object>();
        }

        public static ConditionEvaluationResult Success()
        {
            return new ConditionEvaluationResult { IsSatisfied = true };
        }

        public static ConditionEvaluationResult Failure(string reason)
        {
            return new ConditionEvaluationResult
            {
                IsSatisfied = false,
                FailureReason = reason
            };
        }
    }
}
