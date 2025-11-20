namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common
{
    /// <summary>
    /// DTO for BPMN diagram data
    /// </summary>
    public class BpmnDiagramDto
    {
        /// <summary>
        /// The XML content of the BPMN diagram
        /// </summary>
        public string XmlContent { get; set; }
        
        /// <summary>
        /// The BPMN version used
        /// </summary>
        public string BpmnVersion { get; set; }
        
        /// <summary>
        /// The timestamp when the diagram was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
