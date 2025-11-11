using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Data;
using NouAssist.Resources.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Resources.Services
{
    public class InventoryManagementService : DomainService
    {
        private readonly IInventoryItemRepository _inventoryRepository;

        public InventoryManagementService(IInventoryItemRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<CheckOutResult> CheckOutItemAsync(
            InventoryItem item,
            Guid userId,
            int quantity,
            DateTime? dueDate = null)
        {
            var result = new CheckOutResult();

            if (item.IsRetired)
            {
                result.AddError("Item is retired and cannot be checked out.");
                return result;
            }

            if (item.Status == InventoryStatus.Maintenance || item.Status == InventoryStatus.Repair)
            {
                result.AddError($"Item is under {item.Status.ToString().ToLower()} and cannot be checked out.");
                return result;
            }

            if (quantity > item.QuantityAvailable)
            {
                result.AddError($"Requested quantity ({quantity}) exceeds available quantity ({item.QuantityAvailable}).");
                return result;
            }

            try
            {
                item.CheckOut(userId, quantity, dueDate);
                result.IsSuccessful = true;
                result.CheckedOutItem = item;
            }
            catch (BusinessException ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public async Task<List<InventoryItem>> GetOverdueItemsAsync()
        {
            return await _inventoryRepository.GetOverdueItemsAsync();
        }

        public async Task<List<InventoryItem>> GetItemsDueForMaintenanceAsync()
        {
            return await _inventoryRepository.GetItemsDueForMaintenanceAsync();
        }

        public decimal CalculateTotalInventoryValue(List<InventoryItem> items)
        {
            return items
                .Where(i => !i.IsRetired && i.CurrentValue.HasValue)
                .Sum(i => i.CurrentValue.Value * i.Quantity);
        }

        public Dictionary<InventoryItemKind, int> GetInventoryStatistics(List<InventoryItem> items)
        {
            return items
                .Where(i => !i.IsDeleted && !i.IsRetired)
                .GroupBy(i => i.Kind)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));
        }
    }

    public class CheckOutResult
    {
        
public bool IsSuccessful { get; set; }
        public InventoryItem CheckedOutItem { get; set; }
        public List<string> Errors { get; private set; }

        public CheckOutResult()
        {
            Errors = new List<string>();
            IsSuccessful = false;
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
