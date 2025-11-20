namespace NouAssist.RequirementsManagement.VersioningBaselining.Enums
{
    /// <summary>
    /// Represents the status of a merge operation
    /// </summary>
    public enum MergeStatus
    {
        /// <summary>
        /// Merge is in progress
        /// </summary>
        InProgress = 0,
        
        /// <summary>
        /// Merge completed successfully
        /// </summary>
        Completed = 1,
        
        /// <summary>
        /// Merge has conflicts that need resolution
        /// </summary>
        HasConflicts = 2,
        
        /// <summary>
        /// Merge was aborted
        /// </summary>
        Aborted = 3
    }
}
