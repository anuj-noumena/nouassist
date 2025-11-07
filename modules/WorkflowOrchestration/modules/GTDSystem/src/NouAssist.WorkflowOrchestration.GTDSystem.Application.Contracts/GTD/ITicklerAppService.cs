using System;
using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.GTDSystem.Dtos;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.GTDSystem.GTD
{
    public interface ITicklerAppService : IApplicationService
    {
        Task<TicklerItemDto> GetAsync(Guid id);
        Task<PagedResultDto<TicklerItemDto>> GetListAsync(GetTicklerListDto input);
        Task<ListResultDto<TicklerItemDto>> GetDueTicklersAsync();
        Task<ListResultDto<TicklerItemDto>> GetSurfacedTicklersAsync();
        Task<TicklerItemDto> CreateAsync(CreateTicklerItemDto input);
        Task<TicklerItemDto> UpdateAsync(Guid id, UpdateTicklerItemDto input);
        Task DeleteAsync(Guid id);
        Task<TicklerItemDto> SurfaceAsync(Guid id);
        Task<TicklerItemDto> TakeActionAsync(Guid id, TicklerActionType actionType);
        Task<TicklerItemDto> DismissAsync(Guid id);
        Task<TicklerItemDto> SnoozeAsync(Guid id, SnoozeTicklerDto input);
        Task<TicklerItemDto> RescheduleAsync(Guid id, DateTime newDate);
        Task<TicklerItemDto> SetRecurrenceAsync(Guid id, SetRecurrenceDto input);
        Task ProcessDueTicklersAsync();
    }
}
