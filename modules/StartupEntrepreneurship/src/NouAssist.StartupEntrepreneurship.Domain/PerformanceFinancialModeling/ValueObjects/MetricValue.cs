using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents a metric value with its data type and formatting
    /// </summary>
    public class MetricValue : ValueObject
    {
        public decimal Value { get; private set; }
        public MetricDataType DataType { get; private set; }
        public string Unit { get; private set; }

        private MetricValue() { }

        public MetricValue(decimal value, MetricDataType dataType, string unit = null)
        {
            SetValue(value, dataType);
            SetUnit(unit);
        }

        public override string ToString()
        {
            return DataType switch
            {
                MetricDataType.Percentage => $"{Value:F2}%",
                MetricDataType.Currency => $"${Value:N2}",
                MetricDataType.Count => $"{Value:N0}",
                MetricDataType.Ratio => FormatRatio(),
                MetricDataType.Duration => FormatDuration(),
                _ => $"{Value:F2}{(string.IsNullOrEmpty(Unit) ? "" : $" {Unit}")}"
            };
        }

        public bool IsGreaterThan(MetricValue other)
        {
            EnsureCompatible(other);
            return Value > other.Value;
        }

        public bool IsLessThan(MetricValue other)
        {
            EnsureCompatible(other);
            return Value < other.Value;
        }

        public MetricValue Add(MetricValue other)
        {
            EnsureCompatible(other);
            return new MetricValue(Value + other.Value, DataType, Unit);
        }

        public MetricValue Subtract(MetricValue other)
        {
            EnsureCompatible(other);
            return new MetricValue(Value - other.Value, DataType, Unit);
        }

        public decimal GetPercentageChange(MetricValue previous)
        {
            EnsureCompatible(previous);
            if (previous.Value == 0) return 0;
            return ((Value - previous.Value) / previous.Value) * 100;
        }

        private void SetValue(decimal value, MetricDataType dataType)
        {
            if (dataType == MetricDataType.Percentage && (value < 0 || value > 100))
            {
                throw new ArgumentException("Percentage values must be between 0 and 100");
            }

            Value = value;
            DataType = dataType;
        }

        private void SetUnit(string unit)
        {
            if (!string.IsNullOrEmpty(unit))
            {
                Unit = unit.Trim();
                if (Unit.Length > PerformanceFinancialModelingConsts.MaxMetricUnitLength)
                {
                    throw new ArgumentException($"Unit cannot exceed {PerformanceFinancialModelingConsts.MaxMetricUnitLength} characters");
                }
            }
        }

        private void EnsureCompatible(MetricValue other)
        {
            if (DataType != other.DataType)
            {
                throw new InvalidOperationException("Cannot compare metrics with different data types");
            }
        }

        private string FormatRatio()
        {
            // Simple ratio formatting - can be enhanced based on requirements
            return $"{Value:F2}:1";
        }

        private string FormatDuration()
        {
            // Convert decimal value to meaningful duration - can be enhanced
            var days = (int)Value;
            var hours = (int)((Value - days) * 24);
            return $"{days}d {hours}h";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return DataType;
            yield return Unit;
        }
    }
}
