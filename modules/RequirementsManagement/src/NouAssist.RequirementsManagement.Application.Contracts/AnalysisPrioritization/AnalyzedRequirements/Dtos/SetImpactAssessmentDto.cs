using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO for setting impact assessment on a requirement
    /// </summary>
    public class SetImpactAssessmentDto
    {
        [Required]
        public ImpactLevel TechnicalImpact { get; set; }

        [Required]
        public ImpactLevel BusinessImpact { get; set; }

        [Required]
        public ImpactLevel UserImpact { get; set; }

        [Required]
        public ImpactLevel RiskImpact { get; set; }

        [Required]
        [StringLength(AnalysisConsts.MaxImpactDescriptionLength)]
        public string Description { get; set; } = string.Empty;
    }
}
