namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for activity instance data
    /// </summary>
    public class ActivityInstanceDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the workflow instance this activity instance belongs to
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }
        
        /// <summary>
        /// The ID of the activity definition this instance is based on
        /// </summary>
        public Guid ActivityDefinitionId { get; set; }
        
        /// <summary>
        /// The name of the activity
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The type of the activity
        /// </summary>
        public ActivityType ActivityType { get; set; }
        
        /// <summary>
        /// The status of the activity instance
        /// </summary>
        public TaskStatus Status { get; set; }
        
        /// <summary>
        /// The timestamp when the activity was started
        /// </summary>
        public DateTime? StartTime { get; set; }
        
        /// <summary>
        /// The timestamp when the activity was completed
        /// </summary>
        public DateTime? CompletionTime { get; set; }
        
        /// <summary>
        /// The due date for the activity, if any
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// The assignee of the activity, if it's a user task
        /// </summary>
        public AssigneeReferenceDto Assignee { get; set; }
        
        /// <summary>
        /// Custom properties for the activity instance
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }
    }
}
