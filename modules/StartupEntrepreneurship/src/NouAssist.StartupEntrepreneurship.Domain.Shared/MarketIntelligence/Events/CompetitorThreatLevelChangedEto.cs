using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Events
{
    /// <summary>
    /// Event published when competitor threat level changes
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.MarketIntelligence.CompetitorThreatLevelChanged")]
    public class CompetitorThreatLevelChangedEto : MarketIntelligenceEtoBase
    {
        /// <summary>
        /// Competitor name
        /// </summary>
        public string CompetitorName { get; set; }

        /// <summary>
        /// Previous threat level
        /// </summary>
        public CompetitorThreatLevel PreviousThreatLevel { get; set; }

        /// <summary>
        /// New threat level
        /// </summary>
        public CompetitorThreatLevel NewThreatLevel { get; set; }

        /// <summary>
        /// Reason for change
        /// </summary>
        public string ChangeReason { get; set; }
    }
}
