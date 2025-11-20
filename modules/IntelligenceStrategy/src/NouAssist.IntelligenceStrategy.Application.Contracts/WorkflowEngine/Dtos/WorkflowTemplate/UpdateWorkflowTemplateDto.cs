namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for updating a workflow template
    /// </summary>
    public class UpdateWorkflowTemplateDto
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
    }
}
