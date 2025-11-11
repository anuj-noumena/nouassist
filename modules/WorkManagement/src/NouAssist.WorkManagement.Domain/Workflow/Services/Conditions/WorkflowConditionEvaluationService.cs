using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions
{
    /// <summary>
    /// Orchestrates condition evaluation using multiple evaluators.
    /// Follows Single Responsibility Principle - coordinates evaluation logic
    /// Follows Dependency Inversion Principle - depends on IWorkflowConditionEvaluator abstraction
    /// Follows Open/Closed Principle - new evaluators can be added without modifying this class
    /// </summary>
    public class WorkflowConditionEvaluationService : DomainService
    {
        private readonly IEnumerable<IWorkflowConditionEvaluator> _evaluators;
        private readonly ILogger<WorkflowConditionEvaluationService> _logger;

        public WorkflowConditionEvaluationService(
            IEnumerable<IWorkflowConditionEvaluator> evaluators,
            ILogger<WorkflowConditionEvaluationService> logger)
        {
            _evaluators = evaluators;
            _logger = logger;
        }

        /// <summary>
        /// Evaluates all conditions for a transition
        /// Returns true only if ALL conditions are satisfied (AND logic)
        /// </summary>
        public async Task<ConditionEvaluationAggregateResult> EvaluateAllAsync(
            IEnumerable<WorkflowCondition> conditions,
            WorkflowConditionContext context)
        {
            var result = new ConditionEvaluationAggregateResult { AllSatisfied = true };

            if (conditions == null || !conditions.Any())
            {
                return result; // No conditions means all pass
            }

            foreach (var condition in conditions)
            {
                var evaluationResult = await EvaluateSingleAsync(condition, context);
                result.Results.Add(evaluationResult);

                if (!evaluationResult.IsSatisfied)
                {
                    result.AllSatisfied = false;
                    result.FailureReasons.Add(evaluationResult.FailureReason);

                    _logger.LogWarning(
                        "Condition evaluation failed: {Reason}",
                        evaluationResult.FailureReason);
                }
            }

            return result;
        }

        /// <summary>
        /// Evaluates a single condition using the appropriate evaluator
        /// </summary>
        public async Task<ConditionEvaluationResult> EvaluateSingleAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context)
        {
            if (condition == null)
            {
                return ConditionEvaluationResult.Failure("Condition is null");
            }

            // Find the first evaluator that can handle this condition
            var evaluator = _evaluators.FirstOrDefault(e => e.CanEvaluate(condition));

            if (evaluator == null)
            {
                var errorMsg = $"No evaluator found for condition with operator: {condition.Operator}";
                _logger.LogError(errorMsg);
                return ConditionEvaluationResult.Failure(errorMsg);
            }

            try
            {
                return await evaluator.EvaluateAsync(condition, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating condition: {Key} {Operator} {Value}",
                    condition.Key, condition.Operator, condition.Value);
                return ConditionEvaluationResult.Failure($"Evaluation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Evaluates conditions with OR logic (at least one must be satisfied)
        /// </summary>
        public async Task<ConditionEvaluationAggregateResult> EvaluateAnyAsync(
            IEnumerable<WorkflowCondition> conditions,
            WorkflowConditionContext context)
        {
            var result = new ConditionEvaluationAggregateResult { AllSatisfied = false };

            if (conditions == null || !conditions.Any())
            {
                result.AllSatisfied = true;
                return result; // No conditions means all pass
            }

            foreach (var condition in conditions)
            {
                var evaluationResult = await EvaluateSingleAsync(condition, context);
                result.Results.Add(evaluationResult);

                if (evaluationResult.IsSatisfied)
                {
                    result.AllSatisfied = true;
                    return result; // At least one satisfied, we're done
                }
                else
                {
                    result.FailureReasons.Add(evaluationResult.FailureReason);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Aggregated result of multiple condition evaluations
    /// </summary>
    public class ConditionEvaluationAggregateResult
    {
        public bool AllSatisfied { get; set; }
        public List<ConditionEvaluationResult> Results { get; set; } = new();
        public List<string> FailureReasons { get; set; } = new();

        public int TotalConditions => Results.Count;
        public int SatisfiedCount => Results.Count(r => r.IsSatisfied);
        public int UnsatisfiedCount => Results.Count(r => !r.IsSatisfied);
    }
}
