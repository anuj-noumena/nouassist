namespace NouAssist.BusinessOperations.ProjectExecution.Events
{
    /// <summary>
    /// Event raised when a task status is updated
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ProjectExecution.TaskStatusUpdated")]
    public class TaskStatusUpdatedEto : ProjectExecutionEtoBase
    {
        /// <summary>
        /// ID of the task
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// ID of the project this task belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Previous status of the task
        /// </summary>
        public TaskStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// New status of the task
        /// </summary>
        public TaskStatus NewStatus { get; set; }
    }
}
