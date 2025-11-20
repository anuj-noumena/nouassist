using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos
{
    /// <summary>
    /// DTO for getting automations
    /// </summary>
    public class GetAutomationsDto : PagedResultRequestDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Filter by enabled status
        /// </summary>
        public bool? IsEnabled { get; set; }
        
        /// <summary>
        /// Filter by trigger type
        /// </summary>
        public string TriggerType { get; set; }
    }
}
