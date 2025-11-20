using System;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos
{
    /// <summary>
    /// DTO for ESG Goal Record output
    /// </summary>
    public class EsgGoalRecordDto : EntityDto<Guid>
    {
        public EsgCategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TargetMetricName { get; set; }
        public decimal TargetValue { get; set; }
        public string TargetUnit { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal? CurrentValue { get; set; }
        public DateTime? LastMeasurementDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public bool IsOnTrack { get; set; }
    }
}
