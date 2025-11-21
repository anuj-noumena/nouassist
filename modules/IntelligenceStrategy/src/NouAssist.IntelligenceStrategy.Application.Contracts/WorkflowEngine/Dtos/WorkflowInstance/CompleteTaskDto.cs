using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for completing a task
    /// </summary>
    public class CompleteTaskDto
    {
        /// <summary>
        /// Output data from the task
        /// </summary>
        public Dictionary<string, string> OutputData { get; set; }
    }
}
