using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos
{
    /// <summary>
    /// DTO for comparing two versions of a requirement
    /// </summary>
    public class CompareVersionsDto
    {
        /// <summary>
        /// ID of the requirement
        /// </summary>
        [Required]
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// First version to compare
        /// </summary>
        [Required]
        public string FirstVersion { get; set; }
        
        /// <summary>
        /// Second version to compare
        /// </summary>
        [Required]
        public string SecondVersion { get; set; }
    }
}
