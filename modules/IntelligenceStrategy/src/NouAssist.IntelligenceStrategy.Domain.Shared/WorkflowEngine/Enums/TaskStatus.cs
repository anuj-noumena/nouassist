namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Enums
{
    /// <summary>
    /// Represents the status of a task assignment
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The task has been created but not yet assigned
        /// </summary>
        Created = 0,
        
        /// <summary>
        /// The task has been assigned but not started
        /// </summary>
        Assigned = 1,
        
        /// <summary>
        /// The task is in progress
        /// </summary>
        InProgress = 2,
        
        /// <summary>
        /// The task has been completed
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// The task has failed
        /// </summary>
        Failed = 4,
        
        /// <summary>
        /// The task has been cancelled
        /// </summary>
        Cancelled = 5,
        
        /// <summary>
        /// The task is waiting for a prerequisite
        /// </summary>
        Waiting = 6
    }
}
