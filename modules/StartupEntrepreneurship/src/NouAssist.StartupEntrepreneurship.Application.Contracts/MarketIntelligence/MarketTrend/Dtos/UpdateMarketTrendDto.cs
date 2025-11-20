using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos
{
    /// <summary>
    /// DTO for updating a market trend
    /// </summary>
    public class UpdateMarketTrendDto
    {
        /// <summary>
        /// Trend title
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// Trend description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Keywords related to the trend
        /// </summary>
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxKeywordsLength)]
        public string Keywords { get; set; }

        /// <summary>
        /// Source URL for the trend
        /// </summary>
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// Impact level of the trend
        /// </summary>
        [Required]
        public ImpactLevel ImpactLevel { get; set; }

        /// <summary>
        /// Impact description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxImpactDescriptionLength)]
        public string ImpactDescription { get; set; }

        /// <summary>
        /// Confidence score (0-1 scale)
        /// </summary>
        [Range(0.0, 1.0, ErrorMessage = "Confidence score must be between 0 and 1")]
        public decimal ConfidenceScore { get; set; }

        /// <summary>
        /// Recommended actions based on the trend
        /// </summary>
        [StringLength(MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength)]
        public string RecommendedActions { get; set; }

        /// <summary>
        /// Expiry date for the trend
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Tags for the trend
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxTagsLength)]
        public string Tags { get; set; }
    }
}
