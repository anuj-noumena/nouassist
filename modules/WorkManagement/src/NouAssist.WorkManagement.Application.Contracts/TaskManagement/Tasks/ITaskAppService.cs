using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.TaskManagement.Tasks
{
    public interface ITaskAppService : IApplicationService
    {
        Task<TaskDto> GetAsync(Guid id);
        Task<PagedResultDto<TaskDto>> GetListAsync(GetTaskListDto input);
        Task<TaskDto> CreateAsync(CreateTaskDto input);
        Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto input);
        Task DeleteAsync(Guid id);
        
        // Updated from ActionStep to ChecklistItem
        Task<TaskDto> AddChecklistItemAsync(Guid id, CreateChecklistItemDto input);
        Task<TaskDto> UpdateChecklistItemAsync(Guid id, Guid itemId, CreateChecklistItemDto input);
        Task<TaskDto> RemoveChecklistItemAsync(Guid id, Guid itemId);
        Task<TaskDto> ReorderChecklistItemsAsync(Guid id, Guid[] orderedItemIds);
        
        Task<TaskDto> AddAcceptanceCriterionAsync(Guid id, CreateAcceptanceCriterionDto input);
        Task<TaskDto> UpdateAcceptanceCriterionAsync(Guid id, Guid criterionId, CreateAcceptanceCriterionDto input);
        Task<TaskDto> RemoveAcceptanceCriterionAsync(Guid id, Guid criterionId);
        
        // GTD Organization
        Task<TaskDto> AssignToAreaAsync(Guid id, Guid areaId);
        Task<TaskDto> RemoveFromAreaAsync(Guid id);
        Task<TaskDto> AssignToContextAsync(Guid id, Guid contextId);
        Task<TaskDto> RemoveFromContextAsync(Guid id);
        Task<TaskDto> MoveToProjectAsync(Guid id, Guid projectId);
        Task<TaskDto> MoveToAreaAsync(Guid id, Guid areaId);
        
        Task<TaskDto> ActivateAsync(Guid id);
        Task<TaskDto> DeactivateAsync(Guid id);
        Task<TaskDto> CloneAsync(Guid id, CloneTaskDto input);
        
        // Recurrence
        Task<TaskDto> SetRecurrenceAsync(Guid id, RecurrenceRuleDto input);
        Task<TaskDto> RemoveRecurrenceAsync(Guid id);
        
        // Statistics
        Task<TaskStatisticsDto> GetStatisticsAsync(Guid id);
    }
}