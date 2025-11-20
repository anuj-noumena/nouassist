using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos
{
    /// <summary>
    /// DTO for Opportunity Filter Rule output
    /// </summary>
    public class OpportunityFilterRuleDto : EntityDto<Guid>
    {
        public string RuleName { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CriterionName { get; set; }
        public string CriterionDescription { get; set; }
        public decimal MinThreshold { get; set; }
        public decimal MaxThreshold { get; set; }
        public int CriterionWeight { get; set; }
    }
}
