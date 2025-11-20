using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for automation trigger
    /// </summary>
    public class AutomationTriggerDto
    {
        /// <summary>
        /// ID of the trigger
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Type of trigger
        /// </summary>
        public string TriggerType { get; set; }
        
        /// <summary>
        /// Data defining the trigger
        /// </summary>
        public string TriggerData { get; set; }
        
        /// <summary>
        /// Description of the trigger
        /// </summary>
        public string TriggerDescription { get; set; }
    }
}
