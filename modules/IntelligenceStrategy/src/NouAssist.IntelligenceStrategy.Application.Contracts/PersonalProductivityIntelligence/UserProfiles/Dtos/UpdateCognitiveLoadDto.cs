using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for updating cognitive load
    /// </summary>
    public class UpdateCognitiveLoadDto
    {
        /// <summary>
        /// User ID to update cognitive load for
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// New cognitive load value (0-100)
        /// </summary>
        [Required]
        [Range(0, 100)]
        public double LoadValue { get; set; }
    }
}
