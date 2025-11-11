using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions
{
    /// <summary>
    /// Evaluates conditions based on field values.
    /// Follows Single Responsibility Principle - only handles field value comparisons
    /// Follows Open/Closed Principle - can be extended without modification
    /// </summary>
    public class FieldValueConditionEvaluator : DomainService, IWorkflowConditionEvaluator
    {
        public bool CanEvaluate(WorkflowCondition condition)
        {
            return condition?.Operator != null &&
                   (condition.Operator.Equals("equals", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("notEquals", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("greaterThan", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("lessThan", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("greaterThanOrEquals", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("lessThanOrEquals", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("contains", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("notContains", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("isEmpty", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("isNotEmpty", StringComparison.OrdinalIgnoreCase));
        }

        public Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context)
        {
            if (condition == null)
            {
                return Task.FromResult(ConditionEvaluationResult.Failure("Condition is null"));
            }

            // Get the field value from context
            if (!context.CustomFieldValues.TryGetValue(condition.Key, out var actualValue))
            {
                // Field not found in custom fields, check additional data
                if (!context.AdditionalData.TryGetValue(condition.Key, out actualValue))
                {
                    return Task.FromResult(
                        ConditionEvaluationResult.Failure($"Field '{condition.Key}' not found"));
                }
            }

            var result = EvaluateCondition(condition.Operator, actualValue, condition.Value);

            return Task.FromResult(result
                ? ConditionEvaluationResult.Success()
                : ConditionEvaluationResult.Failure(
                    $"Condition not satisfied: {condition.Key} {condition.Operator} {condition.Value}"));
        }

        private bool EvaluateCondition(string operatorType, object actualValue, object expectedValue)
        {
            return operatorType.ToLower() switch
            {
                "equals" => CompareEquals(actualValue, expectedValue),
                "notequals" => !CompareEquals(actualValue, expectedValue),
                "greaterthan" => CompareGreaterThan(actualValue, expectedValue),
                "lessthan" => CompareLessThan(actualValue, expectedValue),
                "greaterthanorequals" => CompareGreaterThanOrEquals(actualValue, expectedValue),
                "lessthanorequals" => CompareLessThanOrEquals(actualValue, expectedValue),
                "contains" => CompareContains(actualValue, expectedValue),
                "notcontains" => !CompareContains(actualValue, expectedValue),
                "isempty" => IsEmpty(actualValue),
                "isnotempty" => !IsEmpty(actualValue),
                _ => false
            };
        }

        private bool CompareEquals(object actual, object expected)
        {
            if (actual == null && expected == null) return true;
            if (actual == null || expected == null) return false;
            return actual.ToString().Equals(expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private bool CompareGreaterThan(object actual, object expected)
        {
            if (TryConvertToDouble(actual, out var actualNum) &&
                TryConvertToDouble(expected, out var expectedNum))
            {
                return actualNum > expectedNum;
            }
            return false;
        }

        private bool CompareLessThan(object actual, object expected)
        {
            if (TryConvertToDouble(actual, out var actualNum) &&
                TryConvertToDouble(expected, out var expectedNum))
            {
                return actualNum < expectedNum;
            }
            return false;
        }

        private bool CompareGreaterThanOrEquals(object actual, object expected)
        {
            if (TryConvertToDouble(actual, out var actualNum) &&
                TryConvertToDouble(expected, out var expectedNum))
            {
                return actualNum >= expectedNum;
            }
            return false;
        }

        private bool CompareLessThanOrEquals(object actual, object expected)
        {
            if (TryConvertToDouble(actual, out var actualNum) &&
                TryConvertToDouble(expected, out var expectedNum))
            {
                return actualNum <= expectedNum;
            }
            return false;
        }

        private bool CompareContains(object actual, object expected)
        {
            if (actual == null || expected == null) return false;
            return actual.ToString().Contains(expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsEmpty(object value)
        {
            if (value == null) return true;
            if (value is string str) return string.IsNullOrWhiteSpace(str);
            return false;
        }

        private bool TryConvertToDouble(object value, out double result)
        {
            result = 0;
            if (value == null) return false;

            return double.TryParse(value.ToString(), out result);
        }
    }
}
