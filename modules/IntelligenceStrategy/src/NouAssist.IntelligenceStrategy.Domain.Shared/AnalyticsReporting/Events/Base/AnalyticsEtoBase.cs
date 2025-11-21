using System;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Events.Base
{
    /// <summary>
    /// Base class for all Analytics module event transfer objects
    /// </summary>
    [Serializable]
    public abstract class AnalyticsEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
    }
}
