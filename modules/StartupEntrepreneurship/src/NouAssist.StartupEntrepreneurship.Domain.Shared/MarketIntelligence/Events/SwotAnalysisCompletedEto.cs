using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events.Base;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Events
{
    /// <summary>
    /// Event published when SWOT analysis is completed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysisCompleted")]
    public class SwotAnalysisCompletedEto : MarketIntelligenceEtoBase
    {
        /// <summary>
        /// Analysis title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Number of items analyzed
        /// </summary>
        public int ItemsCount { get; set; }

        /// <summary>
        /// Analysis completion date
        /// </summary>
        public DateTime CompletedDate { get; set; }

        /// <summary>
        /// Overall strategic recommendation
        /// </summary>
        public string StrategicRecommendation { get; set; }
    }
}
