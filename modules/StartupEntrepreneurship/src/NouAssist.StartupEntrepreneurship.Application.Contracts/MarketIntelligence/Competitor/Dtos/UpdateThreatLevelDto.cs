using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for updating a competitor's threat level
    /// </summary>
    public class UpdateThreatLevelDto
    {
        /// <summary>
        /// New threat level to set
        /// </summary>
        [Required]
        public CompetitorThreatLevel ThreatLevel { get; set; }

        /// <summary>
        /// Reason for the threat level change
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength)]
        public string Reason { get; set; }
    }
}
