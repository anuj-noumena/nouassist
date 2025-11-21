using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for updating a workflow definition
    /// </summary>
    public class UpdateWorkflowDefinitionDto
    {
        /// <summary>
        /// The name of the workflow definition
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the workflow definition
        /// </summary>
        [StringLength(WorkflowEngineConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// The category of the workflow definition
        /// </summary>
        [StringLength(128)]
        public string Category { get; set; }
    }
}
