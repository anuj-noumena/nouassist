using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos
{
    /// <summary>
    /// DTO for market opportunity alerts
    /// </summary>
    public class OpportunityAlertDto : EntityDto<Guid>
    {
        /// <summary>
        /// Source trend ID
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// Opportunity title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Opportunity description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Impact level
        /// </summary>
        public ImpactLevel ImpactLevel { get; set; }

        /// <summary>
        /// Confidence score (0-1 scale)
        /// </summary>
        public decimal ConfidenceScore { get; set; }

        /// <summary>
        /// Recommended actions
        /// </summary>
        public string RecommendedActions { get; set; }

        /// <summary>
        /// Trend category
        /// </summary>
        public TrendCategory Category { get; set; }

        /// <summary>
        /// Alert type
        /// </summary>
        public string AlertType { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
