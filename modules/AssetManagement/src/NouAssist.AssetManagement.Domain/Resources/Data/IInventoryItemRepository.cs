using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.AssetManagement.Resources.Data
{
    public interface IInventoryItemRepository : IRepository<InventoryItem, Guid>
    {
        Task<List<InventoryItem>> GetListAsync(ISpecification<InventoryItem> spec, int maxResultCount = 50);
        Task<List<InventoryItem>> GetAvailableItemsAsync(InventoryItemKind? kind = null);
        Task<List<InventoryItem>> GetCheckedOutItemsAsync();
        Task<List<InventoryItem>> GetOverdueItemsAsync();
        Task<List<InventoryItem>> GetItemsDueForMaintenanceAsync();
        Task<List<InventoryItem>> GetByLocationAsync(string location);
        Task<List<InventoryItem>> GetAssignedToUserAsync(Guid userId);
        Task<InventoryItem> FindByBarcodeAsync(string barcode);
        Task<InventoryItem> FindBySerialNumberAsync(string serialNumber);
    }
}
