namespace NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces
{
    /// <summary>
    /// Service for managing time logs
    /// </summary>
    public interface ITimeLoggingService
    {
        /// <summary>
        /// Logs time for a task
        /// </summary>
        Task<TimeLog> LogTimeAsync(
            Guid userId,
            Guid projectId,
            double hours,
            DateTime logDate,
            Guid? taskId = null,
            string notes = null,
            Guid? resourceInstanceId = null);
            
        /// <summary>
        /// Gets time logs for a project
        /// </summary>
        Task<List<TimeLog>> GetProjectTimeLogsAsync(
            Guid projectId,
            DateTime? startDate = null,
            DateTime? endDate = null);
            
        /// <summary>
        /// Gets time logs for a user
        /// </summary>
        Task<List<TimeLog>> GetUserTimeLogsAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null);
            
        /// <summary>
        /// Gets time logs for a task
        /// </summary>
        Task<List<TimeLog>> GetTaskTimeLogsAsync(Guid taskId);
    }
}
