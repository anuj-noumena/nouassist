using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for creating a workflow template
    /// </summary>
    public class CreateWorkflowTemplateDto
    {
        /// <summary>
        /// The name of the template
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the template
        /// </summary>
        [StringLength(WorkflowEngineConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// The category of the template
        /// </summary>
        [StringLength(128)]
        public string Category { get; set; }
        
        /// <summary>
        /// The BPMN XML content
        /// </summary>
        [Required]
        public string BpmnXmlContent { get; set; }
        
        /// <summary>
        /// The BPMN version
        /// </summary>
        [Required]
        [StringLength(20)]
        public string BpmnVersion { get; set; }
        
        /// <summary>
        /// Tags for the template
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
