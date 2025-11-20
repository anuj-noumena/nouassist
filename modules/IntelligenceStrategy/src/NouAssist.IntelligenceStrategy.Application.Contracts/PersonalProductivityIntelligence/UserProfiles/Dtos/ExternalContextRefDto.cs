using System;
using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for external context reference
    /// </summary>
    public class ExternalContextRefDto
    {
        /// <summary>
        /// Type of the entity
        /// </summary>
        public string EntityType { get; set; }
        
        /// <summary>
        /// ID of the entity
        /// </summary>
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Additional properties
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }
    }
}
