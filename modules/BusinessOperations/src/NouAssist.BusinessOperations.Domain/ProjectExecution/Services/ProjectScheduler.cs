namespace NouAssist.BusinessOperations.ProjectExecution.Services
{
    /// <summary>
    /// Implementation of the project scheduler service
    /// </summary>
    public class ProjectScheduler : DomainService, IProjectScheduler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<ProjectScheduler> _logger;
        
        public ProjectScheduler(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ILogger<ProjectScheduler> logger)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }
        
        public async Task<DateTime> RecalculateScheduleAsync(Guid projectId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectId);
                }
                
                var tasks = await _taskRepository.GetListByProjectIdAsync(projectId);
                
                // Build dependency graph
                var dependencyGraph = BuildDependencyGraph(tasks);
                
                // Topologically sort tasks based on dependencies
                var sortedTasks = TopologicalSort(dependencyGraph);
                
                // Calculate earliest start and finish times
                var endDate = CalculateSchedule(sortedTasks, project.Details.StartDate);
                
                return endDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to recalculate schedule for project {ProjectId}", projectId);
                throw;
            }
        }
        
        public async Task<List<Guid>> CalculateCriticalPathAsync(Guid projectId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectId);
                }
                
                var tasks = await _taskRepository.GetListByProjectIdAsync(projectId);
                
                // Build dependency graph
                var dependencyGraph = BuildDependencyGraph(tasks);
                
                // Implement critical path algorithm
                var criticalPath = FindCriticalPath(dependencyGraph, project.Details.StartDate);
                
                return criticalPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate critical path for project {ProjectId}", projectId);
                throw;
            }
        }
        
        public async Task<bool> CanTaskStartAsync(Guid taskId)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(taskId, false, true);
                if (task == null)
                {
                    throw new TaskNotFoundException(taskId);
                }
                
                // Check if the task has any FinishToStart or StartToStart dependencies
                foreach (var dependency in task.Dependencies)
                {
                    if (dependency.DependencyType == DependencyType.FinishToStart)
                    {
                        var dependsOnTask = await _taskRepository.GetAsync(dependency.DependsOnTaskId);
                        if (dependsOnTask.Status != TaskStatus.Done)
                        {
                            return false;
                        }
                    }
                    else if (dependency.DependencyType == DependencyType.StartToStart)
                    {
                        var dependsOnTask = await _taskRepository.GetAsync(dependency.DependsOnTaskId);
                        if (dependsOnTask.Status == TaskStatus.ToDo)
                        {
                            return false;
                        }
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if task {TaskId} can start", taskId);
                throw;
            }
        }
        
        private Dictionary<Guid, List<TaskDependencyInfo>> BuildDependencyGraph(List<Task> tasks)
        {
            var graph = new Dictionary<Guid, List<TaskDependencyInfo>>();
            
            foreach (var task in tasks)
            {
                graph[task.Id] = new List<TaskDependencyInfo>();
                
                foreach (var dependency in task.Dependencies)
                {
                    graph[task.Id].Add(new TaskDependencyInfo
                    {
                        TaskId = dependency.DependsOnTaskId,
                        DependencyType = dependency.DependencyType
                    });
                }
            }
            
            return graph;
        }
        
        private List<Task> TopologicalSort(Dictionary<Guid, List<TaskDependencyInfo>> graph)
        {
            // Implementation of topological sort algorithm
            // This would be a more complex implementation in a real system
            // For now, returning an empty list as placeholder
            return new List<Task>();
        }
        
        private DateTime CalculateSchedule(List<Task> sortedTasks, DateTime projectStartDate)
        {
            // Implementation of forward pass algorithm to calculate schedule
            // This would be a more complex implementation in a real system
            // For now, returning project start date + 30 days as placeholder
            return projectStartDate.AddDays(30);
        }
        
        private List<Guid> FindCriticalPath(Dictionary<Guid, List<TaskDependencyInfo>> graph, DateTime projectStartDate)
        {
            // Implementation of critical path algorithm
            // This would be a more complex implementation in a real system
            // For now, returning an empty list as placeholder
            return new List<Guid>();
        }
        
        private class TaskDependencyInfo
        {
            public Guid TaskId { get; set; }
            public DependencyType DependencyType { get; set; }
        }
    }
}
