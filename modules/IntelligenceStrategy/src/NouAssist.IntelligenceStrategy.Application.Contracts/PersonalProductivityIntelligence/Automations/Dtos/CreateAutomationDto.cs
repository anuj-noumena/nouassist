using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for creating an automation
    /// </summary>
    public class CreateAutomationDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Name of the automation
        /// </summary>
        [Required]
        [StringLength(PersonalProductivityIntelligenceConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the automation
        /// </summary>
        [StringLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Whether the automation is enabled
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
