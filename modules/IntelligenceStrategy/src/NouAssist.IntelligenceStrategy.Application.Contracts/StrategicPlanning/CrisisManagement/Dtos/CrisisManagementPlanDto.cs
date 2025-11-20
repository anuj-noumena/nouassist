using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos
{
    /// <summary>
    /// DTO for Crisis Management Plan output
    /// </summary>
    public class CrisisManagementPlanDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CrisisLevel MaxCrisisLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public Guid? ActivatedBy { get; set; }
        public DateTime? LastReviewDate { get; set; }
        public DateTime NextReviewDate { get; set; }
        public bool IsDueForReview { get; set; }
        public List<ContingencyPlanItemDto> ContingencyItems { get; set; }
        public List<CrisisProtocolStepDto> ProtocolSteps { get; set; }

        public CrisisManagementPlanDto()
        {
            ContingencyItems = new List<ContingencyPlanItemDto>();
            ProtocolSteps = new List<CrisisProtocolStepDto>();
        }
    }
}
