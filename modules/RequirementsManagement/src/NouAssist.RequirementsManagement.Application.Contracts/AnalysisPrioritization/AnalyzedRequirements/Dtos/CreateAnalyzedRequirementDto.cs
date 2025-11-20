using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO for creating a new analyzed requirement
    /// </summary>
    public class CreateAnalyzedRequirementDto
    {
        [Required]
        public Guid RequirementId { get; set; }

        [Required]
        [StringLength(500)]
        public string RequirementTitle { get; set; } = string.Empty;
    }
}
