using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for personal automation
    /// </summary>
    public class PersonalAutomationDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Name of the automation
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the automation
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Whether the automation is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// Triggers for the automation
        /// </summary>
        public List<AutomationTriggerDto> Triggers { get; set; }
        
        /// <summary>
        /// Actions for the automation
        /// </summary>
        public List<AutomationActionDto> Actions { get; set; }
    }
}
