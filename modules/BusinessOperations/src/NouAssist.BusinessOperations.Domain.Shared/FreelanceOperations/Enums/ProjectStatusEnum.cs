namespace NouAssist.BusinessOperations.FreelanceOperations.Enums
{
    /// <summary>
    /// Represents the status of a freelance project
    /// </summary>
    public enum ProjectStatus
    {
        /// <summary>
        /// Project is in draft state
        /// </summary>
        Draft = 1,
        
        /// <summary>
        /// Project has been proposed to client
        /// </summary>
        Proposed = 2,
        
        /// <summary>
        /// Project has been accepted and is active
        /// </summary>
        Active = 3,
        
        /// <summary>
        /// Project is on hold
        /// </summary>
        OnHold = 4,
        
        /// <summary>
        /// Project has been completed
        /// </summary>
        Completed = 5,
        
        /// <summary>
        /// Project has been canceled
        /// </summary>
        Canceled = 6,
        
        /// <summary>
        /// Project is archived
        /// </summary>
        Archived = 7
    }
}
