using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos
{
    /// <summary>
    /// DTO for Contingency Plan Item output
    /// </summary>
    public class ContingencyPlanItemDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public Guid? ResponsibleUserId { get; set; }
        public List<string> ActionSteps { get; set; }
        public List<string> RequiredResources { get; set; }
        
        // Risk Scenario properties
        public string RiskDescription { get; set; }
        public CrisisLevel RiskLevel { get; set; }
        public decimal ProbabilityScore { get; set; }
        public decimal ImpactScore { get; set; }
        public decimal RiskScore { get; set; }

        public ContingencyPlanItemDto()
        {
            ActionSteps = new List<string>();
            RequiredResources = new List<string>();
        }
    }
}
