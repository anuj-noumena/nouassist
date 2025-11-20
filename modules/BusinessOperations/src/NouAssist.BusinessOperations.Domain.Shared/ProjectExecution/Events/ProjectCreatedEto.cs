namespace NouAssist.BusinessOperations.ProjectExecution.Events
{
    /// <summary>
    /// Event raised when a new project is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ProjectExecution.ProjectCreated")]
    public class ProjectCreatedEto : ProjectExecutionEtoBase
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Planned end date of the project
        /// </summary>
        public DateTime PlannedEndDate { get; set; }
        
        /// <summary>
        /// Status of the project
        /// </summary>
        public ProjectStatus Status { get; set; }
    }
}
