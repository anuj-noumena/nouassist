namespace NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces
{
    /// <summary>
    /// Service for managing task assignments
    /// </summary>
    public interface ITaskAssignmentService
    {
        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        Task AssignTaskAsync(Guid taskId, Guid userId);
        
        /// <summary>
        /// Unassigns a task from a user
        /// </summary>
        Task UnassignTaskAsync(Guid taskId, Guid userId);
        
        /// <summary>
        /// Gets a list of tasks assigned to a user
        /// </summary>
        Task<List<Task>> GetUserTasksAsync(Guid userId);
        
        /// <summary>
        /// Gets a list of users assigned to a task
        /// </summary>
        Task<List<Guid>> GetTaskAssigneesAsync(Guid taskId);
    }
}
