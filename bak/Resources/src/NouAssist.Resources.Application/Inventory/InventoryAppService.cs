using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Data;
using NouAssist.Resources.Enums;
using NouAssist.Resources.Inventory.Dtos;
using NouAssist.Resources.Permissions;
using NouAssist.Resources.Services;
using NouAssist.Resources.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Resources.Inventory
{
    [Authorize(ResourcesPermissions.Inventory.Default)]
    public class InventoryAppService : ApplicationService, IInventoryAppService
    {
        private readonly IInventoryItemRepository _inventoryRepository;
        private readonly InventoryManagementService _managementService;

        public InventoryAppService(
            IInventoryItemRepository inventoryRepository,
            InventoryManagementService managementService)
        {
            _inventoryRepository = inventoryRepository;
            _managementService = managementService;
        }

        public async Task<InventoryItemDto> GetAsync(Guid id)
        {
            var item = await _inventoryRepository.GetAsync(id);
            return MapToDto(item);
        }

        public async Task<PagedResultDto<InventoryItemDto>> GetListAsync(GetInventoryListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var items = await _inventoryRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _inventoryRepository.CountAsync(spec.ToExpression());

            return new PagedResultDto<InventoryItemDto>(
                totalCount,
                items.Select(MapToDto).ToList()
            );
        }

        [Authorize(ResourcesPermissions.Inventory.Create)]
        public async Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto input)
        {
            var item = new InventoryItem(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.Kind,
                input.Name,
                CurrentTenant.Id,
                input.Description,
                input.Quantity,
                input.Location
            );

            if (!string.IsNullOrWhiteSpace(input.Manufacturer))
            {
                // Set via reflection or add setter methods to aggregate
                typeof(InventoryItem).GetProperty(nameof(InventoryItem.Manufacturer))
                    ?.SetValue(item, input.Manufacturer);
            }

            if (!string.IsNullOrWhiteSpace(input.Model))
            {
                typeof(InventoryItem).GetProperty(nameof(InventoryItem.Model))
                    ?.SetValue(item, input.Model);
            }

            if (!string.IsNullOrWhiteSpace(input.Barcode))
            {
                typeof(InventoryItem).GetProperty(nameof(InventoryItem.Barcode))
                    ?.SetValue(item, input.Barcode);
            }

            if (!string.IsNullOrWhiteSpace(input.SerialNumber))
            {
                typeof(InventoryItem).GetProperty(nameof(InventoryItem.SerialNumber))
                    ?.SetValue(item, input.SerialNumber);
            }

            item.UpdateCondition(input.Condition);

            if (!string.IsNullOrWhiteSpace(input.Building) || !string.IsNullOrWhiteSpace(input.Zone))
            {
                item.UpdateLocation(input.Location ?? "", input.Building, input.Zone);
            }

            if (input.PurchasePrice.HasValue || input.PurchaseDate.HasValue)
            {
                item.SetPurchaseInfo(input.PurchasePrice, input.PurchaseDate);
            }

            await _inventoryRepository.InsertAsync(item);

            return MapToDto(item);
        }

        [Authorize(ResourcesPermissions.Inventory.Update)]
        public async Task<InventoryItemDto> UpdateAsync(Guid id, UpdateInventoryItemDto input)
        {
            var item = await _inventoryRepository.GetAsync(id);

            // Authorization check (could be owner or admin)
            if (item.OwnerId != CurrentUser.Id.Value && 
                !await AuthorizationService.IsGrantedAsync(ResourcesPermissions.Inventory.ViewAll))
            {
                throw new BusinessException("Inventory:NotAuthorized", "You are not authorized to update this item.");
            }

            // Update properties using domain methods
            typeof(InventoryItem).GetProperty(nameof(InventoryItem.Name))?.SetValue(item, input.Name);
            typeof(InventoryItem).GetProperty(nameof(InventoryItem.Description))?.SetValue(item, input.Description);
            typeof(InventoryItem).GetProperty(nameof(InventoryItem.Manufacturer))?.SetValue(item, input.Manufacturer);
            typeof(InventoryItem).GetProperty(nameof(InventoryItem.Model))?.SetValue(item, input.Model);

            item.UpdateCondition(input.Condition);
            item.UpdateLocation(input.Location, input.Building, input.Zone);

            if (input.CurrentValue.HasValue)
            {
                item.SetCurrentValue(input.CurrentValue.Value);
            }

            await _inventoryRepository.UpdateAsync(item);

            return MapToDto(item);
        }

        [Authorize(ResourcesPermissions.Inventory.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var item = await _inventoryRepository.GetAsync(id);

            // Authorization check
            if (item.OwnerId != CurrentUser.Id.Value && 
                !await AuthorizationService.IsGrantedAsync(ResourcesPermissions.Inventory.ViewAll))
            {
                throw new BusinessException("Inventory:NotAuthorized", "You are not authorized to delete this item.");
            }

            item.Delete(CurrentUser.Id.Value.ToString());
            await _inventoryRepository.UpdateAsync(item);
        }

        [Authorize(ResourcesPermissions.Inventory.CheckOut)]
        public async Task<InventoryItemDto> CheckOutAsync(Guid id, CheckOutInventoryDto input)
        {
            var item = await _inventoryRepository.GetAsync(id);

            // Use domain service for check-out logic
            var result = await _managementService.CheckOutItemAsync(
                item,
                input.UserId,
                input.Quantity,
                input.DueDate
            );

            if (!result.IsSuccessful)
            {
                throw new BusinessException("Inventory:CheckOutFailed", string.Join(", ", result.Errors));
            }

            await _inventoryRepository.UpdateAsync(result.CheckedOutItem);

            return MapToDto(result.CheckedOutItem);
        }

        [Authorize(ResourcesPermissions.Inventory.CheckIn)]
        public async Task<InventoryItemDto> CheckInAsync(Guid id, CheckInInventoryDto input)
        {
            var item = await _inventoryRepository.GetAsync(id);

            item.CheckIn(input.Quantity);

            await _inventoryRepository.UpdateAsync(item);

            return MapToDto(item);
        }

        [Authorize(ResourcesPermissions.Inventory.ScheduleMaintenance)]
        public async Task<InventoryItemDto> ScheduleMaintenanceAsync(Guid id, ScheduleMaintenanceDto input)
        {
            var item = await _inventoryRepository.GetAsync(id);

            item.ScheduleMaintenance(input.MaintenanceDate);

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                typeof(InventoryItem).GetProperty(nameof(InventoryItem.MaintenanceNotes))
                    ?.SetValue(item, input.Notes);
            }

            await _inventoryRepository.UpdateAsync(item);

            return MapToDto(item);
        }

        [Authorize(ResourcesPermissions.Inventory.ScheduleMaintenance)]
        public async Task<InventoryItemDto> CompleteMaintenanceAsync(Guid id, string notes)
        {
            var item = await _inventoryRepository.GetAsync(id);

            item.CompleteMaintenance(notes);

            await _inventoryRepository.UpdateAsync(item);

            return MapToDto(item);
        }

        [Authorize(ResourcesPermissions.Inventory.ViewAll)]
        public async Task<List<InventoryItemDto>> GetOverdueItemsAsync()
        {
            var items = await _managementService.GetOverdueItemsAsync();
            return items.Select(MapToDto).ToList();
        }

        [Authorize(ResourcesPermissions.Inventory.ViewAll)]
        public async Task<List<InventoryItemDto>> GetItemsDueForMaintenanceAsync()
        {
            var items = await _managementService.GetItemsDueForMaintenanceAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<List<InventoryItemDto>> GetMyCheckedOutItemsAsync()
        {
            var spec = new AssignedInventorySpecification(CurrentUser.Id.Value);
            var items = await _inventoryRepository.GetListAsync(spec, 100);
            return items.Select(MapToDto).ToList();
        }

        private ISpecification<InventoryItem> CreateListSpecification(GetInventoryListDto input)
        {
            var spec = Specification.True<InventoryItem>();

            if (input.Kind.HasValue)
            {
                spec = spec.And(new InventoryByKindSpecification(input.Kind.Value));
            }

            if (input.Status.HasValue)
            {
                spec = spec.And(new Specification<InventoryItem>(i => i.Status == input.Status.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Location))
            {
                spec = spec.And(new InventoryByLocationSpecification(input.Location));
            }

            if (input.IsAvailable.HasValue && input.IsAvailable.Value)
            {
                spec = spec.And(new AvailableInventorySpecification());
            }

            if (input.IsOverdue.HasValue && input.IsOverdue.Value)
            {
                spec = spec.And(new OverdueInventorySpecification(Clock.Now));
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                spec = spec.And(new Specification<InventoryItem>(i => 
                    i.Name.Contains(input.SearchTerm) || 
                    (i.Description != null && i.Description.Contains(input.SearchTerm))));
            }

            return spec;
        }

        private InventoryItemDto MapToDto(InventoryItem item)
        {
            var dto = ObjectMapper.Map<InventoryItem, InventoryItemDto>(item);
            dto.DisplayLocation = item.GetDisplayLocation();
            dto.QuantityCheckedOut = item.QuantityCheckedOut;
            dto.IsAvailable = item.IsAvailable;
            dto.IsOverdue = item.IsOverdue;
            dto.NeedsMaintenance = item.NeedsMaintenance;
            return dto;
        }
    }
}
