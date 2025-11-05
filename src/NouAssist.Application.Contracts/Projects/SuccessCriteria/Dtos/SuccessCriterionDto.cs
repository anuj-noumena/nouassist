using System;
using System.Collections.Generic;
using NouAssist.Projects.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Projects.SuccessCriteria.Dtos
{
    public class SuccessCriterionDto : FullAuditedEntityDto<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SuccessCriterionType CriterionType { get; set; }

        public string MeasurementMethod { get; set; }
        public string TargetValue { get; set; }
        public string ActualValue { get; set; }
        public string Unit { get; set; }
        public double? TargetNumeric { get; set; }
        public double? ActualNumeric { get; set; }

        public double Weight { get; set; }
        public int OrderIndex { get; set; }
        public bool IsMet { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? MetAt { get; set; }
        public string MetBy { get; set; }

        public string Notes { get; set; }
        public string EvidenceUrl { get; set; }
        public List<Guid> AttachmentIds { get; set; }

        // Computed
        public bool IsOverdue { get; set; }
        public double? AchievementPercentage { get; set; }
    }
}
