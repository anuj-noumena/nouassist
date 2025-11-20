namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for updating the BPMN diagram of a workflow version
    /// </summary>
    public class UpdateBpmnDto
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
