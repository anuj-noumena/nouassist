namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for workflow instance data
    /// </summary>
    public class WorkflowInstanceDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// The ID of the workflow definition this instance is based on
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// The name of the workflow definition
        /// </summary>
        public string WorkflowDefinitionName { get; set; }
        
        /// <summary>
        /// The version number of the workflow definition
        /// </summary>
        public int VersionNumber { get; set; }
        
        /// <summary>
        /// The name of the workflow instance
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The current status of the workflow instance
        /// </summary>
        public InstanceStatus Status { get; set; }
        
        /// <summary>
        /// The timestamp when the workflow instance was started
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// The timestamp when the workflow instance was completed
        /// </summary>
        public DateTime? CompletionTime { get; set; }
        
        /// <summary>
        /// The context data for the workflow instance
        /// </summary>
        public Dictionary<string, string> ContextData { get; set; }
        
        /// <summary>
        /// Summary of current active tasks
        /// </summary>
        public int ActiveTasksCount { get; set; }
        
        /// <summary>
        /// Summary of total tasks
        /// </summary>
        public int TotalTasksCount { get; set; }
    }
}
