using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances
{
    public interface ITaskInstanceAppService : IApplicationService
    {
        Task<TaskInstanceDto> GetAsync(Guid id);
        Task<TaskInstanceDetailDto> GetDetailAsync(Guid id);
        Task<PagedResultDto<TaskInstanceDto>> GetListAsync(GetTaskInstanceListDto input);
        Task<TaskInstanceDto> CreateAsync(CreateTaskInstanceDto input);
        Task<TaskInstanceDto> UpdateAsync(Guid id, UpdateTaskInstanceDto input);
        Task DeleteAsync(Guid id);
        
        // Status Management
        Task<TaskInstanceDto> StartAsync(Guid id);
        Task<TaskInstanceDto> CompleteAsync(Guid id, CompleteTaskDto input);
        Task<TaskInstanceDto> ReopenAsync(Guid id);
        Task<TaskInstanceDto> TransitionToAsync(Guid id, Guid workflowStatusId, string comment = null);
        
        // Assignment
        Task<TaskInstanceDto> AssignAsync(Guid id, AssignTaskDto input);
        Task<TaskInstanceDto> UnassignAsync(Guid id);
        
        // Delegation
        Task<TaskInstanceDto> DelegateAsync(Guid id, DelegateTaskDto input);
        Task<TaskInstanceDto> AcceptDelegationAsync(Guid id, AcceptDelegationDto input);
        Task<TaskInstanceDto> DeclineDelegationAsync(Guid id, DeclineDelegationDto input);
        
        // Blocking
        Task<TaskInstanceDto> BlockAsync(Guid id, BlockTaskDto input);
        Task<TaskInstanceDto> UnblockAsync(Guid id);
        
        // Progress Management - Updated from Step to ChecklistItem
        Task<TaskInstanceDto> CheckItemAsync(Guid id, Guid checklistItemId);
        Task<TaskInstanceDto> UncheckItemAsync(Guid id, Guid checklistItemId);
        Task<TaskInstanceDto> MarkCriterionAsMetAsync(Guid id, MarkCriterionMetDto input);
        Task<TaskInstanceDto> UnmarkCriterionAsMetAsync(Guid id, Guid criterionId);

        // Time Tracking
        Task<TaskInstanceDto> LogTimeAsync(Guid id, LogTimeDto input);
        
        // Collaboration
        Task<TaskInstanceDto> AddWatcherAsync(Guid id, Guid userId);
        Task<TaskInstanceDto> RemoveWatcherAsync(Guid id, Guid userId);
        Task<TaskInstanceDto> IncrementCommentsAsync(Guid id);
        Task<TaskInstanceDto> IncrementAttachmentsAsync(Guid id);
        
        // Dependencies
        Task<TaskInstanceDto> AddDependencyAsync(Guid id, AddDependencyDto input);
        Task<TaskInstanceDto> RemoveDependencyAsync(Guid id, RemoveDependencyDto input);
        Task<List<TaskInstanceDto>> GetDependentTasksAsync(Guid id);
        Task<List<TaskInstanceDto>> GetBlockingTasksAsync(Guid id);
        Task<bool> CanStartAsync(Guid id);
        
        // GTD Organization
        Task<TaskInstanceDto> AssignToAreaAsync(Guid id, Guid areaId);
        Task<TaskInstanceDto> RemoveFromAreaAsync(Guid id);
        Task<TaskInstanceDto> AssignToContextAsync(Guid id, Guid contextId);
        Task<TaskInstanceDto> RemoveFromContextAsync(Guid id);
        Task<TaskInstanceDto> MoveToProjectAsync(Guid id, MoveTaskDto input);
        Task<TaskInstanceDto> MoveToAreaAsync(Guid id, Guid areaId);
        
        // Flags and Markers
        Task<TaskInstanceDto> MarkAsNextActionAsync(Guid id);
        Task<TaskInstanceDto> UnmarkAsNextActionAsync(Guid id);
        Task<TaskInstanceDto> FlagTaskAsync(Guid id, string reason);
        Task<TaskInstanceDto> UnflagTaskAsync(Guid id);
        
        // Bulk Operations
        Task<BulkUpdateResultDto> BulkUpdateAsync(BulkUpdateTaskInstancesDto input);
        Task<BulkUpdateResultDto> BulkDeleteAsync(Guid[] taskInstanceIds);
        
        // Statistics & Reports
        Task<TaskInstanceStatisticsDto> GetStatisticsAsync(Guid id);
        Task<List<TaskInstanceDto>> GetOverdueTasksAsync(Guid? projectId = null, Guid? areaId = null);
        Task<List<TaskInstanceDto>> GetMyTasksAsync();
        Task<List<TaskInstanceDto>> GetDelegatedToMeAsync();
        Task<List<TaskInstanceDto>> GetNextActionsAsync(Guid? projectId = null, Guid? areaId = null, Guid? contextId = null);
        
        // GTD-specific queries
        Task<List<TaskInstanceDto>> GetTasksByAreaAsync(Guid areaId);
        Task<List<TaskInstanceDto>> GetTasksByContextAsync(Guid contextId);
    }
}