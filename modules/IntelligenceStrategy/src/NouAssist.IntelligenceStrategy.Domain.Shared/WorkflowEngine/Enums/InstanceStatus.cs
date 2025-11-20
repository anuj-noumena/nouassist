namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Enums
{
    /// <summary>
    /// Represents the current status of a workflow instance
    /// </summary>
    public enum InstanceStatus
    {
        /// <summary>
        /// The workflow instance is currently executing
        /// </summary>
        Running = 0,
        
        /// <summary>
        /// The workflow instance has completed successfully
        /// </summary>
        Completed = 1,
        
        /// <summary>
        /// The workflow instance has failed with an error
        /// </summary>
        Failed = 2,
        
        /// <summary>
        /// The workflow instance has been manually paused
        /// </summary>
        Paused = 3,
        
        /// <summary>
        /// The workflow instance is waiting for an external trigger or condition
        /// </summary>
        Waiting = 4
    }
}
