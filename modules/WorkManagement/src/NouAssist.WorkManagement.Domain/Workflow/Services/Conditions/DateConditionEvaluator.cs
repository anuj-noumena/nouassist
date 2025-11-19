using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.ValueObjects;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions;

    /// <summary>
    /// NEW: Date-based condition evaluator
    /// </summary>
    public class DateConditionEvaluator : ConditionEvaluatorBase
    {
        protected override string[] SupportedOperators => new[]
        {
            "isToday", "isYesterday", "isTomorrow",
            "isThisWeek", "isThisMonth", "isThisYear",
            "isPast", "isFuture",
            "isWeekday", "isWeekend",
            "isWithinDays", "isWithinHours"
        };

        public override async Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context)
        {
            var dateValue = context.CustomFieldValues.GetValue(condition.Key);

            if (dateValue == null || !TryConvertToDateTime(dateValue, out var date))
            {
                return Failure($"Field '{condition.Key}' is not a valid date");
            }

            var now = Clock.Now;
            var result = condition.Operator.ToLower() switch
            {
                "istoday" => date.Date == now.Date,
                "isyesterday" => date.Date == now.Date.AddDays(-1),
                "istomorrow" => date.Date == now.Date.AddDays(1),
                "isthisweek" => IsInCurrentWeek(date, now),
                "isthismonth" => date.Year == now.Year && date.Month == now.Month,
                "isthisyear" => date.Year == now.Year,
                "ispast" => date < now,
                "isfuture" => date > now,
                "isweekday" => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday,
                "isweekend" => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
                "iswithindays" => IsWithinDays(date, now, condition.Value),
                "iswithinhours" => IsWithinHours(date, now, condition.Value),
                _ => false
            };

            return result ? Success() : Failure($"Date condition not satisfied: {condition.Operator}");
        }

        private bool IsInCurrentWeek(DateTime date, DateTime reference)
        {
            var weekStart = reference.Date.AddDays(-(int)reference.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);
            return date.Date >= weekStart && date.Date < weekEnd;
        }

        private bool IsWithinDays(DateTime date, DateTime reference, object daysValue)
        {
            if (!int.TryParse(daysValue?.ToString(), out var days))
            {
                return false;
            }

            var difference = Math.Abs((date - reference).TotalDays);
            return difference <= days;
        }

        private bool IsWithinHours(DateTime date, DateTime reference, object hoursValue)
        {
            if (!int.TryParse(hoursValue?.ToString(), out var hours))
            {
                return false;
            }

            var difference = Math.Abs((date - reference).TotalHours);
            return difference <= hours;
        }

        private bool TryConvertToDateTime(object value, out DateTime result)
        {
            result = DateTime.MinValue;
            if (value == null)
            {
                return false;
            }

            if (value is DateTime dt)
            {
                result = dt;
                return true;
            }
            return DateTime.TryParse(value.ToString(), out result);
        }
    }
