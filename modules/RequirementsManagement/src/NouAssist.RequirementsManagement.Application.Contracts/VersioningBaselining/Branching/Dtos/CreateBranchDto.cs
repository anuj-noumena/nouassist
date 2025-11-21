using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.VersioningBaselining.Constants;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO for creating a new branch
    /// </summary>
    public class CreateBranchDto
    {
        /// <summary>
        /// Name of the branch
        /// </summary>
        [Required]
        [StringLength(VersionControlConsts.MaxBranchNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the branch
        /// </summary>
        [StringLength(2000)]
        public string Description { get; set; }
        
        /// <summary>
        /// ID of the source baseline or branch
        /// </summary>
        [Required]
        public Guid SourceId { get; set; }
        
        /// <summary>
        /// Type of the source ("Baseline" or "Branch")
        /// </summary>
        [Required]
        public string SourceType { get; set; }
    }
}
