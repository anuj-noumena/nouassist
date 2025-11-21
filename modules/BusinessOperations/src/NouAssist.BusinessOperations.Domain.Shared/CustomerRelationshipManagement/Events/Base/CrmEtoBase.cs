using System;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Events.Base
{
    /// <summary>
    /// Base class for all CRM event transfer objects
    /// </summary>
    [Serializable]
    public abstract class CrmEtoBase
    {
        /// <summary>
        /// The ID of the entity that triggered the event
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The tenant ID associated with the event
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
