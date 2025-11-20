using NouAssist.RequirementsManagement.VersioningBaselining.Enums;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO representing the result of a merge operation
    /// </summary>
    public class MergeResultDto
    {
        /// <summary>
        /// Status of the merge operation
        /// </summary>
        public MergeStatus Status { get; set; }
        
        /// <summary>
        /// Number of requirements successfully merged
        /// </summary>
        public int MergedRequirementCount { get; set; }
        
        /// <summary>
        /// Number of conflicts detected during the merge
        /// </summary>
        public int ConflictCount { get; set; }
        
        /// <summary>
        /// Optional message or summary of the merge result
        /// </summary>
        public string Message { get; set; }
    }
}
