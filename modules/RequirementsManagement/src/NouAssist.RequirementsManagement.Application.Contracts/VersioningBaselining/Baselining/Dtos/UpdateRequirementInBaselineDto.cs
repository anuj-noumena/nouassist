using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos
{
    /// <summary>
    /// DTO for updating a requirement in a baseline
    /// </summary>
    public class UpdateRequirementInBaselineDto
    {
        /// <summary>
        /// ID of the requirement to update
        /// </summary>
        [Required]
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// New version of the requirement
        /// </summary>
        [Required]
        public string NewVersion { get; set; }
    }
}
