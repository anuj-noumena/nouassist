using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates
{
    /// <summary>
    /// Aggregate root for tracking business metrics with goals and data points
    /// </summary>
    public class TrackedMetric : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public MetricType Type { get; private set; }
        public MetricDataType DataType { get; private set; }
        public string Unit { get; private set; }
        public MetricFrequency Frequency { get; private set; }
        public bool IsActive { get; private set; }
        public Guid DefinitionId { get; private set; }
        public MetricValue? LastValue { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }

        // Collections
        private readonly List<MetricDataPoint> _dataPoints;
        private readonly List<MetricGoal> _goals;

        public IReadOnlyCollection<MetricDataPoint> DataPoints => _dataPoints.AsReadOnly();
        public IReadOnlyCollection<MetricGoal> Goals => _goals.AsReadOnly();

        protected TrackedMetric()
        {
            _dataPoints = new List<MetricDataPoint>();
            _goals = new List<MetricGoal>();
        }

        public static TrackedMetric Create(
            Guid id,
            string name,
            string description,
            MetricType type,
            MetricDataType dataType,
            MetricFrequency frequency,
            Guid definitionId,
            string unit = null,
            Guid? tenantId = null)
        {
            return new TrackedMetric(id, name, description, type, dataType, frequency, definitionId, unit, tenantId);
        }

        private TrackedMetric(
            Guid id,
            string name,
            string description,
            MetricType type,
            MetricDataType dataType,
            MetricFrequency frequency,
            Guid definitionId,
            string unit,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Type = type;
            DataType = dataType;
            Frequency = frequency;
            DefinitionId = definitionId;
            Unit = unit?.Trim();
            TenantId = tenantId;
            IsActive = true;
            _dataPoints = new List<MetricDataPoint>();
            _goals = new List<MetricGoal>();

            AddDistributedEvent(new MetricCreatedEto
            {
                Id = Id,
                Name = Name,
                Type = Type,
                Unit = Unit,
                DefinitionId = DefinitionId,
                TenantId = TenantId
            });
        }

        public void UpdateBasicInfo(string name, string description, string unit = null)
        {
            SetName(name);
            SetDescription(description);
            Unit = unit?.Trim();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void AddDataPoint(decimal value, DateTime timestamp, string notes = null)
        {
            if (!IsActive)
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:InactiveMetric");
            }

            var metricValue = new MetricValue(value, DataType, Unit);
            var dataPoint = new MetricDataPoint(Id, metricValue, timestamp, notes);
            
            _dataPoints.Add(dataPoint);
            LastValue = metricValue;
            LastUpdatedAt = timestamp;

            // Check if any goals are achieved
            CheckGoalAchievement(metricValue);

            AddDistributedEvent(new MetricDataPointAddedEto
            {
                Id = Id,
                MetricId = Id,
                MetricName = Name,
                Value = value,
                Timestamp = timestamp,
                Notes = notes,
                TenantId = TenantId
            });
        }

        public void SetGoal(MetricValue targetValue, DateTime targetDate, ThresholdType thresholdType,
                          string description = null, MetricValue secondaryValue = null)
        {
            var goalTarget = new GoalTarget(targetValue, targetDate, thresholdType, secondaryValue);
            var goal = new MetricGoal(Id, goalTarget, description);
            
            // Remove any existing active goals for the same target date
            var existingGoal = _goals.FirstOrDefault(g => g.IsActive && g.Target.TargetDate == targetDate);
            if (existingGoal != null)
            {
                existingGoal.Deactivate();
            }
            
            _goals.Add(goal);

            AddDistributedEvent(new MetricGoalSetEto
            {
                Id = Id,
                MetricId = Id,
                MetricName = Name,
                TargetValue = targetValue.Value,
                TargetDate = targetDate,
                Description = description,
                TenantId = TenantId
            });
        }

        public MetricGoal GetActiveGoal()
        {
            return _goals.FirstOrDefault(g => g.IsActive && !g.Target.IsOverdue());
        }

        public List<MetricDataPoint> GetDataPointsInPeriod(DateTime startDate, DateTime endDate)
        {
            return _dataPoints
                .Where(dp => dp.Timestamp >= startDate && dp.Timestamp <= endDate)
                .OrderBy(dp => dp.Timestamp)
                .ToList();
        }

        public decimal? GetTrend(int periodDays = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var recentPoints = _dataPoints
                .Where(dp => dp.Timestamp >= cutoffDate)
                .OrderBy(dp => dp.Timestamp)
                .ToList();

            if (recentPoints.Count < 2) return null;

            var firstValue = recentPoints.First().Value.Value;
            var lastValue = recentPoints.Last().Value.Value;

            if (firstValue == 0) return null;
            return ((lastValue - firstValue) / firstValue) * 100;
        }

        public MetricValue? GetAverageValue(int periodDays = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var recentPoints = _dataPoints
                .Where(dp => dp.Timestamp >= cutoffDate)
                .ToList();

            if (!recentPoints.Any()) return null;

            var average = recentPoints.Average(dp => dp.Value.Value);
            return new MetricValue(average, DataType, Unit);
        }

        private void CheckGoalAchievement(MetricValue currentValue)
        {
            var activeGoal = GetActiveGoal();
            if (activeGoal != null && !activeGoal.IsAchieved && activeGoal.Target.IsAchieved(currentValue))
            {
                activeGoal.MarkAsAchieved();
                AddDistributedEvent(new MetricGoalAchievedEto
                {
                    Id = Id,
                    MetricId = Id,
                    MetricName = Name,
                    GoalId = activeGoal.Id,
                    CurrentValue = currentValue.Value,
                    TargetValue = activeGoal.Target.TargetValue.Value,
                    TenantId = TenantId
                });
            }
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: PerformanceFinancialModelingConsts.MaxMetricNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: PerformanceFinancialModelingConsts.MaxMetricDescriptionLength
            );
        }
    }
}
