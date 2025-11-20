using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos
{
    /// <summary>
    /// DTO for market trend data
    /// </summary>
    public class MarketTrendDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Trend title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Trend description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Keywords related to the trend
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Source URL for the trend
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// Trend category
        /// </summary>
        public TrendCategory Category { get; set; }

        /// <summary>
        /// Trend impact level
        /// </summary>
        public ImpactLevel ImpactLevel { get; set; }

        /// <summary>
        /// Impact description
        /// </summary>
        public string ImpactDescription { get; set; }

        /// <summary>
        /// Confidence score for the trend (0-1 scale)
        /// </summary>
        public decimal ConfidenceScore { get; set; }

        /// <summary>
        /// Recommended actions based on the trend
        /// </summary>
        public string RecommendedActions { get; set; }

        /// <summary>
        /// Date when the trend was identified
        /// </summary>
        public DateTime IdentifiedDate { get; set; }

        /// <summary>
        /// Expiration date for the trend
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Whether the trend is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Tags for the trend
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Data sources for the trend
        /// </summary>
        public List<TrendDataSourceDto> DataSources { get; set; }
    }
}
