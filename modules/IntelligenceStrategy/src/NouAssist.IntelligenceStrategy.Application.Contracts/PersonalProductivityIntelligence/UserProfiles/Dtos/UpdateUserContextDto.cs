using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for updating user context
    /// </summary>
    public class UpdateUserContextDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Entity type
        /// </summary>
        [Required]
        [StringLength(128)]
        public string EntityType { get; set; }
        
        /// <summary>
        /// Entity ID
        /// </summary>
        [Required]
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Properties to update
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }
    }
}
