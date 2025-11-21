using System;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using NouAssist.RequirementsManagement.VersioningBaselining.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Events
{
    /// <summary>
    /// Event triggered when a merge operation is completed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.VersioningBaselining.MergeOperationCompleted")]
    public class MergeOperationCompletedEto : VersionControlEtoBase
    {
        /// <summary>
        /// ID of the merge operation
        /// </summary>
        public Guid MergeOperationId { get; set; }
        
        /// <summary>
        /// ID of the source branch
        /// </summary>
        public Guid SourceBranchId { get; set; }
        
        /// <summary>
        /// Name of the source branch
        /// </summary>
        public string SourceBranchName { get; set; }
        
        /// <summary>
        /// ID of the target branch
        /// </summary>
        public Guid TargetBranchId { get; set; }
        
        /// <summary>
        /// Name of the target branch
        /// </summary>
        public string TargetBranchName { get; set; }
        
        /// <summary>
        /// Status of the merge operation
        /// </summary>
        public MergeStatus Status { get; set; }
        
        /// <summary>
        /// ID of the user who performed the merge
        /// </summary>
        public Guid MergedById { get; set; }
        
        /// <summary>
        /// Timestamp when the merge was completed
        /// </summary>
        public DateTime CompletionTime { get; set; }
        
        /// <summary>
        /// Number of requirements that were merged
        /// </summary>
        public int MergedRequirementCount { get; set; }
        
        /// <summary>
        /// Number of conflicts that were resolved during the merge
        /// </summary>
        public int ResolvedConflictsCount { get; set; }
    }
}
