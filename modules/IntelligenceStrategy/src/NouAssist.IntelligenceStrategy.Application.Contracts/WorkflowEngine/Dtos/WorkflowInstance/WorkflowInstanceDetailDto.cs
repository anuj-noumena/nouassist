using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for detailed workflow instance data
    /// </summary>
    public class WorkflowInstanceDetailDto : WorkflowInstanceDto
    {
        /// <summary>
        /// The activity instances in this workflow instance
        /// </summary>
        public List<ActivityInstanceDto> ActivityInstances { get; set; }
        
        /// <summary>
        /// The execution logs for this workflow instance
        /// </summary>
        public List<WorkflowExecutionLogDto> ExecutionLogs { get; set; }
    }
}
