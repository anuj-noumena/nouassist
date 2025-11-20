using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO for initiating a merge operation
    /// </summary>
    public class InitiateMergeDto
    {
        /// <summary>
        /// ID of the target branch
        /// </summary>
        [Required]
        public Guid TargetBranchId { get; set; }
    }
}
