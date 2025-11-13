using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions
{
        /// <summary>
    /// IMPROVED: Field value condition evaluator with better type handling
    /// </summary>
    public class FieldValueConditionEvaluator : ConditionEvaluatorBase
    {
        protected override string[] SupportedOperators => new[]
        {
            "equals", "notEquals",
            "greaterThan", "lessThan", "greaterThanOrEquals", "lessThanOrEquals",
            "contains", "notContains", "startsWith", "endsWith",
            "isEmpty", "isNotEmpty",
            "in", "notIn"
        };

        public override async Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context)
        {
            var actualValue = context.CustomFieldValues.GetValue(condition.Key);

            if (actualValue == null && !context.AdditionalData.TryGetValue(condition.Key, out actualValue))
            {
                return Failure($"Field '{condition.Key}' not found");
            }

            var result = condition.Operator.ToLower() switch
            {
                "equals" => Equals(actualValue, condition.Value),
                "notequals" => !Equals(actualValue, condition.Value),
                "greaterthan" => CompareGreaterThan(actualValue, condition.Value),
                "lessthan" => CompareLessThan(actualValue, condition.Value),
                "greaterthanorequals" => CompareGreaterThanOrEquals(actualValue, condition.Value),
                "lessthanorequals" => CompareLessThanOrEquals(actualValue, condition.Value),
                "contains" => Contains(actualValue, condition.Value),
                "notcontains" => !Contains(actualValue, condition.Value),
                "startswith" => StartsWith(actualValue, condition.Value),
                "endswith" => EndsWith(actualValue, condition.Value),
                "isempty" => IsEmpty(actualValue),
                "isnotempty" => !IsEmpty(actualValue),
                "in" => In(actualValue, condition.Value),
                "notin" => !In(actualValue, condition.Value),
                _ => false
            };

            return result
                ? Success()
                : Failure($"Condition not satisfied: {condition.Key} {condition.Operator} {condition.Value}");
        }

        private bool Equals(object actual, object expected)
        {
            if (actual == null && expected == null) return true;
            if (actual == null || expected == null) return false;

            // Try numeric comparison first
            if (TryConvertToDouble(actual, out var actualNum) &&
                TryConvertToDouble(expected, out var expectedNum))
            {
                return Math.Abs(actualNum - expectedNum) < 0.0001;
            }

            // String comparison
            return actual.ToString().Equals(expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private bool CompareGreaterThan(object actual, object expected)
        {
            if (TryConvertToDouble(actual, out var actualNum) &&
                TryConvertToDouble(expected, out var expectedNum))
            {
                return actualNum > expectedNum;
            }

            // Try date comparison
            if (TryConvertToDateTime(actual, out var actualDate) &&
                TryConvertToDateTime(expected, out var expectedDate))
            {
                return actualDate > expectedDate;
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

            if (TryConvertToDateTime(actual, out var actualDate) &&
                TryConvertToDateTime(expected, out var expectedDate))
            {
                return actualDate < expectedDate;
            }

            return false;
        }

        private bool CompareGreaterThanOrEquals(object actual, object expected)
        {
            return Equals(actual, expected) || CompareGreaterThan(actual, expected);
        }

        private bool CompareLessThanOrEquals(object actual, object expected)
        {
            return Equals(actual, expected) || CompareLessThan(actual, expected);
        }

        private bool Contains(object actual, object expected)
        {
            if (actual == null || expected == null) return false;
            return actual.ToString().Contains(expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private bool StartsWith(object actual, object expected)
        {
            if (actual == null || expected == null) return false;
            return actual.ToString().StartsWith(expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private bool EndsWith(object actual, object expected)
        {
            if (actual == null || expected == null) return false;
            return actual.ToString().EndsWith(expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsEmpty(object value)
        {
            if (value == null) return true;
            if (value is string str) return string.IsNullOrWhiteSpace(str);
            if (value is IEnumerable<object> enumerable) return !enumerable.Any();
            return false;
        }

        private bool In(object actual, object expected)
        {
            if (actual == null || expected == null) return false;

            // Expected should be a collection
            if (expected is IEnumerable<object> collection)
            {
                return collection.Any(item => Equals(actual, item));
            }

            // Try parsing as comma-separated string
            if (expected is string str)
            {
                var values = str.Split(',').Select(v => v.Trim());
                return values.Any(v => Equals(actual, v));
            }

            return false;
        }

        private bool TryConvertToDouble(object value, out double result)
        {
            result = 0;
            if (value == null) return false;
            return double.TryParse(value.ToString(), out result);
        }

        private bool TryConvertToDateTime(object value, out DateTime result)
        {
            result = DateTime.MinValue;
            if (value == null) return false;
            if (value is DateTime dt)
            {
                result = dt;
                return true;
            }
            return DateTime.TryParse(value.ToString(), out result);
        }
    }

}
