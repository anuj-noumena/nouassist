using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events.Base;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Events
{
    /// <summary>
    /// Event published when a new market trend is identified
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrendIdentified")]
    public class MarketTrendIdentifiedEto : MarketIntelligenceEtoBase
    {
        /// <summary>
        /// Trend title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Trend category
        /// </summary>
        public TrendCategory Category { get; set; }

        /// <summary>
        /// Impact level
        /// </summary>
        public ImpactLevel ImpactLevel { get; set; }

        /// <summary>
        /// Confidence score
        /// </summary>
        public decimal ConfidenceScore { get; set; }
    }
}
