namespace NouAssist.BusinessOperations.ProjectExecution.Enums
{
    /// <summary>
    /// Represents the status of a task
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// Task is to do and not started
        /// </summary>
        ToDo = 0,
        
        /// <summary>
        /// Task is in progress
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// Task is done/completed
        /// </summary>
        Done = 2,
        
        /// <summary>
        /// Task is blocked and cannot progress
        /// </summary>
        Blocked = 3
    }
}
