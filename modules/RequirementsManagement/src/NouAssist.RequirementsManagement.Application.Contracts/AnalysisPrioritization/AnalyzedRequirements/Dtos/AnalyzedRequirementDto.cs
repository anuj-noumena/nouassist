using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO representing an analyzed requirement with its attributes and relationships
    /// </summary>
    public class AnalyzedRequirementDto : FullAuditedEntityDto<Guid>
    {
        public Guid RequirementId { get; set; }
        public string RequirementTitle { get; set; } = string.Empty;
        public ImpactAssessmentDto? ImpactAssessment { get; set; }
        public DateTime? LastAnalyzedAt { get; set; }
        public Guid? LastAnalyzedBy { get; set; }
        public List<RequirementAttributeDto> Attributes { get; set; } = new();
        public List<RelationshipLinkDto> Relationships { get; set; } = new();
    }
}
