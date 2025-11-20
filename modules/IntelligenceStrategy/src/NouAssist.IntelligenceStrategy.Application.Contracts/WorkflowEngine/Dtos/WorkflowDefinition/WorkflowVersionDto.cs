namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for workflow version data
    /// </summary>
    public class WorkflowVersionDto
    {
        /// <summary>
        /// The ID of the workflow definition this version belongs to
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// Version number
        /// </summary>
        public int VersionNumber { get; set; }
        
        /// <summary>
        /// Indicates if this version is published
        /// </summary>
        public bool IsPublished { get; set; }
        
        /// <summary>
        /// The timestamp when this version was created
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Optional comments about this version
        /// </summary>
        public string Comments { get; set; }
        
        /// <summary>
        /// The BPMN diagram for this version
        /// </summary>
        public BpmnDiagramDto BpmnDiagram { get; set; }
        
        /// <summary>
        /// The activities defined in this workflow version
        /// </summary>
        public List<ActivityDefinitionDto> Activities { get; set; }
        
        /// <summary>
        /// The conditions defined in this workflow version
        /// </summary>
        public List<ConditionDefinitionDto> Conditions { get; set; }
    }
}
