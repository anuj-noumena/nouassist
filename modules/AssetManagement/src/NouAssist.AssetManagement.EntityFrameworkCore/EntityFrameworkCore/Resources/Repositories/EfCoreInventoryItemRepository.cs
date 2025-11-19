using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Data;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.AssetManagement.EntityFrameworkCore.Resources.EntityFrameworkCore.Repositories
{
    public class EfCoreInventoryItemRepository 
        : EfCoreRepository<IAssetManagementDbContext, InventoryItem, Guid>, IInventoryItemRepository
    {
        public EfCoreInventoryItemRepository(
            IDbContextProvider<IAssetManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<InventoryItem>> GetListAsync(
            ISpecification<InventoryItem> spec, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderBy(i => i.Name)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<InventoryItem>> GetAvailableItemsAsync(InventoryItemKind? kind = null)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(i => i.Status == InventoryStatus.Available && 
                                    i.QuantityAvailable > 0 && 
                                    !i.IsDeleted);

            if (kind.HasValue)
            {
                query = query.Where(i => i.Kind == kind.Value);
            }

            return await query.OrderBy(i => i.Name).ToListAsync();
        }

        public async Task<List<InventoryItem>> GetCheckedOutItemsAsync()
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.Status == InventoryStatus.CheckedOut && !i.IsDeleted)
                .OrderBy(i => i.DueBackAt)
                .ToListAsync();
        }

        public async Task<List<InventoryItem>> GetOverdueItemsAsync()
        {
            var now = DateTime.UtcNow;
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.DueBackAt.HasValue && 
                           i.DueBackAt.Value < now && 
                           i.AssignedToUserId.HasValue && 
                           !i.IsDeleted)
                .OrderBy(i => i.DueBackAt)
                .ToListAsync();
        }

        public async Task<List<InventoryItem>> GetItemsDueForMaintenanceAsync()
        {
            var now = DateTime.UtcNow;
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.NextMaintenanceAt.HasValue && 
                           i.NextMaintenanceAt.Value <= now && 
                           !i.IsDeleted)
                .OrderBy(i => i.NextMaintenanceAt)
                .ToListAsync();
        }

        public async Task<List<InventoryItem>> GetByLocationAsync(string location)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.Location == location && !i.IsDeleted)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<List<InventoryItem>> GetAssignedToUserAsync(Guid userId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(i => i.AssignedToUserId == userId && !i.IsDeleted)
                .OrderBy(i => i.DueBackAt)
                .ToListAsync();
        }

        public async Task<InventoryItem> FindByBarcodeAsync(string barcode)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(i => i.Barcode == barcode && !i.IsDeleted);
        }

        public async Task<InventoryItem> FindBySerialNumberAsync(string serialNumber)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(i => i.SerialNumber == serialNumber && !i.IsDeleted);
        }
    }
}
