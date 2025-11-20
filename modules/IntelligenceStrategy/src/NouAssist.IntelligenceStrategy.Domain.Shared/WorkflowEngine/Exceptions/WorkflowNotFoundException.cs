namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions
{
    /// <summary>
    /// Exception thrown when a workflow is not found
    /// </summary>
    public class WorkflowNotFoundException : WorkflowEngineDomainException
    {
        /// <summary>
        /// The ID of the workflow that was not found
        /// </summary>
        public Guid WorkflowId { get; }
        
        /// <summary>
        /// Creates a new instance of <see cref="WorkflowNotFoundException"/>
        /// </summary>
        /// <param name="workflowId">The ID of the workflow that was not found</param>
        public WorkflowNotFoundException(Guid workflowId)
            : base("WorkflowEngine:ErrorMessage:WorkflowNotFound")
        {
            WorkflowId = workflowId;
            WithData("workflowId", workflowId);
        }
    }
}
