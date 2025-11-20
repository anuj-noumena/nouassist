namespace NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces
{
    /// <summary>
    /// Service for tracking progress of projects and tasks
    /// </summary>
    public interface IProgressTrackingService
    {
        /// <summary>
        /// Gets the completion percentage of a project
        /// </summary>
        Task<double> GetProjectCompletionPercentageAsync(Guid projectId);
        
        /// <summary>
        /// Gets the completion percentage of a phase
        /// </summary>
        Task<double> GetPhaseCompletionPercentageAsync(Guid projectId, Guid phaseId);
        
        /// <summary>
        /// Gets the status of project milestones
        /// </summary>
        Task<Dictionary<Guid, bool>> GetMilestonesStatusAsync(Guid projectId);
        
        /// <summary>
        /// Updates milestone completion status
        /// </summary>
        Task UpdateMilestoneStatusAsync(Guid projectId, Guid milestoneId, bool isCompleted);
    }
}
