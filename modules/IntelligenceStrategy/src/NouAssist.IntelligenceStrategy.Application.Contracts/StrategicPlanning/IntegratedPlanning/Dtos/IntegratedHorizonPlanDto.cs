using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning.Dtos
{
    /// <summary>
    /// DTO for Integrated Horizon Plan output
    /// </summary>
    public class IntegratedHorizonPlanDto : FullAuditedEntityDto<Guid>
    {
        public string PlanName { get; set; }
        public string Description { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public PlanningHorizon PrimaryHorizon { get; set; }
        public string VisionStatement { get; set; }
        public bool IsActive { get; set; }
        public List<TimelineEventMarkerDto> TimelineEvents { get; set; }
        public List<string> FutureSelfDialogues { get; set; }

        public IntegratedHorizonPlanDto()
        {
            TimelineEvents = new List<TimelineEventMarkerDto>();
            FutureSelfDialogues = new List<string>();
        }
    }
}
