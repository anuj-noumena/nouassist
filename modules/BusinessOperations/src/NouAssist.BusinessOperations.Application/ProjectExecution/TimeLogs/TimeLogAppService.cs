using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs
{
    [Authorize(ProjectExecutionPermissions.TimeLogs.Default)]
    public class TimeLogAppService : 
        CrudAppService<
            TimeLog,
            TimeLogDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateTimeLogDto,
            UpdateTimeLogDto>,
        ITimeLogAppService
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeLoggingService _timeLoggingService;
        
        public TimeLogAppService(
            ITimeLogRepository timeLogRepository,
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ITimeLoggingService timeLoggingService)
            : base(timeLogRepository)
        {
            _timeLogRepository = timeLogRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _timeLoggingService = timeLoggingService;
            
            GetPolicyName = ProjectExecutionPermissions.TimeLogs.Default;
            GetListPolicyName = ProjectExecutionPermissions.TimeLogs.Default;
            CreatePolicyName = ProjectExecutionPermissions.TimeLogs.Create;
            UpdatePolicyName = ProjectExecutionPermissions.TimeLogs.Edit;
            DeletePolicyName = ProjectExecutionPermissions.TimeLogs.Delete;
        }
        
        public override async Task<TimeLogDto> GetAsync(Guid id)
        {
            var timeLog = await _timeLogRepository.GetAsync(id);
            
            var dto = ObjectMapper.Map<TimeLog, TimeLogDto>(timeLog);
            
            // Get related entities for display names
            if (timeLog.ProjectId != Guid.Empty)
            {
                var project = await _projectRepository.GetAsync(timeLog.ProjectId);
                dto.ProjectName = project.Details.Name;
            }
            
            if (timeLog.TaskId.HasValue && timeLog.TaskId.Value != Guid.Empty)
            {
                var task = await _taskRepository.GetAsync(timeLog.TaskId.Value);
                dto.TaskName = task.Details.Name;
            }
            
            // UserName would be set by an external user service in a real implementation
            dto.UserName = $"User {timeLog.UserId}";
            
            return dto;
        }
        
        public async Task<List<TimeLogDto>> GetListByProjectIdAsync(Guid projectId, DateTime? startDate = null, DateTime? endDate = null)
        {
            CheckViewPermission(projectId);
            
            var timeLogs = await _timeLogRepository.GetListByProjectIdAsync(projectId, startDate, endDate);
            
            return await MapToTimeLogDtosAsync(timeLogs);
        }
        
        public async Task<List<TimeLogDto>> GetListByTaskIdAsync(Guid taskId)
        {
            // First get the task to check project permissions
            var task = await _taskRepository.GetAsync(taskId);
            CheckViewPermission(task.ProjectId);
            
            var timeLogs = await _timeLogRepository.GetListByTaskIdAsync(taskId);
            
            return await MapToTimeLogDtosAsync(timeLogs);
        }
        
        public async Task<List<TimeLogDto>> GetListByUserIdAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            // For viewing other users' time logs, we need ViewOthers permission
            if (CurrentUser.Id != userId)
            {
                await AuthorizationService.CheckAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers);
            }
            
            var timeLogs = await _timeLogRepository.GetListByUserIdAsync(userId, startDate, endDate);
            
            return await MapToTimeLogDtosAsync(timeLogs);
        }
        
        public override async Task<TimeLogDto> CreateAsync(CreateTimeLogDto input)
        {
            // Validate that project and task exist and task belongs to project
            if (input.TaskId.HasValue)
            {
                var task = await _taskRepository.GetAsync(input.TaskId.Value);
                if (task.ProjectId != input.ProjectId)
                {
                    throw new BusinessException("ProjectExecution:ErrorMessage:TaskNotPartOfProject");
                }
            }
            
            var timeLog = await _timeLoggingService.LogTimeAsync(
                CurrentUser.Id.Value,
                input.ProjectId,
                input.Hours,
                input.LogDate,
                input.TaskId,
                input.Notes,
                input.ResourceInstanceId);
                
            return await GetAsync(timeLog.Id);
        }
        
        public override async Task<TimeLogDto> UpdateAsync(Guid id, UpdateTimeLogDto input)
        {
            var timeLog = await _timeLogRepository.GetAsync(id);
            
            // Ensure the user is updating their own time log
            if (timeLog.UserId != CurrentUser.Id)
            {
                await AuthorizationService.CheckAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers);
            }
            
            timeLog.UpdateDetails(
                input.Hours,
                input.LogDate,
                input.Notes,
                input.ResourceInstanceId);
                
            await _timeLogRepository.UpdateAsync(timeLog);
            
            return await GetAsync(id);
        }
        
        public async Task<TimeLogSummaryDto> GetProjectSummaryAsync(Guid projectId, DateTime? startDate = null, DateTime? endDate = null)
        {
            CheckViewPermission(projectId);
            
            var actualStartDate = startDate ?? Clock.Now.AddDays(-30);
            var actualEndDate = endDate ?? Clock.Now;
            
            var timeLogs = await _timeLogRepository.GetListByProjectIdAsync(projectId, actualStartDate, actualEndDate);
            
            return await CreateSummaryAsync(timeLogs, actualStartDate, actualEndDate);
        }
        
        public async Task<TimeLogSummaryDto> GetUserSummaryAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            // For viewing other users' summary, we need ViewOthers permission
            if (CurrentUser.Id != userId)
            {
                await AuthorizationService.CheckAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers);
            }
            
            var actualStartDate = startDate ?? Clock.Now.AddDays(-30);
            var actualEndDate = endDate ?? Clock.Now;
            
            var timeLogs = await _timeLogRepository.GetListByUserIdAsync(userId, actualStartDate, actualEndDate);
            
            return await CreateSummaryAsync(timeLogs, actualStartDate, actualEndDate);
        }
        
        private async Task<List<TimeLogDto>> MapToTimeLogDtosAsync(List<TimeLog> timeLogs)
        {
            var dtos = new List<TimeLogDto>();
            
            // Dictionary to cache project and task names
            var projectNames = new Dictionary<Guid, string>();
            var taskNames = new Dictionary<Guid, string>();
            
            foreach (var timeLog in timeLogs)
            {
                var dto = ObjectMapper.Map<TimeLog, TimeLogDto>(timeLog);
                
                // Get project name (from cache or repository)
                if (!projectNames.TryGetValue(timeLog.ProjectId, out var projectName))
                {
                    var project = await _projectRepository.GetAsync(timeLog.ProjectId);
                    projectName = project.Details.Name;
                    projectNames[timeLog.ProjectId] = projectName;
                }
                
                dto.ProjectName = projectName;
                
                // Get task name if applicable
                if (timeLog.TaskId.HasValue && timeLog.TaskId.Value != Guid.Empty)
                {
                    if (!taskNames.TryGetValue(timeLog.TaskId.Value, out var taskName))
                    {
                        var task = await _taskRepository.GetAsync(timeLog.TaskId.Value);
                        taskName = task.Details.Name;
                        taskNames[timeLog.TaskId.Value] = taskName;
                    }
                    
                    dto.TaskName = taskName;
                }
                
                // UserName would be set by an external user service in a real implementation
                dto.UserName = $"User {timeLog.UserId}";
                
                dtos.Add(dto);
            }
            
            return dtos;
        }
        
        private async Task<TimeLogSummaryDto> CreateSummaryAsync(List<TimeLog> timeLogs, DateTime startDate, DateTime endDate)
        {
            var totalHours = timeLogs.Sum(tl => tl.Details.Hours);
            
            // Project breakdown
            var projectBreakdown = timeLogs
                .GroupBy(tl => tl.ProjectId)
                .Select(g => new TimeLogProjectSummaryDto
                {
                    ProjectId = g.Key,
                    Hours = g.Sum(tl => tl.Details.Hours),
                    Percentage = totalHours > 0 ? g.Sum(tl => tl.Details.Hours) / totalHours * 100 : 0
                })
                .ToList();
                
            // Get project names
            foreach (var item in projectBreakdown)
            {
                var project = await _projectRepository.GetAsync(item.ProjectId);
                item.ProjectName = project.Details.Name;
            }
            
            // User breakdown
            var userBreakdown = timeLogs
                .GroupBy(tl => tl.UserId)
                .Select(g => new TimeLogUserSummaryDto
                {
                    UserId = g.Key,
                    Hours = g.Sum(tl => tl.Details.Hours),
                    Percentage = totalHours > 0 ? g.Sum(tl => tl.Details.Hours) / totalHours * 100 : 0,
                    UserName = $"User {g.Key}" // Would use a user service in a real implementation
                })
                .ToList();
                
            // Daily breakdown
            var dailyBreakdown = new List<TimeLogDailySummaryDto>();
            
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var dailyHours = timeLogs
                    .Where(tl => tl.Details.LogDate.Date == date)
                    .Sum(tl => tl.Details.Hours);
                    
                dailyBreakdown.Add(new TimeLogDailySummaryDto
                {
                    Date = date,
                    Hours = dailyHours
                });
            }
            
            return new TimeLogSummaryDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalHours = totalHours,
                ProjectBreakdown = projectBreakdown,
                UserBreakdown = userBreakdown,
                DailyBreakdown = dailyBreakdown
            };
        }
        
        private void CheckViewPermission(Guid projectId)
        {
            // In a real implementation, this method would check if the user has permission
            // to view time logs for the given project. For now, it's a placeholder.
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
using NouAssist.BusinessOperations.ProjectExecution.Exceptions;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs
{
    [Authorize(ProjectExecutionPermissions.TimeLogs.Default)]
    public class TimeLogAppService : ApplicationService, ITimeLogAppService
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeLoggingService _timeLoggingService;
        private readonly IIdentityUserLookupService _userLookupService;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<TimeLogAppService> _logger;
        
        public TimeLogAppService(
            ITimeLogRepository timeLogRepository,
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ITimeLoggingService timeLoggingService,
            IIdentityUserLookupService userLookupService,
            ICurrentUser currentUser,
            ILogger<TimeLogAppService> logger)
        {
            _timeLogRepository = timeLogRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _timeLoggingService = timeLoggingService;
            _userLookupService = userLookupService;
            _currentUser = currentUser;
            _logger = logger;
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.View)]
        public virtual async Task<TimeLogDto> GetAsync(Guid id)
        {
            try
            {
                var timeLog = await _timeLogRepository.GetAsync(id);
                
                // Check ownership if user doesn't have ViewOthers permission
                if (!await AuthorizationService.IsGrantedAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers) &&
                    timeLog.UserId != _currentUser.Id)
                {
                    throw new AbpAuthorizationException("You are not authorized to view this time log");
                }
                
                var timeLogDto = ObjectMapper.Map<TimeLog, TimeLogDto>(timeLog);
                
                // Load user name
                try
                {
                    var user = await _userLookupService.FindByIdAsync(timeLog.UserId);
                    if (user != null)
                    {
                        timeLogDto.UserName = user.Name;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find user {UserId} for time log", timeLog.UserId);
                    timeLogDto.UserName = "Unknown User";
                }
                
                // Load project name
                try
                {
                    var project = await _projectRepository.GetAsync(timeLog.ProjectId);
                    timeLogDto.ProjectName = project.Details.Name;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find project {ProjectId} for time log", timeLog.ProjectId);
                    timeLogDto.ProjectName = "Unknown Project";
                }
                
                // Load task name if applicable
                if (timeLog.TaskId.HasValue)
                {
                    try
                    {
                        var task = await _taskRepository.GetAsync(timeLog.TaskId.Value);
                        timeLogDto.TaskName = task.Details.Name;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find task {TaskId} for time log", timeLog.TaskId);
                        timeLogDto.TaskName = "Unknown Task";
                    }
                }
                
                return timeLogDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.View)]
        public virtual async Task<PagedResultDto<TimeLogDto>> GetListAsync(GetTimeLogListDto input)
        {
            try
            {
                // Apply different filters based on permissions
                var hasViewOthersPermission = await AuthorizationService.IsGrantedAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers);
                
                var query = await _timeLogRepository.GetQueryableAsync();
                
                // Filter by user if no ViewOthers permission
                if (!hasViewOthersPermission)
                {
                    if (!_currentUser.Id.HasValue)
                    {
                        return new PagedResultDto<TimeLogDto>(0, new List<TimeLogDto>());
                    }
                    
                    query = query.Where(tl => tl.UserId == _currentUser.Id.Value);
                }
                else if (input.UserId.HasValue)
                {
                    query = query.Where(tl => tl.UserId == input.UserId.Value);
                }
                
                // Apply other filters
                query = query.WhereIf(
                    input.ProjectId.HasValue,
                    tl => tl.ProjectId == input.ProjectId
                );
                
                query = query.WhereIf(
                    input.TaskId.HasValue,
                    tl => tl.TaskId == input.TaskId
                );
                
                query = query.WhereIf(
                    input.LogDateFrom.HasValue,
                    tl => tl.Details.LogDate >= input.LogDateFrom.Value
                );
                
                query = query.WhereIf(
                    input.LogDateTo.HasValue,
                    tl => tl.Details.LogDate <= input.LogDateTo.Value
                );
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = query.OrderBy(!string.IsNullOrWhiteSpace(input.Sorting) 
                    ? input.Sorting 
                    : "Details.LogDate desc");
                
                // Apply paging
                query = query.PageBy(input);
                
                // Execute query
                var timeLogs = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs and load additional data
                var timeLogDtos = new List<TimeLogDto>();
                
                // Preload user, project, and task data for efficiency
                var userIds = timeLogs.Select(tl => tl.UserId).Distinct().ToList();
                var projectIds = timeLogs.Select(tl => tl.ProjectId).Distinct().ToList();
                var taskIds = timeLogs.Where(tl => tl.TaskId.HasValue)
                    .Select(tl => tl.TaskId.Value).Distinct().ToList();
                
                var userDictionary = new Dictionary<Guid, string>();
                var projectDictionary = new Dictionary<Guid, string>();
                var taskDictionary = new Dictionary<Guid, string>();
                
                // Load users
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
                
                // Load projects
                foreach (var projectId in projectIds)
                {
                    try
                    {
                        var project = await _projectRepository.GetAsync(projectId);
                        projectDictionary[projectId] = project.Details.Name;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find project {ProjectId}", projectId);
                        projectDictionary[projectId] = "Unknown Project";
                    }
                }
                
                // Load tasks
                foreach (var taskId in taskIds)
                {
                    try
                    {
                        var task = await _taskRepository.GetAsync(taskId);
                        taskDictionary[taskId] = task.Details.Name;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find task {TaskId}", taskId);
                        taskDictionary[taskId] = "Unknown Task";
                    }
                }
                
                // Map time logs to DTOs
                foreach (var timeLog in timeLogs)
                {
                    var timeLogDto = ObjectMapper.Map<TimeLog, TimeLogDto>(timeLog);
                    
                    // Set user, project, and task names
                    if (userDictionary.ContainsKey(timeLog.UserId))
                    {
                        timeLogDto.UserName = userDictionary[timeLog.UserId];
                    }
                    
                    if (projectDictionary.ContainsKey(timeLog.ProjectId))
                    {
                        timeLogDto.ProjectName = projectDictionary[timeLog.ProjectId];
                    }
                    
                    if (timeLog.TaskId.HasValue && taskDictionary.ContainsKey(timeLog.TaskId.Value))
                    {
                        timeLogDto.TaskName = taskDictionary[timeLog.TaskId.Value];
                    }
                    
                    timeLogDtos.Add(timeLogDto);
                }
                
                return new PagedResultDto<TimeLogDto>(totalCount, timeLogDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.View)]
        public virtual async Task<PagedResultDto<TimeLogDto>> GetMyTimeLogsAsync(GetTimeLogListDto input)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new PagedResultDto<TimeLogDto>(0, new List<TimeLogDto>());
                }
                
                // Override UserId filter with current user
                var myInput = new GetTimeLogListDto
                {
                    UserId = _currentUser.Id.Value,
                    ProjectId = input.ProjectId,
                    TaskId = input.TaskId,
                    LogDateFrom = input.LogDateFrom,
                    LogDateTo = input.LogDateTo,
                    Sorting = input.Sorting,
                    SkipCount = input.SkipCount,
                    MaxResultCount = input.MaxResultCount
                };
                
                return await GetListAsync(myInput);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.Create)]
        public virtual async Task<TimeLogDto> CreateAsync(CreateTimeLogDto input)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    throw new UserFriendlyException("User ID not available");
                }
                
                var timeLog = await _timeLoggingService.LogTimeAsync(
                    _currentUser.Id.Value,
                    input.ProjectId,
                    input.Hours,
                    input.LogDate,
                    input.TaskId,
                    input.Notes,
                    input.ResourceInstanceId
                );
                
                var timeLogDto = ObjectMapper.Map<TimeLog, TimeLogDto>(timeLog);
                
                // Set current user name
                try
                {
                    var user = await _userLookupService.FindByIdAsync(_currentUser.Id.Value);
                    if (user != null)
                    {
                        timeLogDto.UserName = user.Name;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find current user {UserId}", _currentUser.Id.Value);
                }
                
                // Load project name
                try
                {
                    var project = await _projectRepository.GetAsync(input.ProjectId);
                    timeLogDto.ProjectName = project.Details.Name;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find project {ProjectId}", input.ProjectId);
                }
                
                // Load task name if applicable
                if (input.TaskId.HasValue)
                {
                    try
                    {
                        var task = await _taskRepository.GetAsync(input.TaskId.Value);
                        timeLogDto.TaskName = task.Details.Name;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find task {TaskId}", input.TaskId);
                    }
                }
                
                return timeLogDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.Update)]
        public virtual async Task<TimeLogDto> UpdateAsync(Guid id, UpdateTimeLogDto input)
        {
            try
            {
                var timeLog = await _timeLogRepository.GetAsync(id);
                
                // Check ownership if user doesn't have ViewOthers permission
                if (!await AuthorizationService.IsGrantedAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers) &&
                    timeLog.UserId != _currentUser.Id)
                {
                    throw new AbpAuthorizationException("You are not authorized to update this time log");
                }
                
                timeLog.UpdateDetails(
                    input.Hours,
                    input.LogDate,
                    input.Notes,
                    input.ResourceInstanceId
                );
                
                await _timeLogRepository.UpdateAsync(timeLog);
                
                var timeLogDto = ObjectMapper.Map<TimeLog, TimeLogDto>(timeLog);
                
                // Load user name
                try
                {
                    var user = await _userLookupService.FindByIdAsync(timeLog.UserId);
                    if (user != null)
                    {
                        timeLogDto.UserName = user.Name;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find user {UserId}", timeLog.UserId);
                }
                
                // Load project name
                try
                {
                    var project = await _projectRepository.GetAsync(timeLog.ProjectId);
                    timeLogDto.ProjectName = project.Details.Name;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not find project {ProjectId}", timeLog.ProjectId);
                }
                
                // Load task name if applicable
                if (timeLog.TaskId.HasValue)
                {
                    try
                    {
                        var task = await _taskRepository.GetAsync(timeLog.TaskId.Value);
                        timeLogDto.TaskName = task.Details.Name;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not find task {TaskId}", timeLog.TaskId);
                    }
                }
                
                return timeLogDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var timeLog = await _timeLogRepository.GetAsync(id);
                
                // Check ownership if user doesn't have ViewOthers permission
                if (!await AuthorizationService.IsGrantedAsync(ProjectExecutionPermissions.TimeLogs.ViewOthers) &&
                    timeLog.UserId != _currentUser.Id)
                {
                    throw new AbpAuthorizationException("You are not authorized to delete this time log");
                }
                
                await _timeLogRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.View)]
        public virtual async Task<TimeLogSummaryDto> GetProjectSummaryAsync(
            Guid projectId, 
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            try
            {
                var timeLogs = await _timeLogRepository.GetListByProjectIdAsync(
                    projectId, 
                    startDate, 
                    endDate);
                
                return CreateSummaryDto(timeLogs);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.View)]
        public virtual async Task<TimeLogSummaryDto> GetTaskSummaryAsync(Guid taskId)
        {
            try
            {
                var timeLogs = await _timeLogRepository.GetListByTaskIdAsync(taskId);
                
                return CreateSummaryDto(timeLogs);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        [Authorize(ProjectExecutionPermissions.TimeLogs.View)]
        public virtual async Task<TimeLogSummaryDto> GetMyTimeLogSummaryAsync(
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new TimeLogSummaryDto 
                    { 
                        TotalHours = 0,
                        HoursByDate = new Dictionary<DateTime, double>(),
                        HoursByUser = new Dictionary<Guid, double>(),
                        HoursByTask = new Dictionary<Guid, double>()
                    };
                }
                
                var timeLogs = await _timeLogRepository.GetListByUserIdAsync(
                    _currentUser.Id.Value, 
                    startDate, 
                    endDate);
                
                return CreateSummaryDto(timeLogs);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        private TimeLogSummaryDto CreateSummaryDto(List<TimeLog> timeLogs)
        {
            var summary = new TimeLogSummaryDto();
            
            // Calculate total hours
            summary.TotalHours = timeLogs.Sum(tl => tl.Details.Hours);
            
            // Group by date
            var byDate = timeLogs
                .GroupBy(tl => tl.Details.LogDate.Date)
                .ToDictionary(g => g.Key, g => g.Sum(tl => tl.Details.Hours));
                
            summary.HoursByDate = byDate;
            
            // Group by user
            var byUser = timeLogs
                .GroupBy(tl => tl.UserId)
                .ToDictionary(g => g.Key, g => g.Sum(tl => tl.Details.Hours));
                
            summary.HoursByUser = byUser;
            
            // Group by task
            var byTask = timeLogs
                .Where(tl => tl.TaskId.HasValue)
                .GroupBy(tl => tl.TaskId.Value)
                .ToDictionary(g => g.Key, g => g.Sum(tl => tl.Details.Hours));
                
            summary.HoursByTask = byTask;
            
            return summary;
        }
        
        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "An error occurred in TimeLogAppService: {Message}", exception.Message);
            
            if (exception is EntityNotFoundException entityNotFoundException)
            {
                throw new UserFriendlyException($"TimeLog with ID {entityNotFoundException.Id} not found");
            }
            
            if (exception is ProjectExecutionDomainException domainException)
            {
                throw new UserFriendlyException(domainException.Message, domainException.Code, domainException.Data);
            }
        }
    }
}
