using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using NouAssist.BusinessOperations.ProjectExecution.Specifications;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks
{
    [Authorize(ProjectExecutionPermissions.Tasks.Default)]
    public class TaskAppService : 
        CrudAppService<
            Task,
            TaskDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateTaskDto,
            UpdateTaskDto>,
        ITaskAppService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly ITaskAssignmentService _taskAssignmentService;
        
        public TaskAppService(
            ITaskRepository taskRepository,
            ITimeLogRepository timeLogRepository,
            ITaskAssignmentService taskAssignmentService)
            : base(taskRepository)
        {
            _taskRepository = taskRepository;
            _timeLogRepository = timeLogRepository;
            _taskAssignmentService = taskAssignmentService;
            
            GetPolicyName = ProjectExecutionPermissions.Tasks.Default;
            GetListPolicyName = ProjectExecutionPermissions.Tasks.Default;
            CreatePolicyName = ProjectExecutionPermissions.Tasks.Create;
            UpdatePolicyName = ProjectExecutionPermissions.Tasks.Edit;
            DeletePolicyName = ProjectExecutionPermissions.Tasks.Delete;
        }
        
        public override async Task<TaskDto> GetAsync(Guid id)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id, includeAssignments: true, includeDependencies: true);
            
            var dto = ObjectMapper.Map<Task, TaskDto>(task);
            
            // Get total logged hours
            dto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(id);
            
            return dto;
        }
        
        public async Task<TaskDto> GetWithDetailsAsync(Guid id)
        {
            return await GetAsync(id);
        }
        
        public async Task<List<TaskListDto>> GetListByProjectIdAsync(Guid projectId)
        {
            var tasks = await _taskRepository.GetListByProjectIdAsync(projectId);
            
            return await MapToTaskListDtosAsync(tasks);
        }
        
        public async Task<List<TaskListDto>> GetListByPhaseIdAsync(Guid phaseId)
        {
            var tasks = await _taskRepository.GetListByPhaseIdAsync(phaseId);
            
            return await MapToTaskListDtosAsync(tasks);
        }
        
        public async Task<List<TaskListDto>> GetListByAssignedUserIdAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetListByAssignedUserIdAsync(userId);
            
            return await MapToTaskListDtosAsync(tasks);
        }
        
        public async Task<List<TaskListDto>> GetListByStatusAsync(Guid projectId, TaskStatus status)
        {
            var tasks = await _taskRepository.GetListByStatusAsync(projectId, status);
            
            return await MapToTaskListDtosAsync(tasks);
        }
        
        public async Task<List<TaskListDto>> GetOverdueTasksAsync(Guid? projectId = null)
        {
            var overdueSpecification = new OverdueTaskSpecification(Clock.Now);
            
            var query = await Repository.GetQueryableAsync();
            
            if (projectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }
            
            var tasks = await AsyncExecuter.ToListAsync(
                query.Where(overdueSpecification.ToExpression()));
                
            return await MapToTaskListDtosAsync(tasks);
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageStatus)]
        public async Task<TaskDto> UpdateStatusAsync(Guid id, UpdateTaskStatusDto input)
        {
            var task = await _taskRepository.GetAsync(id);
            
            task.UpdateStatus(input.Status);
            
            await _taskRepository.UpdateAsync(task);
            
            return await GetAsync(id);
        }
        
        public override async Task<TaskDto> CreateAsync(CreateTaskDto input)
        {
            var task = Task.Create(
                GuidGenerator.Create(),
                input.ProjectId,
                input.Name,
                input.Description,
                input.EstimatedHours,
                input.Priority,
                input.StartDate,
                input.DueDate,
                input.PhaseId,
                CurrentTenant.Id);
                
            await _taskRepository.InsertAsync(task);
            
            return await GetAsync(task.Id);
        }
        
        public override async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto input)
        {
            var task = await _taskRepository.GetAsync(id);
            
            task.UpdateDetails(
                input.Name,
                input.Description,
                input.EstimatedHours,
                input.Priority);
                
            task.UpdateDates(input.StartDate, input.DueDate);
            
            task.SetPhase(input.PhaseId);
            
            await _taskRepository.UpdateAsync(task);
            
            return await GetAsync(id);
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageAssignments)]
        public async Task<TaskDto> AssignToUserAsync(Guid id, Guid userId)
        {
            await _taskAssignmentService.AssignTaskAsync(id, userId);
            
            return await GetAsync(id);
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageAssignments)]
        public async Task<TaskDto> UnassignFromUserAsync(Guid id, Guid userId)
        {
            await _taskAssignmentService.UnassignTaskAsync(id, userId);
            
            return await GetAsync(id);
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageDependencies)]
        public async Task<TaskDto> AddDependencyAsync(Guid id, CreateTaskDependencyDto input)
        {
            // Check for circular dependency
            if (await _taskRepository.WouldCreateCircularDependencyAsync(id, input.DependsOnTaskId))
            {
                throw new BusinessException("ProjectExecution:ErrorMessage:CircularDependency");
            }
            
            var task = await _taskRepository.GetWithDetailsAsync(id, false, true);
            
            task.AddDependency(input.DependsOnTaskId, input.DependencyType);
            
            await _taskRepository.UpdateAsync(task);
            
            return await GetAsync(id);
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageDependencies)]
        public async Task<TaskDto> RemoveDependencyAsync(Guid id, Guid dependsOnTaskId, DependencyType dependencyType)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id, false, true);
            
            task.RemoveDependency(dependsOnTaskId, dependencyType);
            
            await _taskRepository.UpdateAsync(task);
            
            return await GetAsync(id);
        }
        
        private async Task<List<TaskListDto>> MapToTaskListDtosAsync(List<Task> tasks)
        {
            var dtos = new List<TaskListDto>();
            
            foreach (var task in tasks)
            {
                var dto = ObjectMapper.Map<Task, TaskListDto>(task);
                
                // Get total logged hours
                dto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(task.Id);
                
                // Get assignee count
                var taskWithAssignments = await _taskRepository.GetWithDetailsAsync(task.Id, includeAssignments: true, includeDependencies: false);
                dto.AssigneeCount = taskWithAssignments.Assignments.Count(a => a.IsActive);
                
                // Check if overdue
                dto.IsOverdue = task.Status != TaskStatus.Done && 
                                task.DueDate.HasValue && 
                                task.DueDate.Value < Clock.Now;
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using NouAssist.BusinessOperations.ProjectExecution.Exceptions;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks
{
    [Authorize(ProjectExecutionPermissions.Tasks.Default)]
    public class TaskAppService : ApplicationService, ITaskAppService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly ITaskAssignmentService _taskAssignmentService;
        private readonly IProjectScheduler _projectScheduler;
        private readonly IIdentityUserLookupService _userLookupService;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<TaskAppService> _logger;
        
        public TaskAppService(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            ITimeLogRepository timeLogRepository,
            ITaskAssignmentService taskAssignmentService,
            IProjectScheduler projectScheduler,
            IIdentityUserLookupService userLookupService,
            ICurrentUser currentUser,
            ILogger<TaskAppService> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _timeLogRepository = timeLogRepository;
            _taskAssignmentService = taskAssignmentService;
            _projectScheduler = projectScheduler;
            _userLookupService = userLookupService;
            _currentUser = currentUser;
            _logger = logger;
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.View)]
        public virtual async Task<TaskDto> GetAsync(Guid id)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(id, includeAssignments: true);
                
                var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
                
                taskDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(id);
                
                // Get current assignees
                taskDto.AssigneeIds = task.Assignments
                    .Where(a => a.IsActive)
                    .Select(a => a.UserId)
                    .ToList();
                
                // Load user names for assignees
                foreach (var userId in taskDto.AssigneeIds)
                {
                    try
                    {
                        var user = await _userLookupService.FindByIdAsync(userId);
                        if (user != null)
                        {
                            taskDto.AssigneeNames.Add(user.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find user {UserId} for task assignee", userId);
                        taskDto.AssigneeNames.Add("Unknown User");
                    }
                }
                
                return taskDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.View)]
        public virtual async Task<TaskWithDetailsDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(id, includeAssignments: true, includeDependencies: true);
                
                var taskDto = ObjectMapper.Map<Task, TaskWithDetailsDto>(task);
                
                taskDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(id);
                
                // Get current assignees
                taskDto.AssigneeIds = task.Assignments
                    .Where(a => a.IsActive)
                    .Select(a => a.UserId)
                    .ToList();
                
                // Load user names for assignees and assignments
                var userIds = task.Assignments.Select(a => a.UserId).Distinct().ToList();
                var userDictionary = new Dictionary<Guid, string>();
                
                foreach (var userId in userIds)
                {
                    try
                    {
                        var user = await _userLookupService.FindByIdAsync(userId);
                        if (user != null)
                        {
                            userDictionary[userId] = user.Name;
                        }
                        else
                        {
                            userDictionary[userId] = "Unknown User";
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find user {UserId}", userId);
                        userDictionary[userId] = "Unknown User";
                    }
                }
                
                // Apply user names to assignments
                foreach (var assignment in taskDto.Assignments)
                {
                    if (userDictionary.ContainsKey(assignment.UserId))
                    {
                        assignment.UserName = userDictionary[assignment.UserId];
                    }
                    else
                    {
                        assignment.UserName = "Unknown User";
                    }
                }
                
                // Apply user names to assignee names list
                taskDto.AssigneeNames = taskDto.AssigneeIds
                    .Select(id => userDictionary.ContainsKey(id) ? userDictionary[id] : "Unknown User")
                    .ToList();
                
                // Load task names for dependencies
                foreach (var dependency in taskDto.Dependencies)
                {
                    try
                    {
                        var dependsOnTask = await _taskRepository.GetAsync(dependency.DependsOnTaskId);
                        dependency.DependsOnTaskName = dependsOnTask.Details.Name;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find task {TaskId} for dependency", dependency.DependsOnTaskId);
                        dependency.DependsOnTaskName = "Unknown Task";
                    }
                }
                
                return taskDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.View)]
        public virtual async Task<PagedResultDto<TaskDto>> GetListAsync(GetTaskListDto input)
        {
            try
            {
                var query = await _taskRepository.GetQueryableAsync();
                
                // Apply filters
                query = query.WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                    t => t.Details.Name.Contains(input.Filter) || 
                         t.Details.Description.Contains(input.Filter)
                );
                
                query = query.WhereIf(
                    input.ProjectId.HasValue,
                    t => t.ProjectId == input.ProjectId
                );
                
                query = query.WhereIf(
                    input.PhaseId.HasValue,
                    t => t.PhaseId == input.PhaseId
                );
                
                query = query.WhereIf(
                    input.Status.HasValue,
                    t => t.Status == input.Status
                );
                
                query = query.WhereIf(
                    input.Priority.HasValue,
                    t => t.Details.Priority == input.Priority
                );
                
                query = query.WhereIf(
                    input.DueDateFrom.HasValue,
                    t => t.DueDate.HasValue && t.DueDate >= input.DueDateFrom.Value
                );
                
                query = query.WhereIf(
                    input.DueDateTo.HasValue,
                    t => t.DueDate.HasValue && t.DueDate <= input.DueDateTo.Value
                );
                
                if (input.AssignedToUserId.HasValue)
                {
                    var userId = input.AssignedToUserId.Value;
                    query = query.Where(t => t.Assignments.Any(a => a.UserId == userId && a.IsActive));
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(!string.IsNullOrWhiteSpace(input.Sorting) 
                    ? input.Sorting 
                    : "DueDate asc, Details.Priority desc");
                
                // Apply paging
                query = query.PageBy(input);
                
                // Execute query
                var tasks = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs and load additional data
                var taskDtos = new List<TaskDto>();
                
                foreach (var task in tasks)
                {
                    var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
                    
                    // Fetch and include assignments
                    var taskWithAssignments = await _taskRepository.GetWithDetailsAsync(task.Id, includeAssignments: true);
                    
                    // Get current assignees
                    taskDto.AssigneeIds = taskWithAssignments.Assignments
                        .Where(a => a.IsActive)
                        .Select(a => a.UserId)
                        .ToList();
                    
                    // Load user names for assignees
                    foreach (var userId in taskDto.AssigneeIds)
                    {
                        try
                        {
                            var user = await _userLookupService.FindByIdAsync(userId);
                            if (user != null)
                            {
                                taskDto.AssigneeNames.Add(user.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Could not find user {UserId} for task assignee", userId);
                            taskDto.AssigneeNames.Add("Unknown User");
                        }
                    }
                    
                    taskDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(task.Id);
                    
                    taskDtos.Add(taskDto);
                }
                
                return new PagedResultDto<TaskDto>(totalCount, taskDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.View)]
        public virtual async Task<List<TaskDto>> GetMyTasksAsync()
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new List<TaskDto>();
                }
                
                var tasks = await _taskRepository.GetListByAssignedUserIdAsync(_currentUser.Id.Value);
                
                var taskDtos = new List<TaskDto>();
                
                foreach (var task in tasks)
                {
                    var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
                    
                    // Fetch and include assignments
                    var taskWithAssignments = await _taskRepository.GetWithDetailsAsync(task.Id, includeAssignments: true);
                    
                    // Get current assignees
                    taskDto.AssigneeIds = taskWithAssignments.Assignments
                        .Where(a => a.IsActive)
                        .Select(a => a.UserId)
                        .ToList();
                    
                    // Load user names for assignees
                    foreach (var userId in taskDto.AssigneeIds)
                    {
                        try
                        {
                            var user = await _userLookupService.FindByIdAsync(userId);
                            if (user != null)
                            {
                                taskDto.AssigneeNames.Add(user.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Could not find user {UserId} for task assignee", userId);
                            taskDto.AssigneeNames.Add("Unknown User");
                        }
                    }
                    
                    taskDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(task.Id);
                    
                    taskDtos.Add(taskDto);
                }
                
                return taskDtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.Create)]
        public virtual async Task<TaskDto> CreateAsync(CreateTaskDto input)
        {
            try
            {
                // Validate that project exists
                var project = await _projectRepository.GetAsync(input.ProjectId);
                
                // Create the task
                var task = Task.Create(
                    GuidGenerator.Create(),
                    input.ProjectId,
                    input.Name,
                    input.Description,
                    input.EstimatedHours,
                    input.Priority,
                    input.StartDate,
                    input.DueDate,
                    input.PhaseId,
                    CurrentTenant.Id
                );
                
                await _taskRepository.InsertAsync(task);
                
                // Assign to initial assignees if specified
                if (input.AssigneeIds != null && input.AssigneeIds.Any())
                {
                    foreach (var userId in input.AssigneeIds)
                    {
                        task.AssignTo(userId);
                    }
                    
                    await _taskRepository.UpdateAsync(task);
                }
                
                // Map to DTO
                var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
                taskDto.TotalLoggedHours = 0;
                taskDto.AssigneeIds = input.AssigneeIds ?? new List<Guid>();
                
                // Load user names for assignees
                foreach (var userId in taskDto.AssigneeIds)
                {
                    try
                    {
                        var user = await _userLookupService.FindByIdAsync(userId);
                        if (user != null)
                        {
                            taskDto.AssigneeNames.Add(user.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find user {UserId} for task assignee", userId);
                        taskDto.AssigneeNames.Add("Unknown User");
                    }
                }
                
                return taskDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.Update)]
        public virtual async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto input)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(id);
                
                task.UpdateDetails(
                    input.Name,
                    input.Description,
                    input.EstimatedHours,
                    input.Priority
                );
                
                task.UpdateDates(input.StartDate, input.DueDate);
                
                task.SetPhase(input.PhaseId);
                
                await _taskRepository.UpdateAsync(task);
                
                // Map to DTO
                var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
                
                taskDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(id);
                
                // Get current assignees
                var taskWithAssignments = await _taskRepository.GetWithDetailsAsync(id, includeAssignments: true);
                
                taskDto.AssigneeIds = taskWithAssignments.Assignments
                    .Where(a => a.IsActive)
                    .Select(a => a.UserId)
                    .ToList();
                
                // Load user names for assignees
                foreach (var userId in taskDto.AssigneeIds)
                {
                    try
                    {
                        var user = await _userLookupService.FindByIdAsync(userId);
                        if (user != null)
                        {
                            taskDto.AssigneeNames.Add(user.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find user {UserId} for task assignee", userId);
                        taskDto.AssigneeNames.Add("Unknown User");
                    }
                }
                
                return taskDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.Update)]
        public virtual async Task<TaskDto> UpdateStatusAsync(Guid id, UpdateTaskStatusDto input)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(id);
                
                task.UpdateStatus(input.Status);
                
                await _taskRepository.UpdateAsync(task);
                
                // Map to DTO
                var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
                
                taskDto.TotalLoggedHours = await _timeLogRepository.GetTotalHoursByTaskIdAsync(id);
                
                // Get current assignees
                var taskWithAssignments = await _taskRepository.GetWithDetailsAsync(id, includeAssignments: true);
                
                taskDto.AssigneeIds = taskWithAssignments.Assignments
                    .Where(a => a.IsActive)
                    .Select(a => a.UserId)
                    .ToList();
                
                // Load user names for assignees
                foreach (var userId in taskDto.AssigneeIds)
                {
                    try
                    {
                        var user = await _userLookupService.FindByIdAsync(userId);
                        if (user != null)
                        {
                            taskDto.AssigneeNames.Add(user.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find user {UserId} for task assignee", userId);
                        taskDto.AssigneeNames.Add("Unknown User");
                    }
                }
                
                return taskDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _taskRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.Assign)]
        public virtual async Task AssignToAsync(Guid id, AssignTaskDto input)
        {
            try
            {
                await _taskAssignmentService.AssignTaskAsync(id, input.UserId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.Assign)]
        public virtual async Task UnassignFromAsync(Guid id, Guid userId)
        {
            try
            {
                await _taskAssignmentService.UnassignTaskAsync(id, userId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageDependencies)]
        public virtual async Task<TaskDependencyDto> AddDependencyAsync(Guid id, AddTaskDependencyDto input)
        {
            try
            {
                // Check for circular dependency
                var wouldCreateCircular = await _taskRepository.WouldCreateCircularDependencyAsync(
                    id,
                    input.DependsOnTaskId
                );
                
                if (wouldCreateCircular)
                {
                    throw new UserFriendlyException("Adding this dependency would create a circular reference");
                }
                
                var task = await _taskRepository.GetWithDetailsAsync(id, includeDependencies: true);
                
                var dependency = task.AddDependency(input.DependsOnTaskId, input.DependencyType);
                
                await _taskRepository.UpdateAsync(task);
                
                // Map to DTO
                var dependencyDto = ObjectMapper.Map<TaskDependency, TaskDependencyDto>(dependency);
                
                // Load task name
                try
                {
                    var dependsOnTask = await _taskRepository.GetAsync(input.DependsOnTaskId);
                    dependencyDto.DependsOnTaskName = dependsOnTask.Details.Name;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find task {TaskId} for dependency", input.DependsOnTaskId);
                    dependencyDto.DependsOnTaskName = "Unknown Task";
                }
                
                return dependencyDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.ManageDependencies)]
        public virtual async Task RemoveDependencyAsync(Guid id, Guid dependsOnTaskId, NouAssist.BusinessOperations.ProjectExecution.Enums.DependencyType dependencyType)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(id, includeDependencies: true);
                
                task.RemoveDependency(dependsOnTaskId, dependencyType);
                
                await _taskRepository.UpdateAsync(task);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.View)]
        public virtual async Task<double> GetTotalLoggedHoursAsync(Guid id)
        {
            try
            {
                return await _timeLogRepository.GetTotalHoursByTaskIdAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.Tasks.View)]
        public virtual async Task<bool> CanTaskStartAsync(Guid id)
        {
            try
            {
                return await _projectScheduler.CanTaskStartAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "An error occurred in TaskAppService: {Message}", exception.Message);
            
            if (exception is EntityNotFoundException entityNotFoundException)
            {
                throw new UserFriendlyException($"Task with ID {entityNotFoundException.Id} not found");
            }
            
            if (exception is ProjectExecutionDomainException domainException)
            {
                throw new UserFriendlyException(domainException.Message, domainException.Code, domainException.Data);
            }
        }
    }
}
