using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Exceptions;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.Services
{
    /// <summary>
    /// Implementation of the time logging service
    /// </summary>
    public class TimeLoggingService : DomainService, ITimeLoggingService
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TimeLoggingService> _logger;
        
        public TimeLoggingService(
            ITimeLogRepository timeLogRepository,
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ILogger<TimeLoggingService> logger)
        {
            _timeLogRepository = timeLogRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }
        
        public async Task<TimeLog> LogTimeAsync(
            Guid userId,
            Guid projectId,
            double hours,
            DateTime logDate,
            Guid? taskId = null,
            string notes = null,
            Guid? resourceInstanceId = null)
        {
            try
            {
                // Verify project exists
                var project = await _projectRepository.GetAsync(projectId);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectId);
                }
                
                // Verify task exists and belongs to the project if specified
                if (taskId.HasValue)
                {
                    var task = await _taskRepository.GetAsync(taskId.Value);
                    if (task == null)
                    {
                        throw new TaskNotFoundException(taskId.Value);
                    }
                    
                    if (task.ProjectId != projectId)
                    {
                        throw new BusinessException("ProjectExecution:ErrorMessage:TaskNotPartOfProject");
                    }
                }
                
                // Create and save the time log
                var timeLog = TimeLog.Create(
                    GuidGenerator.Create(),
                    userId,
                    projectId,
                    hours,
                    logDate,
                    taskId,
                    notes,
                    resourceInstanceId,
                    CurrentTenant.Id
                );
                
                await _timeLogRepository.InsertAsync(timeLog);
                
                return timeLog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log time for project {ProjectId}", projectId);
                throw;
            }
        }
        
        public async Task<List<TimeLog>> GetProjectTimeLogsAsync(
            Guid projectId,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                return await _timeLogRepository.GetListByProjectIdAsync(projectId, startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get time logs for project {ProjectId}", projectId);
                throw;
            }
        }
        
        public async Task<List<TimeLog>> GetUserTimeLogsAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                return await _timeLogRepository.GetListByUserIdAsync(userId, startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get time logs for user {UserId}", userId);
                throw;
            }
        }
        
        public async Task<List<TimeLog>> GetTaskTimeLogsAsync(Guid taskId)
        {
            try
            {
                return await _timeLogRepository.GetListByTaskIdAsync(taskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get time logs for task {TaskId}", taskId);
                throw;
            }
        }
    }
}
