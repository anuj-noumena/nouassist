using System;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO for filtering analyzed requirements list
    /// </summary>
    public class GetAnalyzedRequirementListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public AttributeType? AttributeType { get; set; }
        public RelationshipType? RelationshipType { get; set; }
        public bool? HasImpactAssessment { get; set; }
        public DateTime? AnalyzedAfter { get; set; }
        public DateTime? AnalyzedBefore { get; set; }
    }
}
