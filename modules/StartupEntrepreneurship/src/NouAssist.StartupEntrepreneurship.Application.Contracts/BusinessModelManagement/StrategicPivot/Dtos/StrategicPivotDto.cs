using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class StrategicPivotDto : FullAuditedEntityDto<Guid>
    {
        public PivotType Type { get; set; }
        public PivotStatus Status { get; set; }
        public PivotRationaleDto Rationale { get; set; }
        public string Description { get; set; }
        public Guid InitiatedBy { get; set; }
        public DateTime InitiatedDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public string ActualOutcome { get; set; }
        public bool WasSuccessful { get; set; }
        public List<ImpactMetricDefinitionDto> ImpactMetrics { get; set; }
        public double OverallSuccessRate { get; set; }
        public bool IsOverdue { get; set; }
        public double? DurationInDays { get; set; }

        public StrategicPivotDto()
        {
            ImpactMetrics = new List<ImpactMetricDefinitionDto>();
        }
    }
}
