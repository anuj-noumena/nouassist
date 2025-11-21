using System;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events.Base
{
    /// <summary>
    /// Base class for Decision Support integration events
    /// </summary>
    [Serializable]
    public abstract class DecisionSupportEtoBase
    {
        /// <summary>
        /// Unique identifier of the entity
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The tenant ID associated with this entity, if multi-tenancy is enabled
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
