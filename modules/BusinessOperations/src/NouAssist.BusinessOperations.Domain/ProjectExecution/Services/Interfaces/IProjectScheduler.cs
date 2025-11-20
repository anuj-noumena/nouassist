namespace NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces
{
    /// <summary>
    /// Service for scheduling project tasks and dependencies
    /// </summary>
    public interface IProjectScheduler
    {
        /// <summary>
        /// Recalculates the project schedule based on task dependencies
        /// </summary>
        Task<DateTime> RecalculateScheduleAsync(Guid projectId);
        
        /// <summary>
        /// Calculates the critical path for a project
        /// </summary>
        Task<List<Guid>> CalculateCriticalPathAsync(Guid projectId);
        
        /// <summary>
        /// Checks if a task can be started based on its dependencies
        /// </summary>
        Task<bool> CanTaskStartAsync(Guid taskId);
    }
}
