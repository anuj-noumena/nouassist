using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for automation action
    /// </summary>
    public class AutomationActionDto
    {
        /// <summary>
        /// ID of the action
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Type of action
        /// </summary>
        public string ActionType { get; set; }
        
        /// <summary>
        /// Data defining the action
        /// </summary>
        public string ActionData { get; set; }
        
        /// <summary>
        /// Order of execution
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Description of the action
        /// </summary>
        public string ActionDescription { get; set; }
    }
}
