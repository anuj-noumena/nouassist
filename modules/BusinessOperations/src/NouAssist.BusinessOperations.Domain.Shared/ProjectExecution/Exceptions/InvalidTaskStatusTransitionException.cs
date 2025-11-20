namespace NouAssist.BusinessOperations.ProjectExecution.Exceptions
{
    /// <summary>
    /// Exception thrown when trying to make an invalid task status transition
    /// </summary>
    public class InvalidTaskStatusTransitionException : ProjectExecutionDomainException
    {
        /// <summary>
        /// Current status of the task
        /// </summary>
        public TaskStatus CurrentStatus { get; }
        
        /// <summary>
        /// Target status of the task
        /// </summary>
        public TaskStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new InvalidTaskStatusTransitionException
        /// </summary>
        /// <param name="currentStatus">Current status of the task</param>
        /// <param name="targetStatus">Target status of the task</param>
        public InvalidTaskStatusTransitionException(TaskStatus currentStatus, TaskStatus targetStatus)
            : base("ProjectExecution:ErrorMessage:InvalidTaskStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
