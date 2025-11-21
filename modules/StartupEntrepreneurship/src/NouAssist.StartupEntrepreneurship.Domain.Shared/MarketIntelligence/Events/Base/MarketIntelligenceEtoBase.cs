using System;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Events.Base
{
    /// <summary>
    /// Base class for Market Intelligence domain events
    /// </summary>
    [Serializable]
    public abstract class MarketIntelligenceEtoBase
    {
        /// <summary>
        /// Entity identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Tenant identifier for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Event timestamp
        /// </summary>
        public DateTime EventTime { get; set; }

        protected MarketIntelligenceEtoBase()
        {
            EventTime = DateTime.UtcNow;
        }
    }
}
