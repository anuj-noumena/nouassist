using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos
{
    /// <summary>
    /// DTO for adding a requirement to a baseline
    /// </summary>
    public class AddRequirementToBaselineDto
    {
        /// <summary>
        /// ID of the requirement to add
        /// </summary>
        [Required]
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// Version of the requirement to add
        /// </summary>
        [Required]
        public string Version { get; set; }
    }
}
