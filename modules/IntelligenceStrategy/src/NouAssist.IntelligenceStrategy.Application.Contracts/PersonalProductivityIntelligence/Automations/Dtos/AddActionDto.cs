using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for adding an action
    /// </summary>
    public class AddActionDto
    {
        /// <summary>
        /// ID of the automation
        /// </summary>
        [Required]
        public Guid AutomationId { get; set; }
        
        /// <summary>
        /// Type of action
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ActionType { get; set; }
        
        /// <summary>
        /// Data defining the action
        /// </summary>
        [Required]
        public string ActionData { get; set; }
        
        /// <summary>
        /// Order of execution
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Description of the action
        /// </summary>
        [StringLength(256)]
        public string ActionDescription { get; set; }
    }
}
