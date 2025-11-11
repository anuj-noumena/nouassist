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
        
        Task<TaskDto> AddActionStepAsync(Guid id, CreateTaskActionStepDto input);
        Task<TaskDto> UpdateActionStepAsync(Guid id, Guid stepId, CreateTaskActionStepDto input);
        Task<TaskDto> RemoveActionStepAsync(Guid id, Guid stepId);
        Task<TaskDto> ReorderActionStepsAsync(Guid id, Guid[] orderedStepIds);
        
        Task<TaskDto> AddAcceptanceCriterionAsync(Guid id, CreateAcceptanceCriterionDto input);
        Task<TaskDto> UpdateAcceptanceCriterionAsync(Guid id, Guid criterionId, CreateAcceptanceCriterionDto input);
        Task<TaskDto> RemoveAcceptanceCriterionAsync(Guid id, Guid criterionId);
        
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
