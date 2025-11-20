using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning.Dtos
{
    /// <summary>
    /// DTO for Capability Roadmap Item output
    /// </summary>
    public class CapabilityRoadmapItemDto : EntityDto<Guid>
    {
        public string SkillName { get; set; }
        public string SkillCategory { get; set; }
        public string SkillDescription { get; set; }
        public CapabilityLevel CurrentLevel { get; set; }
        public CapabilityLevel TargetLevel { get; set; }
        public DateTime TargetDate { get; set; }
        public string DevelopmentPath { get; set; }
        public decimal InvestmentRequired { get; set; }
        public bool IsCompleted { get; set; }
        public decimal ProgressPercentage { get; set; }
        public List<string> RequiredActions { get; set; }

        public CapabilityRoadmapItemDto()
        {
            RequiredActions = new List<string>();
        }
    }
}
