using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events
{
    /// <summary>
    /// Event emitted when a personal automation is triggered
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.PersonalAutomationTriggered")]
    public class PersonalAutomationTriggeredEto : ProductivityIntelligenceEtoBase
    {
        /// <summary>
        /// ID of the personal automation
        /// </summary>
        public Guid AutomationId { get; set; }
        
        /// <summary>
        /// Name of the automation
        /// </summary>
        public string AutomationName { get; set; }
        
        /// <summary>
        /// Type of trigger that activated the automation
        /// </summary>
        public AutomationTriggerType TriggerType { get; set; }
        
        /// <summary>
        /// Description of what triggered the automation
        /// </summary>
        public string TriggerDescription { get; set; }
    }
}
