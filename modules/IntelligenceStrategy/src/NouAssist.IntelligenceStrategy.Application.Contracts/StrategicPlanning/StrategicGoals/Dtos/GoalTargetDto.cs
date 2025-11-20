using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for Goal Target output
    /// </summary>
    public class GoalTargetDto : EntityDto<Guid>
    {
        public string MetricName { get; set; }
        public decimal TargetValue { get; set; }
        public string Unit { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal? CurrentValue { get; set; }
        public bool IsAchieved { get; set; }
        public DateTime? AchievedDate { get; set; }
        public string Notes { get; set; }
    }
}
