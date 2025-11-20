namespace NouAssist.RequirementsManagement.VersioningBaselining.Enums
{
    /// <summary>
    /// Represents the status of a requirement branch
    /// </summary>
    public enum BranchStatus
    {
        /// <summary>
        /// Branch is active and can be modified
        /// </summary>
        Active = 0,
        
        /// <summary>
        /// Branch has been merged and is no longer active
        /// </summary>
        Merged = 1,
        
        /// <summary>
        /// Branch has been abandoned
        /// </summary>
        Abandoned = 2
    }
}
