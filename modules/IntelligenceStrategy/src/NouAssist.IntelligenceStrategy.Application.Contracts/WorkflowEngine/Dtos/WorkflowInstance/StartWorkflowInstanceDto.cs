using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for starting a workflow instance
    /// </summary>
    public class StartWorkflowInstanceDto
    {
        /// <summary>
        /// The name of the workflow instance
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Initial context data for the workflow instance
        /// </summary>
        public Dictionary<string, string> ContextData { get; set; }
    }
}
