using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ProjectExecution.Exceptions;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.Services
{
    /// <summary>
    /// Implementation of the task assignment service
    /// </summary>
    public class TaskAssignmentService : DomainService, ITaskAssignmentService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskAssignmentService> _logger;
        
        public TaskAssignmentService(
            ITaskRepository taskRepository,
            ILogger<TaskAssignmentService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }
        
        public async Task AssignTaskAsync(Guid taskId, Guid userId)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(taskId, true, false);
                if (task == null)
                {
                    throw new TaskNotFoundException(taskId);
                }
                
                task.AssignTo(userId);
                await _taskRepository.UpdateAsync(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign task {TaskId} to user {UserId}", taskId, userId);
                throw;
            }
        }
        
        public async Task UnassignTaskAsync(Guid taskId, Guid userId)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(taskId, true, false);
                if (task == null)
                {
                    throw new TaskNotFoundException(taskId);
                }
                
                task.UnassignFrom(userId);
                await _taskRepository.UpdateAsync(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to unassign task {TaskId} from user {UserId}", taskId, userId);
                throw;
            }
        }
        
        public async Task<List<Task>> GetUserTasksAsync(Guid userId)
        {
            try
            {
                return await _taskRepository.GetListByAssignedUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get tasks for user {UserId}", userId);
                throw;
            }
        }
        
        public async Task<List<Guid>> GetTaskAssigneesAsync(Guid taskId)
        {
            try
            {
                var task = await _taskRepository.GetWithDetailsAsync(taskId, true, false);
                if (task == null)
                {
                    throw new TaskNotFoundException(taskId);
                }
                
                return task.Assignments
                    .Where(a => a.IsActive)
                    .Select(a => a.UserId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get assignees for task {TaskId}", taskId);
                throw;
            }
        }
    }
}
