namespace NouAssist.BusinessOperations.ProjectExecution.Entities
{
    /// <summary>
    /// Represents a milestone within a project
    /// </summary>
    public class ProjectMilestone : Entity
    {
        /// <summary>
        /// ID of the project this milestone belongs to
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// Name of the milestone
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Target date for the milestone
        /// </summary>
        public DateTime TargetDate { get; private set; }
        
        /// <summary>
        /// Optional description of the milestone
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Completion date of the milestone, if completed
        /// </summary>
        public DateTime? CompletedDate { get; private set; }
        
        /// <summary>
        /// Indicates if the milestone is completed
        /// </summary>
        public bool IsCompleted => CompletedDate.HasValue;
        
        protected ProjectMilestone() { }
        
        internal ProjectMilestone(
            Guid projectId,
            string name,
            DateTime targetDate,
            string description = null)
        {
            ProjectId = projectId;
            SetName(name);
            TargetDate = targetDate;
            Description = description;
        }
        
        internal void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectExecutionConsts.MaxProjectMilestoneNameLength
            );
        }
        
        internal void MarkAsCompleted(DateTime completionDate)
        {
            CompletedDate = completionDate;
        }
        
        internal void MarkAsIncomplete()
        {
            CompletedDate = null;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { ProjectId, Name };
        }
    }
}
