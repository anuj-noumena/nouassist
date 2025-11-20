namespace NouAssist.BusinessOperations.ProjectExecution.Enums
{
    /// <summary>
    /// Represents the status of a project
    /// </summary>
    public enum ProjectStatus
    {
        /// <summary>
        /// Project is in planning phase
        /// </summary>
        Planning = 0,
        
        /// <summary>
        /// Project is active and ongoing
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Project is temporarily on hold
        /// </summary>
        OnHold = 2,
        
        /// <summary>
        /// Project is completed
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// Project is cancelled
        /// </summary>
        Cancelled = 4
    }
}
