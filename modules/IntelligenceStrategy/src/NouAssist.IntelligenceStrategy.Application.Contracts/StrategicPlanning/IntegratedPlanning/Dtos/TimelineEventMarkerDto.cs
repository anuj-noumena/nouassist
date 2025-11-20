using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning.Dtos
{
    /// <summary>
    /// DTO for Timeline Event Marker output
    /// </summary>
    public class TimelineEventMarkerDto : EntityDto<Guid>
    {
        public DateTime EventDate { get; set; }
        public PlanningHorizon EventHorizon { get; set; }
        public string DateDescription { get; set; }
        public bool IsEstimatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public bool IsCriticalPath { get; set; }
        public string Dependencies { get; set; }
        public List<Guid> RelatedGoalIds { get; set; }

        public TimelineEventMarkerDto()
        {
            RelatedGoalIds = new List<Guid>();
        }
    }
}
