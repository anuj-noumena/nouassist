using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos
{
    /// <summary>
    /// DTO for getting a list of market trends with filtering options
    /// </summary>
    public class GetMarketTrendListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter trends by title, description, or keywords
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter by trend category
        /// </summary>
        public TrendCategory? Category { get; set; }

        /// <summary>
        /// Filter by impact level
        /// </summary>
        public ImpactLevel? ImpactLevel { get; set; }

        /// <summary>
        /// Filter for active trends only
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Minimum confidence score to filter by
        /// </summary>
        public decimal? MinConfidenceScore { get; set; }

        /// <summary>
        /// Filter for trends valid after this date
        /// </summary>
        public DateTime? ValidAfterDate { get; set; }

        /// <summary>
        /// Whether to include data sources in the results
        /// </summary>
        public bool IncludeDataSources { get; set; } = false;
    }
}
