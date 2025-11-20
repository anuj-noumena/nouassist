namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Exception thrown when a merge operation encounters conflicts
    /// </summary>
    public class MergeConflictException : VersionControlDomainException
    {
        /// <summary>
        /// ID of the merge operation
        /// </summary>
        public Guid MergeOperationId { get; }
        
        /// <summary>
        /// ID of the source branch
        /// </summary>
        public Guid SourceBranchId { get; }
        
        /// <summary>
        /// ID of the target branch
        /// </summary>
        public Guid TargetBranchId { get; }
        
        /// <summary>
        /// Number of conflicts detected
        /// </summary>
        public int ConflictCount { get; }

        /// <summary>
        /// Creates a new MergeConflictException
        /// </summary>
        public MergeConflictException(Guid mergeOperationId, Guid sourceBranchId, Guid targetBranchId, int conflictCount)
            : base("VersionControl:ErrorMessage:MergeConflict")
        {
            MergeOperationId = mergeOperationId;
            SourceBranchId = sourceBranchId;
            TargetBranchId = targetBranchId;
            ConflictCount = conflictCount;
            
            WithData("mergeOperationId", mergeOperationId);
            WithData("sourceBranchId", sourceBranchId);
            WithData("targetBranchId", targetBranchId);
            WithData("conflictCount", conflictCount);
        }
    }
}
