namespace NouAssist.BusinessOperations.ProjectExecution.Events
{
    /// <summary>
    /// Event raised when a new task is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ProjectExecution.TaskCreated")]
    public class TaskCreatedEto : ProjectExecutionEtoBase
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
        /// Name of the task
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Estimated hours for the task
        /// </summary>
        public double EstimatedHours { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public TaskPriority Priority { get; set; }
    }
}
