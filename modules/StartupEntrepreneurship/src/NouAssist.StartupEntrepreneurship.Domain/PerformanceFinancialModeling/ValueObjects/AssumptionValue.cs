using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents an assumption value used in financial modeling
    /// </summary>
    public class AssumptionValue : ValueObject
    {
        public string Name { get; private set; }
        public decimal Value { get; private set; }
        public string Unit { get; private set; }
        public string Description { get; private set; }
        public decimal? MinValue { get; private set; }
        public decimal? MaxValue { get; private set; }

        private AssumptionValue() { }

        public AssumptionValue(string name, decimal value, string unit = null, string description = null, 
                             decimal? minValue = null, decimal? maxValue = null)
        {
            SetName(name);
            SetValue(value, minValue, maxValue);
            Unit = unit?.Trim();
            Description = description?.Trim();
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public bool IsWithinRange()
        {
            if (MinValue.HasValue && Value < MinValue.Value) return false;
            if (MaxValue.HasValue && Value > MaxValue.Value) return false;
            return true;
        }

        public AssumptionValue WithValue(decimal newValue)
        {
            return new AssumptionValue(Name, newValue, Unit, Description, MinValue, MaxValue);
        }

        public AssumptionValue WithRange(decimal? minValue, decimal? maxValue)
        {
            return new AssumptionValue(Name, Value, Unit, Description, minValue, maxValue);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: PerformanceFinancialModelingConsts.MaxAssumptionNameLength
            );
        }

        private void SetValue(decimal value, decimal? minValue, decimal? maxValue)
        {
            if (minValue.HasValue && value < minValue.Value)
            {
                throw new ArgumentException($"Value {value} is below minimum allowed value {minValue.Value}");
            }
            
            if (maxValue.HasValue && value > maxValue.Value)
            {
                throw new ArgumentException($"Value {value} is above maximum allowed value {maxValue.Value}");
            }
            
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Value;
            yield return Unit;
            yield return Description;
            yield return MinValue;
            yield return MaxValue;
        }
    }
}
