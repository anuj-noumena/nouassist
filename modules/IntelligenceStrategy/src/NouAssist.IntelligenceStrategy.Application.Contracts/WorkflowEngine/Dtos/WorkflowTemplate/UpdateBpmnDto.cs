using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for updating the BPMN diagram of a workflow template
    /// </summary>
    public class UpdateTemplateBpmnDto
    {
        /// <summary>
        /// The XML content of the BPMN diagram
        /// </summary>
        [Required]
        public string XmlContent { get; set; }
        
        /// <summary>
        /// The BPMN version
        /// </summary>
        [Required]
        [StringLength(20)]
        public string BpmnVersion { get; set; }
    }
}
