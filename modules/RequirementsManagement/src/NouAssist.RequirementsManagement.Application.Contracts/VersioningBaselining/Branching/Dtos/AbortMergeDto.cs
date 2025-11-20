using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO for aborting a merge operation
    /// </summary>
    public class AbortMergeDto
    {
        /// <summary>
        /// Reason for aborting the merge
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Reason { get; set; }
    }
}
