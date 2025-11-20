using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO for updating an analyzed requirement
    /// </summary>
    public class UpdateAnalyzedRequirementDto
    {
        [Required]
        [StringLength(500)]
        public string RequirementTitle { get; set; } = string.Empty;
    }
}
