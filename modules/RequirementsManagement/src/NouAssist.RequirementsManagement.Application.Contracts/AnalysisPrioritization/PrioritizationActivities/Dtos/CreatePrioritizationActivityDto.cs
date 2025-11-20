using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO for creating a new prioritization activity
    /// </summary>
    public class CreatePrioritizationActivityDto
    {
        [Required]
        [StringLength(AnalysisConsts.MaxPrioritizationActivityNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(AnalysisConsts.MaxPrioritizationActivityDescriptionLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public PrioritizationMethod Method { get; set; }

        [StringLength(2000)]
        public string? Instructions { get; set; }
    }
}
