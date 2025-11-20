using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for adding a trigger
    /// </summary>
    public class AddTriggerDto
    {
        /// <summary>
        /// ID of the automation
        /// </summary>
        [Required]
        public Guid AutomationId { get; set; }
        
        /// <summary>
        /// Type of trigger
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TriggerType { get; set; }
        
        /// <summary>
        /// Data defining the trigger
        /// </summary>
        [Required]
        public string TriggerData { get; set; }
        
        /// <summary>
        /// Description of the trigger
        /// </summary>
        [StringLength(256)]
        public string TriggerDescription { get; set; }
    }
}
