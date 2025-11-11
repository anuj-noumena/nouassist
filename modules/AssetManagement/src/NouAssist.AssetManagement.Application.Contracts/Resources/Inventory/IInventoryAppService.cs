using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.AssetManagement.Resources.Inventory.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.AssetManagement.Resources.Inventory
{
    public interface IInventoryAppService : IApplicationService
    {
        Task<InventoryItemDto> GetAsync(Guid id);
        Task<PagedResultDto<InventoryItemDto>> GetListAsync(GetInventoryListDto input);
        Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto input);
        Task<InventoryItemDto> UpdateAsync(Guid id, UpdateInventoryItemDto input);
        Task DeleteAsync(Guid id);
        Task<InventoryItemDto> CheckOutAsync(Guid id, CheckOutInventoryDto input);
        Task<InventoryItemDto> CheckInAsync(Guid id, CheckInInventoryDto input);
        Task<InventoryItemDto> ScheduleMaintenanceAsync(Guid id, ScheduleMaintenanceDto input);
        Task<InventoryItemDto> CompleteMaintenanceAsync(Guid id, string notes);
        Task<List<InventoryItemDto>> GetOverdueItemsAsync();
        Task<List<InventoryItemDto>> GetItemsDueForMaintenanceAsync();
        Task<List<InventoryItemDto>> GetMyCheckedOutItemsAsync();
    }
}
