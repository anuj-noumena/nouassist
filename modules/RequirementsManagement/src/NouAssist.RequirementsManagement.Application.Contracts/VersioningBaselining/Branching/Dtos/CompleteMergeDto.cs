using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO for completing a merge operation
    /// </summary>
    public class CompleteMergeDto
    {
        /// <summary>
        /// Number of requirements successfully merged
        /// </summary>
        [Required]
        public int MergedRequirementCount { get; set; }
        
        /// <summary>
        /// Number of conflicts resolved during the merge
        /// </summary>
        [Required]
        public int ResolvedConflictsCount { get; set; }
    }
}
