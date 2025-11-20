using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks
{
    /// <summary>
    /// Interface for task application service
    /// </summary>
    public interface ITaskAppService : 
        ICrudAppService<
            TaskDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateTaskDto,
            UpdateTaskDto>
    {
        /// <summary>
        /// Gets a task with all details
        /// </summary>
        Task<TaskDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a list of tasks by project ID
        /// </summary>
        Task<List<TaskListDto>> GetListByProjectIdAsync(Guid projectId);
        
        /// <summary>
        /// Gets a list of tasks by phase ID
        /// </summary>
        Task<List<TaskListDto>> GetListByPhaseIdAsync(Guid phaseId);
        
        /// <summary>
        /// Gets a list of tasks assigned to a user
        /// </summary>
        Task<List<TaskListDto>> GetListByAssignedUserIdAsync(Guid userId);
        
        /// <summary>
        /// Gets a list of tasks by status
        /// </summary>
        Task<List<TaskListDto>> GetListByStatusAsync(Guid projectId, TaskStatus status);
        
        /// <summary>
        /// Updates the status of a task
        /// </summary>
        Task<TaskDto> UpdateStatusAsync(Guid id, UpdateTaskStatusDto input);
        
        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        Task<TaskDto> AssignToUserAsync(Guid id, Guid userId);
        
        /// <summary>
        /// Unassigns a task from a user
        /// </summary>
        Task<TaskDto> UnassignFromUserAsync(Guid id, Guid userId);
        
        /// <summary>
        /// Adds a dependency to a task
        /// </summary>
        Task<TaskDto> AddDependencyAsync(Guid id, CreateTaskDependencyDto input);
        
        /// <summary>
        /// Removes a dependency from a task
        /// </summary>
        Task<TaskDto> RemoveDependencyAsync(Guid id, Guid dependsOnTaskId, DependencyType dependencyType);
        
        /// <summary>
        /// Gets a list of overdue tasks
        /// </summary>
        Task<List<TaskListDto>> GetOverdueTasksAsync(Guid? projectId = null);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks
{
    /// <summary>
    /// Interface for task application service
    /// </summary>
    public interface ITaskAppService : IApplicationService
    {
        /// <summary>
        /// Gets a task by ID
        /// </summary>
        Task<TaskDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a task with all its details by ID
        /// </summary>
        Task<TaskWithDetailsDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of tasks
        /// </summary>
        Task<PagedResultDto<TaskDto>> GetListAsync(GetTaskListDto input);
        
        /// <summary>
        /// Gets a list of tasks assigned to the current user
        /// </summary>
        Task<List<TaskDto>> GetMyTasksAsync();
        
        /// <summary>
        /// Creates a new task
        /// </summary>
        Task<TaskDto> CreateAsync(CreateTaskDto input);
        
        /// <summary>
        /// Updates an existing task
        /// </summary>
        Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto input);
        
        /// <summary>
        /// Updates a task's status
        /// </summary>
        Task<TaskDto> UpdateStatusAsync(Guid id, UpdateTaskStatusDto input);
        
        /// <summary>
        /// Deletes a task
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        Task AssignToAsync(Guid id, AssignTaskDto input);
        
        /// <summary>
        /// Unassigns a task from a user
        /// </summary>
        Task UnassignFromAsync(Guid id, Guid userId);
        
        /// <summary>
        /// Adds a dependency to a task
        /// </summary>
        Task<TaskDependencyDto> AddDependencyAsync(Guid id, AddTaskDependencyDto input);
        
        /// <summary>
        /// Removes a dependency from a task
        /// </summary>
        Task RemoveDependencyAsync(Guid id, Guid dependsOnTaskId, NouAssist.BusinessOperations.ProjectExecution.Enums.DependencyType dependencyType);
        
        /// <summary>
        /// Gets the total logged hours for a task
        /// </summary>
        Task<double> GetTotalLoggedHoursAsync(Guid id);
        
        /// <summary>
        /// Checks if a task can be started based on its dependencies
        /// </summary>
        Task<bool> CanTaskStartAsync(Guid id);
    }
}
