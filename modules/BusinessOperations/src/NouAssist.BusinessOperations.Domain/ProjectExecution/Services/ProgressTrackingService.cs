namespace NouAssist.BusinessOperations.ProjectExecution.Services
{
    /// <summary>
    /// Implementation of the progress tracking service
    /// </summary>
    public class ProgressTrackingService : DomainService, IProgressTrackingService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<ProgressTrackingService> _logger;
        
        public ProgressTrackingService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ILogger<ProgressTrackingService> logger)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }
        
        public async Task<double> GetProjectCompletionPercentageAsync(Guid projectId)
        {
            try
            {
                var tasks = await _taskRepository.GetListByProjectIdAsync(projectId);
                if (tasks.Count == 0)
                {
                    return 0;
                }
                
                var completedTasks = tasks.Count(t => t.Status == TaskStatus.Done);
                return (double)completedTasks / tasks.Count * 100;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get completion percentage for project {ProjectId}", projectId);
                throw;
            }
        }
        
        public async Task<double> GetPhaseCompletionPercentageAsync(Guid projectId, Guid phaseId)
        {
            try
            {
                var tasks = await _taskRepository.GetListByPhaseIdAsync(phaseId);
                if (tasks.Count == 0)
                {
                    return 0;
                }
                
                var completedTasks = tasks.Count(t => t.Status == TaskStatus.Done);
                return (double)completedTasks / tasks.Count * 100;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get completion percentage for phase {PhaseId}", phaseId);
                throw;
            }
        }
        
        public async Task<Dictionary<Guid, bool>> GetMilestonesStatusAsync(Guid projectId)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(projectId, false, true, false);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectId);
                }
                
                return project.Milestones.ToDictionary(
                    m => m.GetKeys()[1] as Guid? ?? Guid.Empty,
                    m => m.IsCompleted
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get milestones status for project {ProjectId}", projectId);
                throw;
            }
        }
        
        public async Task UpdateMilestoneStatusAsync(Guid projectId, Guid milestoneId, bool isCompleted)
        {
            try
            {
                var project = await _projectRepository.GetWithDetailsAsync(projectId, false, true, false);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectId);
                }
                
                var milestone = project.Milestones.FirstOrDefault(m => 
                    (m.GetKeys()[1] as Guid? ?? Guid.Empty) == milestoneId);
                    
                if (milestone == null)
                {
                    throw new BusinessException("Milestone not found");
                }
                
                if (isCompleted)
                {
                    milestone.MarkAsCompleted(Clock.Now);
                }
                else
                {
                    milestone.MarkAsIncomplete();
                }
                
                await _projectRepository.UpdateAsync(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update milestone {MilestoneId} status", milestoneId);
                throw;
            }
        }
    }
}
