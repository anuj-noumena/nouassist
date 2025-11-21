using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents a target value for a metric goal
    /// </summary>
    public class GoalTarget : ValueObject
    {
        public MetricValue TargetValue { get; private set; }
        public DateTime TargetDate { get; private set; }
        public ThresholdType ThresholdType { get; private set; }
        public MetricValue? SecondaryValue { get; private set; } // For Between/Outside threshold types

        private GoalTarget() { }

        public GoalTarget(MetricValue targetValue, DateTime targetDate, ThresholdType thresholdType, MetricValue secondaryValue = null)
        {
            TargetValue = targetValue ?? throw new ArgumentNullException(nameof(targetValue));
            SetTargetDate(targetDate);
            ThresholdType = thresholdType;
            
            if ((thresholdType == ThresholdType.Between || thresholdType == ThresholdType.Outside) && secondaryValue == null)
            {
                throw new ArgumentException("Secondary value is required for Between and Outside threshold types", nameof(secondaryValue));
            }
            
            SecondaryValue = secondaryValue;
        }

        public bool IsAchieved(MetricValue currentValue)
        {
            return ThresholdType switch
            {
                ThresholdType.Above => currentValue.IsGreaterThan(TargetValue),
                ThresholdType.Below => currentValue.IsLessThan(TargetValue),
                ThresholdType.Equals => Math.Abs(currentValue.Value - TargetValue.Value) < 0.01m,
                ThresholdType.Between => IsBetween(currentValue),
                ThresholdType.Outside => !IsBetween(currentValue),
                _ => false
            };
        }

        public decimal GetProgress(MetricValue currentValue, MetricValue initialValue = null)
        {
            initialValue ??= new MetricValue(0, TargetValue.DataType);
            
            var totalProgress = TargetValue.Value - initialValue.Value;
            var currentProgress = currentValue.Value - initialValue.Value;
            
            if (totalProgress == 0) return 100;
            
            return Math.Max(0, Math.Min(100, (currentProgress / totalProgress) * 100));
        }

        public int GetDaysRemaining()
        {
            return Math.Max(0, (TargetDate - DateTime.UtcNow).Days);
        }

        public bool IsOverdue()
        {
            return DateTime.UtcNow > TargetDate;
        }

        private void SetTargetDate(DateTime targetDate)
        {
            if (targetDate <= DateTime.UtcNow)
            {
                throw new ArgumentException("Target date must be in the future", nameof(targetDate));
            }
            TargetDate = targetDate;
        }

        private bool IsBetween(MetricValue value)
        {
            if (SecondaryValue == null) return false;
            
            var min = Math.Min(TargetValue.Value, SecondaryValue.Value);
            var max = Math.Max(TargetValue.Value, SecondaryValue.Value);
            
            return value.Value >= min && value.Value <= max;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TargetValue;
            yield return TargetDate;
            yield return ThresholdType;
            yield return SecondaryValue;
        }
    }
}
