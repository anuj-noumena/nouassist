using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IInventoryItemRepository
    /// </summary>
    public class EfCoreInventoryItemRepository 
        : EfCoreRepository<RIMDbContext, InventoryItemDefinitionAggregate, Guid>, IInventoryItemRepository
    {
        /// <summary>
        /// Creates a new instance of EfCoreInventoryItemRepository
        /// </summary>
        /// <param name="dbContextProvider">Database context provider</param>
        public EfCoreInventoryItemRepository(
            IDbContextProvider<RIMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Includes details (stock levels, transactions) when fetching inventory items
        /// </summary>
        /// <param name="includeDetails">Whether to include details</param>
        /// <returns>Queryable with optional details</returns>
        public override async Task<IQueryable<InventoryItemDefinitionAggregate>> WithDetailsAsync(bool includeDetails = true)
        {
            if (!includeDetails)
            {
                return await base.WithDetailsAsync(includeDetails);
            }

            return (await GetQueryableAsync())
                .Include(ii => ii.StockLevels)
                .Include(ii => ii.Transactions);
        }

        /// <summary>
        /// Finds an inventory item by its SKU
        /// </summary>
        /// <param name="sku">Stock keeping unit</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Inventory item or null if not found</returns>
        public async Task<InventoryItemDefinitionAggregate> FindBySkuAsync(
            string sku,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .FirstOrDefaultAsync(ii => EF.Property<string>(ii, "Sku") == sku, cancellationToken);
        }

        /// <summary>
        /// Checks if an inventory item with the specified SKU exists
        /// </summary>
        /// <param name="sku">Stock keeping unit</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        public async Task<bool> ExistsBySkuAsync(
            string sku,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .AnyAsync(ii => EF.Property<string>(ii, "Sku") == sku, cancellationToken);
        }

        /// <summary>
        /// Gets a list of inventory items by category path
        /// </summary>
        /// <param name="categoryPath">Category path</param>
        /// <param name="includeChildCategories">Whether to include items in child categories</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetListByCategoryPathAsync(
            string categoryPath,
            bool includeChildCategories = false,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);

            if (includeChildCategories)
            {
                // Include all inventory items where the category path starts with the specified path
                return await query
                    .Where(ii => EF.Property<string>(ii, "CategoryPath").StartsWith(categoryPath))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                // Include only inventory items with an exact category path match
                return await query
                    .Where(ii => EF.Property<string>(ii, "CategoryPath") == categoryPath)
                    .ToListAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Gets a list of active inventory items
        /// </summary>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active inventory items</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetActiveListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ii => ii.IsActive)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of inventory items that are below their minimum stock level
        /// </summary>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items below minimum stock level</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetItemsBelowMinimumStockLevelAsync(
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            // Get all active inventory items with their stock levels
            var query = await WithDetailsAsync(true);
            var items = await query
                .Where(ii => ii.IsActive)
                .ToListAsync(cancellationToken);

            // Filter in memory for items below minimum stock level
            var result = items.Where(ii => ii.IsBelowMinimumStockLevel()).ToList();

            // If details should be excluded, reload without details
            if (!includeDetails)
            {
                var resultIds = result.Select(ii => ii.Id).ToList();
                query = await GetQueryableAsync();
                return await query
                    .Where(ii => resultIds.Contains(ii.Id))
                    .ToListAsync(cancellationToken);
            }

            return result;
        }

        /// <summary>
        /// Gets a list of inventory items at a specific location that are below minimum stock level
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items below minimum stock level at the specified location</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetItemsBelowMinimumStockLevelAtLocationAsync(
            Guid locationId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            // Get all active inventory items with their stock levels
            var query = await WithDetailsAsync(true);
            var items = await query
                .Where(ii => ii.IsActive && ii.StockLevels.Any(sl => sl.LocationId == locationId))
                .ToListAsync(cancellationToken);

            // Filter in memory for items below minimum stock level at the specified location
            var result = items.Where(ii => 
            {
                var stockLevel = ii.GetStockLevel(locationId);
                return stockLevel != null && stockLevel.Quantity < ii.MinimumStockLevel;
            }).ToList();

            // If details should be excluded, reload without details
            if (!includeDetails)
            {
                var resultIds = result.Select(ii => ii.Id).ToList();
                query = await GetQueryableAsync();
                return await query
                    .Where(ii => resultIds.Contains(ii.Id))
                    .ToListAsync(cancellationToken);
            }

            return result;
        }

        /// <summary>
        /// Gets a list of inventory items at a specific location
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items at the specified location</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetItemsAtLocationAsync(
            Guid locationId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ii => ii.StockLevels.Any(sl => sl.LocationId == locationId))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of inventory items by barcode
        /// </summary>
        /// <param name="barcode">Barcode</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetListByBarcodeAsync(
            string barcode,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ii => ii.Barcode == barcode)
                .ToListAsync(cancellationToken);
        }
    }
}
