using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for updating a competitor
    /// </summary>
    public class UpdateCompetitorDto
    {
        /// <summary>
        /// Competitor name
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.Competitor.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Competitor description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.Competitor.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Competitor info
        /// </summary>
        [Required]
        public CreateCompetitorInfoDto CompetitorInfo { get; set; }

        /// <summary>
        /// Tags for the competitor
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxTagsLength)]
        public string Tags { get; set; }

        /// <summary>
        /// Notes about the competitor
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxNotesLength)]
        public string Notes { get; set; }
    }
}
