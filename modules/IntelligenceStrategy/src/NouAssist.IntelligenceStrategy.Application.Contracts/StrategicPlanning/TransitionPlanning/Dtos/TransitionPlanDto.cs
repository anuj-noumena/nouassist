using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning.Dtos
{
    /// <summary>
    /// DTO for Transition Plan output
    /// </summary>
    public class TransitionPlanDto : FullAuditedEntityDto<Guid>
    {
        public string PlanName { get; set; }
        public string Description { get; set; }
        public TransitionType Type { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public string CurrentPhase { get; set; }
        public decimal EstimatedCost { get; set; }
        public bool IsActive { get; set; }
        public decimal OverallProgress { get; set; }
        public decimal RemainingCost { get; set; }
        public List<CapabilityRoadmapItemDto> CapabilityRoadmap { get; set; }
        public List<string> Milestones { get; set; }

        public TransitionPlanDto()
        {
            CapabilityRoadmap = new List<CapabilityRoadmapItemDto>();
            Milestones = new List<string>();
        }
    }
}
