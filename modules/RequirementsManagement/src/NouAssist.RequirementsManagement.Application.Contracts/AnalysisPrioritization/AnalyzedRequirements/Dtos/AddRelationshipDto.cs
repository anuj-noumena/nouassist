using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO for adding a relationship between requirements
    /// </summary>
    public class AddRelationshipDto
    {
        [Required]
        public Guid TargetRequirementId { get; set; }

        [Required]
        public RelationshipType RelationshipType { get; set; }

        [StringLength(AnalysisConsts.MaxRelationshipDescriptionLength)]
        public string? Description { get; set; }
    }
}
