using System;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents an Environmental, Social, and Governance goal
    /// </summary>
    public class EsgGoalRecord : Entity<Guid>
    {
        public Guid SustainableGrowthFrameworkId { get; private set; }
        public EsgCategory Category { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TargetMetric Target { get; private set; }
        public decimal? CurrentValue { get; private set; }
        public DateTime? LastMeasurementDate { get; private set; }

        protected EsgGoalRecord() { }

        internal EsgGoalRecord(
            Guid id,
            Guid sustainableGrowthFrameworkId,
            EsgCategory category,
            string title,
            string description,
            TargetMetric target) : base(id)
        {
            SustainableGrowthFrameworkId = sustainableGrowthFrameworkId;
            Category = category;
            SetTitle(title);
            SetDescription(description);
            Target = target;
        }

        internal void RecordMeasurement(decimal value, DateTime measurementDate)
        {
            CurrentValue = value;
            LastMeasurementDate = measurementDate;
        }

        public decimal ProgressPercentage => 
            CurrentValue.HasValue ? Math.Min(100, (CurrentValue.Value / Target.Value) * 100) : 0;

        public bool IsOnTrack => CurrentValue.HasValue && CurrentValue.Value >= GetExpectedValue();

        private decimal GetExpectedValue()
        {
            if (!LastMeasurementDate.HasValue) return 0;

            var totalDays = (Target.TargetDate - DateTime.UtcNow).TotalDays;
            var elapsedDays = (LastMeasurementDate.Value - DateTime.UtcNow).TotalDays;
            
            if (totalDays <= 0) return Target.Value;
            
            var expectedProgress = Math.Max(0, elapsedDays / totalDays);
            return Target.Value * (decimal)expectedProgress;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: StrategicPlanningConsts.MaxGoalTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: StrategicPlanningConsts.MaxEsgGoalDescriptionLength
            );
        }
    }
}
