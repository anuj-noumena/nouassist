using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos
{
    /// <summary>
    /// DTO for Opportunity Management Record output
    /// </summary>
    public class OpportunityManagementRecordDto : FullAuditedEntityDto<Guid>
    {
        public string RecordName { get; set; }
        public string Description { get; set; }
        public FocusLevel CurrentFocusLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<OpportunityFilterRuleDto> FilterRules { get; set; }
        public List<string> StrategicExperiments { get; set; }

        public OpportunityManagementRecordDto()
        {
            FilterRules = new List<OpportunityFilterRuleDto>();
            StrategicExperiments = new List<string>();
        }
    }
}
