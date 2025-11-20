namespace NouAssist.RequirementsManagement.VersioningBaselining.Enums
{
    /// <summary>
    /// Represents the status of a baseline in its lifecycle
    /// </summary>
    public enum BaselineStatus
    {
        /// <summary>
        /// Baseline is in draft state and can be modified
        /// </summary>
        Draft = 0,
        
        /// <summary>
        /// Baseline is active and should not be modified
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Baseline is archived and can no longer be used for active development
        /// </summary>
        Archived = 2
    }
}
