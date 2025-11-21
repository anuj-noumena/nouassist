using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Defines an impact metric for measuring strategic pivot outcomes
    /// </summary>
    public class ImpactMetricDefinition : Entity<Guid>
    {
        public Guid PivotId { get; private set; }
        public string MetricName { get; private set; }
        public string Description { get; private set; }
        public string Unit { get; private set; }
        public string MeasurementMethod { get; private set; }
        public decimal? BaselineValue { get; private set; }
        public decimal? TargetValue { get; private set; }

        private readonly List<ImpactValue> _measurements;
        public IReadOnlyCollection<ImpactValue> Measurements => _measurements.AsReadOnly();

        protected ImpactMetricDefinition()
        {
            _measurements = new List<ImpactValue>();
        }

        internal ImpactMetricDefinition(
            Guid id,
            Guid pivotId,
            string metricName,
            string description,
            string unit,
            string measurementMethod,
            decimal? baselineValue = null,
            decimal? targetValue = null) : base(id)
        {
            PivotId = pivotId;
            SetMetricName(metricName);
            Description = description;
            SetUnit(unit);
            MeasurementMethod = measurementMethod;
            BaselineValue = baselineValue;
            TargetValue = targetValue;
            _measurements = new List<ImpactValue>();
        }

        private void SetMetricName(string metricName)
        {
            MetricName = Check.NotNullOrWhiteSpace(metricName, nameof(metricName), maxLength: 100);
        }

        private void SetUnit(string unit)
        {
            Unit = Check.NotNullOrWhiteSpace(unit, nameof(unit), maxLength: 50);
        }

        public void AddMeasurement(ImpactValue measurement)
        {
            if (measurement == null)
                throw new ArgumentNullException(nameof(measurement));

            if (measurement.MetricName != MetricName)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "MetricName")
                    .WithData("expected", MetricName)
                    .WithData("actual", measurement.MetricName);
            }

            _measurements.Add(measurement);
        }

        public void UpdateTargetValue(decimal targetValue)
        {
            TargetValue = targetValue;
        }

        public void UpdateBaselineValue(decimal baselineValue)
        {
            BaselineValue = baselineValue;
        }

        public decimal? GetLatestMeasurement()
        {
            return _measurements
                .OrderByDescending(m => m.MeasuredAt)
                .FirstOrDefault()?.Value;
        }

        public double? CalculateProgressToTarget()
        {
            if (!BaselineValue.HasValue || !TargetValue.HasValue)
                return null;

            var latestValue = GetLatestMeasurement();
            if (!latestValue.HasValue)
                return null;

            var totalChange = TargetValue.Value - BaselineValue.Value;
            if (totalChange == 0)
                return 100.0; // Already at target

            var actualChange = latestValue.Value - BaselineValue.Value;
            return (double)(actualChange / totalChange * 100);
        }
    }
}
