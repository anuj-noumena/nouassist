using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Tasks.TaskInstances.Dtos;
using NouAssist.Tasks.Tasks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Tasks.TaskInstances
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
        Task<TaskInstanceDto> CompleteAsync(Guid id, UpdateTaskStatusDto input);
        Task<TaskInstanceDto> CancelAsync(Guid id, string reason);
        Task<TaskInstanceDto> ChangeWorkflowStatusAsync(Guid id, ChangeWorkflowStatusDto input);
        Task<TaskInstanceDto> ReopenAsync(Guid id);
        
        // Assignment
        Task<TaskInstanceDto> AssignAsync(Guid id, AssignTaskDto input);
        Task<TaskInstanceDto> UnassignAsync(Guid id);
        Task<TaskInstanceDto> ReassignAsync(Guid id, AssignTaskDto input);
        
        // Delegation
        Task<TaskInstanceDto> DelegateAsync(Guid id, DelegateTaskDto input);
        Task<TaskInstanceDto> AcceptDelegationAsync(Guid id, AcceptDelegationDto input);
        Task<TaskInstanceDto> DeclineDelegationAsync(Guid id, DeclineDelegationDto input);
        Task<TaskInstanceDto> RevokeDelegationAsync(Guid id);
        
        // Blocking
        Task<TaskInstanceDto> BlockAsync(Guid id, BlockTaskDto input);
        Task<TaskInstanceDto> UnblockAsync(Guid id);
        Task<TaskInstanceDto> BlockWaitingForUserAsync(Guid id, string userName, string additionalContext = null);
        
        // Progress Management
        Task<TaskInstanceDto> CompleteStepAsync(Guid id, CompleteStepDto input);
        Task<TaskInstanceDto> UncompleteStepAsync(Guid id, Guid stepId);
        Task<TaskInstanceDto> MarkCriterionAsMetAsync(Guid id, MarkCriterionMetDto input);
        Task<TaskInstanceDto> UnmarkCriterionAsMetAsync(Guid id, Guid criterionId);

        // Time Tracking
        Task<TaskInstanceDto> LogTimeAsync(Guid id, LogTimeDto input);
        
        // Watchers
        Task<TaskInstanceDto> AddWatcherAsync(Guid id, Guid userId);
        Task<TaskInstanceDto> RemoveWatcherAsync(Guid id, Guid userId);
        
        // Subtask Management
        Task<TaskInstanceDto> CreateSubtaskAsync(Guid parentId, CreateTaskInstanceDto input);
        Task<List<TaskInstanceDto>> GetSubtasksAsync(Guid parentId);
        Task<TaskInstanceDto> SetParentAsync(Guid id, SetParentTaskDto input);
        Task<TaskInstanceDto> RemoveParentAsync(Guid id);
        Task<TaskInstanceDto> ReorderSubtasksAsync(Guid parentId, ReorderSubtasksDto input);
        
        // Dependencies
        Task<TaskInstanceDto> AddDependencyAsync(Guid id, AddDependencyDto input);
        Task<TaskInstanceDto> RemoveDependencyAsync(Guid id, RemoveDependencyDto input);
        Task<List<TaskInstanceDto>> GetDependentTasksAsync(Guid id);
        Task<List<TaskInstanceDto>> GetBlockingTasksAsync(Guid id);
        Task<bool> CanStartAsync(Guid id);
        
        // Task Movement
        Task<TaskInstanceDto> MoveToProjectAsync(Guid id, MoveTaskDto input);
        
        // Flags and Markers
        Task<TaskInstanceDto> MarkAsNextActionAsync(Guid id);
        Task<TaskInstanceDto> UnmarkAsNextActionAsync(Guid id);
        Task<TaskInstanceDto> FlagTaskAsync(Guid id, string reason);
        Task<TaskInstanceDto> UnflagTaskAsync(Guid id);
        
        // Discussion
        Task<TaskInstanceDto> LockDiscussionAsync(Guid id);
        Task<TaskInstanceDto> UnlockDiscussionAsync(Guid id);
        
        // Bulk Operations
        Task<BulkUpdateResultDto> BulkUpdateAsync(BulkUpdateTaskInstancesDto input);
        Task<BulkUpdateResultDto> BulkDeleteAsync(Guid[] taskInstanceIds);
        
        // Statistics & Reports
        Task<TaskInstanceStatisticsDto> GetStatisticsAsync(Guid id);
        Task<List<TaskInstanceDto>> GetOverdueTasksAsync(Guid? projectId = null);
        Task<List<TaskInstanceDto>> GetMyTasksAsync();
        Task<List<TaskInstanceDto>> GetDelegatedToMeAsync();
        Task<List<TaskInstanceDto>> GetNextActionsAsync(Guid? projectId = null);
    }
}
