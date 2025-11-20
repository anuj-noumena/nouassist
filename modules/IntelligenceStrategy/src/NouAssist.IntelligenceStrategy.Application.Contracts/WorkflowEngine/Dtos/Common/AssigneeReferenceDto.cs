namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common
{
    /// <summary>
    /// DTO for assignee reference data
    /// </summary>
    public class AssigneeReferenceDto
    {
        /// <summary>
        /// The type of assignee
        /// </summary>
        public AssigneeType Type { get; set; }
        
        /// <summary>
        /// The ID of the assignee
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Optional display name of the assignee
        /// </summary>
        public string DisplayName { get; set; }
    }
}
