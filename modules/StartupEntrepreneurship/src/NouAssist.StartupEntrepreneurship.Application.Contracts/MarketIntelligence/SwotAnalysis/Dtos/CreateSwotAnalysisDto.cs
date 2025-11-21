using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for creating a SWOT analysis
    /// </summary>
    public class CreateSwotAnalysisDto
    {
        /// <summary>
        /// Analysis title
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// Analysis description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Analysis notes
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxNotesLength)]
        public string AnalysisNotes { get; set; }
    }
}
