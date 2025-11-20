using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for completing a SWOT analysis
    /// </summary>
    public class CompleteSwotAnalysisDto
    {
        /// <summary>
        /// Strategic recommendation based on the analysis
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxDescriptionLength)]
        public string StrategicRecommendation { get; set; }
    }
}
