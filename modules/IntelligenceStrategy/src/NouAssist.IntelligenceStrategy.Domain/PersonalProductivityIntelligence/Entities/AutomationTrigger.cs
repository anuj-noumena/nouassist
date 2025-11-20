using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a trigger for a personal automation
    /// </summary>
    public class AutomationTrigger : Entity<Guid>
    {
        /// <summary>
        /// ID of the automation this trigger belongs to
        /// </summary>
        public Guid AutomationId { get; protected set; }
        
        /// <summary>
        /// Type of trigger
        /// </summary>
        public AutomationTriggerType TriggerType { get; protected set; }
        
        /// <summary>
        /// Data defining the trigger (JSON or structured format)
        /// </summary>
        public string TriggerData { get; protected set; }
        
        /// <summary>
        /// Human-readable description of the trigger
        /// </summary>
        public string TriggerDescription { get; protected set; }

        protected AutomationTrigger()
        {
        }

        /// <summary>
        /// Creates a new automation trigger
        /// </summary>
        public AutomationTrigger(
            Guid id,
            Guid automationId,
            AutomationTriggerType triggerType,
            string triggerData,
            string triggerDescription) : base(id)
        {
            AutomationId = automationId;
            TriggerType = triggerType;
            TriggerData = triggerData;
            TriggerDescription = triggerDescription;
        }
    }
}
