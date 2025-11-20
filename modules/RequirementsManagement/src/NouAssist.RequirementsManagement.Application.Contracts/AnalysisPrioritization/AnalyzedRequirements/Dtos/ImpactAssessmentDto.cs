using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO representing impact assessment results
    /// </summary>
    public class ImpactAssessmentDto
    {
        public ImpactLevel TechnicalImpact { get; set; }
        public ImpactLevel BusinessImpact { get; set; }
        public ImpactLevel UserImpact { get; set; }
        public ImpactLevel RiskImpact { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal OverallScore { get; set; }
    }
}
