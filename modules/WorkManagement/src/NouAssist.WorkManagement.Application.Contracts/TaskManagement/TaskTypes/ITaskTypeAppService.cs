using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.TaskTypes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.TaskManagement.TaskTypes
{
    public interface ITaskTypeAppService : IApplicationService
    {
        Task<TaskTypeDto> GetAsync(Guid id);
        Task<PagedResultDto<TaskTypeDto>> GetListAsync(GetTaskTypeListDto input);
        Task<ListResultDto<TaskTypeDto>> GetActiveListAsync();
        Task<TaskTypeDto> CreateAsync(CreateTaskTypeDto input);
        Task<TaskTypeDto> UpdateAsync(Guid id, UpdateTaskTypeDto input);
        Task DeleteAsync(Guid id);
        
        Task<TaskTypeDto> ActivateAsync(Guid id);
        Task<TaskTypeDto> DeactivateAsync(Guid id);
    }
}
