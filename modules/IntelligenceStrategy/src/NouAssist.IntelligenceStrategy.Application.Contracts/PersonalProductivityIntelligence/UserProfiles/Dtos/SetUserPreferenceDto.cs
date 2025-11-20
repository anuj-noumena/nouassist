using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for setting a user preference
    /// </summary>
    public class SetUserPreferenceDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Preference key
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Key { get; set; }
        
        /// <summary>
        /// Preference value
        /// </summary>
        [StringLength(PersonalProductivityIntelligenceConsts.MaxContentLength)]
        public string Value { get; set; }
    }
}
