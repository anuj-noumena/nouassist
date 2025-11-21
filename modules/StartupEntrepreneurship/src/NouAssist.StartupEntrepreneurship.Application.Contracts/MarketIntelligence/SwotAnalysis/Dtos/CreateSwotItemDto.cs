using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for creating a SWOT item
    /// </summary>
    public class CreateSwotItemDto
    {
        /// <summary>
        /// SWOT category
        /// </summary>
        [Required]
        public SwotCategory Category { get; set; }

        /// <summary>
        /// Item title
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxItemTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// Item description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxItemDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Impact score (0-1 scale)
        /// </summary>
        [Range(0.0, 1.0, ErrorMessage = "Impact must be between 0 and 1")]
        public decimal Impact { get; set; } = 0.5m;

        /// <summary>
        /// Priority level (1-10, lower is higher priority)
        /// </summary>
        [Range(1, 10, ErrorMessage = "Priority must be between 1 and 10")]
        public int Priority { get; set; } = 1;

        /// <summary>
        /// Action items related to this SWOT item
        /// </summary>
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength)]
        public string ActionItems { get; set; }

        /// <summary>
        /// Tags for the item
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxTagsLength)]
        public string Tags { get; set; }
    }
}
