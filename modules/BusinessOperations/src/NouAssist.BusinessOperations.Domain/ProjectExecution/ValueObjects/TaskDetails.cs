namespace NouAssist.BusinessOperations.ProjectExecution.ValueObjects
{
    /// <summary>
    /// Value object containing the essential details of a task
    /// </summary>
    public class TaskDetails : ValueObject
    {
        /// <summary>
        /// Name of the task
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Estimated hours for task completion
        /// </summary>
        public double EstimatedHours { get; private set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public TaskPriority Priority { get; private set; }
        
        protected TaskDetails() { }
        
        public TaskDetails(
            string name,
            string description,
            double estimatedHours,
            TaskPriority priority)
        {
            SetName(name);
            SetDescription(description);
            SetEstimatedHours(estimatedHours);
            Priority = priority;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectExecutionConsts.MaxTaskNameLength
            );
        }
        
        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProjectExecutionConsts.MaxTaskDescriptionLength
            );
        }
        
        private void SetEstimatedHours(double estimatedHours)
        {
            if (estimatedHours <= 0)
            {
                throw new BusinessException("ProjectExecution:Validation:DurationInvalid");
            }
            
            EstimatedHours = estimatedHours;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return EstimatedHours;
            yield return Priority;
        }
    }
}
