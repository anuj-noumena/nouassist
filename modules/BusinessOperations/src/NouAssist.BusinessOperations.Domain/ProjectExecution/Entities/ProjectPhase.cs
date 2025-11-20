namespace NouAssist.BusinessOperations.ProjectExecution.Entities
{
    /// <summary>
    /// Represents a phase within a project with specific timeframe
    /// </summary>
    public class ProjectPhase : Entity
    {
        /// <summary>
        /// ID of the project this phase belongs to
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// Name of the phase
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Start date of the phase
        /// </summary>
        public DateTime StartDate { get; private set; }
        
        /// <summary>
        /// End date of the phase
        /// </summary>
        public DateTime EndDate { get; private set; }
        
        /// <summary>
        /// Order of the phase within the project
        /// </summary>
        public int Order { get; private set; }
        
        protected ProjectPhase() { }
        
        internal ProjectPhase(
            Guid projectId,
            string name,
            DateTime startDate,
            DateTime endDate,
            int order)
        {
            ProjectId = projectId;
            SetName(name);
            SetDates(startDate, endDate);
            Order = order;
        }
        
        internal void UpdateDates(DateTime startDate, DateTime endDate)
        {
            SetDates(startDate, endDate);
        }
        
        internal void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectExecutionConsts.MaxProjectPhaseNameLength
            );
        }
        
        private void SetDates(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new BusinessException("ProjectExecution:Validation:DateOrderInvalid");
            }
            
            StartDate = startDate;
            EndDate = endDate;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { ProjectId, Name };
        }
    }
}
