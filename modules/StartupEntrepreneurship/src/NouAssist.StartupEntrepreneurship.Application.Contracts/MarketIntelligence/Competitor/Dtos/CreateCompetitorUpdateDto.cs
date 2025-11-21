using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for creating a competitor update log
    /// </summary>
    public class CreateCompetitorUpdateDto
    {
        /// <summary>
        /// The type of update
        /// </summary>
        [Required]
        public UpdateType Type { get; set; }

        /// <summary>
        /// Update title
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// Update description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Information source
        /// </summary>
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)]
        public string Source { get; set; }

        /// <summary>
        /// Impact level of the update
        /// </summary>
        public ImpactLevel ImpactLevel { get; set; } = ImpactLevel.Low;

        /// <summary>
        /// Tags for the update
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxTagsLength)]
        public string Tags { get; set; }
    }
}
