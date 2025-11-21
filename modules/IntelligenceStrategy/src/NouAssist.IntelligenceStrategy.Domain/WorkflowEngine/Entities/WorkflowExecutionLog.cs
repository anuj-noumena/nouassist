using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Entities
{
    /// <summary>
    /// Represents a log entry for workflow execution
    /// </summary>
    public class WorkflowExecutionLog : Entity<Guid>
    {
        /// <summary>
        /// The ID of the workflow instance this log entry belongs to
        /// </summary>
        public Guid WorkflowInstanceId { get; private set; }
        
        /// <summary>
        /// The ID of the activity instance, if applicable
        /// </summary>
        public Guid? ActivityInstanceId { get; private set; }
        
        /// <summary>
        /// The timestamp of the log entry
        /// </summary>
        public DateTime Timestamp { get; private set; }
        
        /// <summary>
        /// The type of the log entry
        /// </summary>
        public string LogType { get; private set; }
        
        /// <summary>
        /// The message of the log entry
        /// </summary>
        public string Message { get; private set; }
        
        /// <summary>
        /// Additional data for the log entry
        /// </summary>
        public Dictionary<string, string> Data { get; private set; }

        protected WorkflowExecutionLog() { }

        internal WorkflowExecutionLog(
            Guid id,
            Guid workflowInstanceId,
            Guid? activityInstanceId,
            string logType,
            string message,
            Dictionary<string, string> data = null)
            : base(id)
        {
            WorkflowInstanceId = workflowInstanceId;
            ActivityInstanceId = activityInstanceId;
            Timestamp = DateTime.UtcNow;
            LogType = logType;
            Message = message;
            Data = data ?? new Dictionary<string, string>();
        }
    }
}
