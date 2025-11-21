using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.VersioningBaselining.Constants;
using NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos
{
    /// <summary>
    /// DTO for creating a new version of a requirement
    /// </summary>
    public class CreateVersionDto
    {
        /// <summary>
        /// ID of the requirement to version
        /// </summary>
        [Required]
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// Content snapshot of the requirement at this version
        /// </summary>
        [Required]
        public string ContentSnapshot { get; set; }
        
        /// <summary>
        /// Comment describing the changes in this version
        /// </summary>
        [Required]
        [StringLength(VersionControlConsts.MaxChangeCommentLength)]
        public string ChangeComment { get; set; }
        
        /// <summary>
        /// Type of version change (Major, Minor, Patch)
        /// </summary>
        [Required]
        public VersionChangeType ChangeType { get; set; }
    }
}
