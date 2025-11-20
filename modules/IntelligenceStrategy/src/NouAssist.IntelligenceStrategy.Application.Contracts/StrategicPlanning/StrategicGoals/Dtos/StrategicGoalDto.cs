using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for Strategic Goal Definition output
    /// </summary>
    public class StrategicGoalDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public GoalStatus Status { get; set; }
        public GoalPriority Priority { get; set; }
        public PlanningHorizon Horizon { get; set; }
        public DateTime TargetDate { get; set; }
        public Guid? OwnerId { get; set; }
        public string Notes { get; set; }
        public List<string> Tags { get; set; }
        public List<GoalTargetDto> Targets { get; set; }
        public decimal OverallProgress { get; set; }
        public bool IsOnTrack { get; set; }

        public StrategicGoalDto()
        {
            Tags = new List<string>();
            Targets = new List<GoalTargetDto>();
        }
    }
}
