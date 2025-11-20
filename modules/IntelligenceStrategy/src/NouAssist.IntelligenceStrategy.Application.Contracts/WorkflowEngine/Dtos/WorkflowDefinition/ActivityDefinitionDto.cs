namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for activity definition data
    /// </summary>
    public class ActivityDefinitionDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the workflow definition this activity belongs to
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// The name of the activity
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The type of the activity
        /// </summary>
        public ActivityType ActivityType { get; set; }
        
        /// <summary>
        /// The description of the activity
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Additional properties for the activity
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }
    }
}
