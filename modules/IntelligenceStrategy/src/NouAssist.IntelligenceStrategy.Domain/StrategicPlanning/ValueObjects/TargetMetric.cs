using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing a measurable target metric
    /// </summary>
    public class TargetMetric : ValueObject
    {
        public string Name { get; private set; }
        public decimal Value { get; private set; }
        public string Unit { get; private set; }
        public DateTime TargetDate { get; private set; }

        private TargetMetric() { }

        public TargetMetric(string name, decimal value, string unit, DateTime targetDate)
        {
            SetName(name);
            SetValue(value);
            SetUnit(unit);
            SetTargetDate(targetDate);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: StrategicPlanningConsts.MaxMetricNameLength
            );
        }

        private void SetValue(decimal value)
        {
            if (value <= 0)
            {
                throw new BusinessException("StrategicPlanning:ErrorMessage:InvalidTargetValue");
            }
            Value = value;
        }

        private void SetUnit(string unit)
        {
            Unit = Check.NotNullOrEmpty(unit, nameof(unit));
        }

        private void SetTargetDate(DateTime targetDate)
        {
            if (targetDate <= DateTime.UtcNow)
            {
                throw new BusinessException("StrategicPlanning:ErrorMessage:InvalidTargetDate");
            }
            TargetDate = targetDate;
        }

        public bool IsAchieved(decimal currentValue)
        {
            return currentValue >= Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Value;
            yield return Unit;
            yield return TargetDate;
        }
    }
}
