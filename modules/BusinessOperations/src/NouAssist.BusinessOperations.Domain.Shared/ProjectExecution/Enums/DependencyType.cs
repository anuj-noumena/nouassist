namespace NouAssist.BusinessOperations.ProjectExecution.Enums
{
    /// <summary>
    /// Represents the type of dependency between tasks
    /// </summary>
    public enum DependencyType
    {
        /// <summary>
        /// Finish-to-Start: Task B cannot start until Task A finishes
        /// </summary>
        FinishToStart = 0,
        
        /// <summary>
        /// Start-to-Start: Task B cannot start until Task A starts
        /// </summary>
        StartToStart = 1,
        
        /// <summary>
        /// Finish-to-Finish: Task B cannot finish until Task A finishes
        /// </summary>
        FinishToFinish = 2,
        
        /// <summary>
        /// Start-to-Finish: Task B cannot finish until Task A starts
        /// </summary>
        StartToFinish = 3
    }
}
