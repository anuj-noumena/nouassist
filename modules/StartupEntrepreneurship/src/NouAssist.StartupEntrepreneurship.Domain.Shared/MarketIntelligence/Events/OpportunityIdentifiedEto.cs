using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events.Base;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Events
{
    /// <summary>
    /// Event published when a market opportunity is identified
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.MarketIntelligence.OpportunityIdentified")]
    public class OpportunityIdentifiedEto : MarketIntelligenceEtoBase
    {
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
        /// Source that identified the opportunity
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Recommended actions
        /// </summary>
        public string RecommendedActions { get; set; }
    }
}
