using System;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events.Base
{
    /// <summary>
    /// Base class for all Personal Productivity Intelligence event transfer objects
    /// </summary>
    [Serializable]
    public abstract class ProductivityIntelligenceEtoBase
    {
        /// <summary>
        /// User ID associated with the event
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Tenant ID (for multi-tenancy support)
        /// </summary>
        public Guid? TenantId { get; set; }
        
        /// <summary>
        /// Timestamp when the event occurred
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
