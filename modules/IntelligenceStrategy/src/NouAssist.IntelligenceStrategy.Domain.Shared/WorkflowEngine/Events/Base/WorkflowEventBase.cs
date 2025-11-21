using System;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base
{
    /// <summary>
    /// Base class for all workflow-related domain events
    /// </summary>
    [Serializable]
    public abstract class WorkflowEventBase
    {
        /// <summary>
        /// The unique identifier of the entity that triggered the event
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The tenant ID associated with this event
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
