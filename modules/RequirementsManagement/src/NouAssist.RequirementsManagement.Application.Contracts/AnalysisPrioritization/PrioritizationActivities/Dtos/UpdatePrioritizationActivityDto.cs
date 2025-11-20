using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO for updating a prioritization activity
    /// </summary>
    public class UpdatePrioritizationActivityDto
    {
        [Required]
        [StringLength(AnalysisConsts.MaxPrioritizationActivityNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(AnalysisConsts.MaxPrioritizationActivityDescriptionLength)]
        public string Description { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Instructions { get; set; }
    }
}
