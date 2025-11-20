namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for assigning a task
    /// </summary>
    public class AssignTaskDto
    {
        /// <summary>
        /// The type of assignee
        /// </summary>
        [Required]
        public AssigneeType AssigneeType { get; set; }
        
        /// <summary>
        /// The ID of the assignee
        /// </summary>
        [Required]
        public Guid AssigneeId { get; set; }
        
        /// <summary>
        /// Optional display name of the assignee
        /// </summary>
        [StringLength(256)]
        public string AssigneeDisplayName { get; set; }
        
        /// <summary>
        /// Optional due date for the task
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
