using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Repository interface for the InventoryItemDefinition aggregate
    /// </summary>
    public interface IInventoryItemRepository : IRepository<InventoryItemDefinitionAggregate, Guid>
    {
        /// <summary>
        /// Finds an inventory item by its SKU
        /// </summary>
        /// <param name="sku">Stock keeping unit</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Inventory item or null if not found</returns>
        Task<InventoryItemDefinitionAggregate> FindBySkuAsync(
            string sku,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an inventory item with the specified SKU exists
        /// </summary>
        /// <param name="sku">Stock keeping unit</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsBySkuAsync(
            string sku,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of inventory items by category path
        /// </summary>
        /// <param name="categoryPath">Category path</param>
        /// <param name="includeChildCategories">Whether to include items in child categories</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items</returns>
        Task<List<InventoryItemDefinitionAggregate>> GetListByCategoryPathAsync(
            string categoryPath,
            bool includeChildCategories = false,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of active inventory items
        /// </summary>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active inventory items</returns>
        Task<List<InventoryItemDefinitionAggregate>> GetActiveListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of inventory items that are below their minimum stock level
        /// </summary>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items below minimum stock level</returns>
        Task<List<InventoryItemDefinitionAggregate>> GetItemsBelowMinimumStockLevelAsync(
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of inventory items at a specific location that are below minimum stock level
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items below minimum stock level at the specified location</returns>
        Task<List<InventoryItemDefinitionAggregate>> GetItemsBelowMinimumStockLevelAtLocationAsync(
            Guid locationId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of inventory items at a specific location
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items at the specified location</returns>
        Task<List<InventoryItemDefinitionAggregate>> GetItemsAtLocationAsync(
            Guid locationId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of inventory items by barcode
        /// </summary>
        /// <param name="barcode">Barcode</param>
        /// <param name="includeDetails">Whether to include details (stock levels, transactions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of inventory items</returns>
        Task<List<InventoryItemDefinitionAggregate>> GetListByBarcodeAsync(
            string barcode,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
