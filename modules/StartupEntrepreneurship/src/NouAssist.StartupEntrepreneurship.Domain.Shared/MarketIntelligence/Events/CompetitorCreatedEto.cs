using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events.Base;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Events
{
    /// <summary>
    /// Event published when a new competitor is added
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.MarketIntelligence.CompetitorCreated")]
    public class CompetitorCreatedEto : MarketIntelligenceEtoBase
    {
        /// <summary>
        /// Competitor name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Competitor website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Threat level
        /// </summary>
        public CompetitorThreatLevel ThreatLevel { get; set; }

        /// <summary>
        /// Industry
        /// </summary>
        public string Industry { get; set; }
    }
}
