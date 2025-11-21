using System;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation is attempted on a workflow in an invalid state
    /// </summary>
    public class InvalidWorkflowStateException : WorkflowEngineDomainException
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidWorkflowStateException"/>
        /// </summary>
        /// <param name="workflowId">The ID of the workflow</param>
        /// <param name="currentState">The current state of the workflow</param>
        /// <param name="expectedState">The expected state for the operation</param>
        public InvalidWorkflowStateException(Guid workflowId, string currentState, string expectedState)
            : base("WorkflowEngine:ErrorMessage:InvalidWorkflowState")
        {
            WithData("workflowId", workflowId);
            WithData("currentState", currentState);
            WithData("expectedState", expectedState);
        }
    }
}
