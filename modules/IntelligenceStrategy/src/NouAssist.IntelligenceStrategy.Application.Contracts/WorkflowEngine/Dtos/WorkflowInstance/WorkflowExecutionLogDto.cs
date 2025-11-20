namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for workflow execution log data
    /// </summary>
    public class WorkflowExecutionLogDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the workflow instance this log entry belongs to
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }
        
        /// <summary>
        /// The ID of the activity instance, if applicable
        /// </summary>
        public Guid? ActivityInstanceId { get; set; }
        
        /// <summary>
        /// The timestamp of the log entry
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The type of the log entry
        /// </summary>
        public string LogType { get; set; }
        
        /// <summary>
        /// The message of the log entry
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Additional data for the log entry
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
    }
}
