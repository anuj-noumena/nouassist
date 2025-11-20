namespace NouAssist.BusinessOperations.ProjectExecution.Exceptions
{
    /// <summary>
    /// Exception thrown when trying to make an invalid project status transition
    /// </summary>
    public class InvalidProjectStatusTransitionException : ProjectExecutionDomainException
    {
        /// <summary>
        /// Current status of the project
        /// </summary>
        public ProjectStatus CurrentStatus { get; }
        
        /// <summary>
        /// Target status of the project
        /// </summary>
        public ProjectStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new InvalidProjectStatusTransitionException
        /// </summary>
        /// <param name="currentStatus">Current status of the project</param>
        /// <param name="targetStatus">Target status of the project</param>
        public InvalidProjectStatusTransitionException(ProjectStatus currentStatus, ProjectStatus targetStatus)
            : base("ProjectExecution:ErrorMessage:InvalidProjectStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
