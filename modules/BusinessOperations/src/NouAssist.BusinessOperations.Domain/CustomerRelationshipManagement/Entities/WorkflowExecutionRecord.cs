namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Records the execution of a workflow for auditing and tracking
    /// </summary>
    public class WorkflowExecutionRecord : Entity
    {
        /// <summary>
        /// The workflow that was executed
        /// </summary>
        public Guid WorkflowId { get; private set; }
        
        /// <summary>
        /// Execution identifier
        /// </summary>
        public Guid ExecutionId { get; private set; }
        
        /// <summary>
        /// Entity that triggered the workflow
        /// </summary>
        public Guid TriggeredByEntityId { get; private set; }
        
        /// <summary>
        /// Type of entity that triggered the workflow
        /// </summary>
        public string TriggeredByEntityType { get; private set; }
        
        /// <summary>
        /// When the execution started
        /// </summary>
        public DateTime StartTime { get; private set; }
        
        /// <summary>
        /// When the execution completed
        /// </summary>
        public DateTime? CompletionTime { get; private set; }
        
        /// <summary>
        /// Overall status of the execution
        /// </summary>
        public string Status { get; private set; }
        
        /// <summary>
        /// Error message if execution failed
        /// </summary>
        public string ErrorMessage { get; private set; }
        
        /// <summary>
        /// Detailed execution log in JSON format
        /// </summary>
        public string ExecutionLog { get; private set; }

        protected WorkflowExecutionRecord()
        {
        }

        internal WorkflowExecutionRecord(
            Guid workflowId,
            Guid executionId,
            Guid triggeredByEntityId,
            string triggeredByEntityType,
            DateTime startTime)
        {
            WorkflowId = workflowId;
            ExecutionId = executionId;
            TriggeredByEntityId = triggeredByEntityId;
            TriggeredByEntityType = triggeredByEntityType;
            StartTime = startTime;
            Status = "Running";
        }

        internal void Complete(DateTime completionTime)
        {
            CompletionTime = completionTime;
            Status = "Completed";
        }

        internal void Fail(string errorMessage)
        {
            Status = "Failed";
            ErrorMessage = errorMessage;
            CompletionTime = Clock.Now;
        }

        internal void AppendToLog(string logEntry)
        {
            if (string.IsNullOrEmpty(ExecutionLog))
            {
                ExecutionLog = logEntry;
            }
            else
            {
                ExecutionLog += Environment.NewLine + logEntry;
            }
        }

        public override object[] GetKeys()
        {
            return new object[] { ExecutionId };
        }
    }
}
