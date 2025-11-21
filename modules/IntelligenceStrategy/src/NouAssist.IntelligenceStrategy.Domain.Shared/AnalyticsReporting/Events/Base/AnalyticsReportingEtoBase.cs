using System;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base
{
    /// <summary>
    /// Base class for all AnalyticsReporting event transfer objects (ETOs)
    /// </summary>
    public abstract class AnalyticsReportingEtoBase
    {
        /// <summary>
        /// Unique identifier of the entity
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Optional tenant identifier for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
