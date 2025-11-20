using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem
{
    /// <summary>
    /// Application service interface for managing inventory items
    /// </summary>
    public interface IInventoryItemAppService : IApplicationService
    {
        /// <summary>
        /// Gets an inventory item by ID
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <returns>Inventory item DTO</returns>
        Task<InventoryItemDto> GetAsync(Guid id);

        /// <summary>
        /// Gets an inventory item by SKU
        /// </summary>
        /// <param name="sku">Stock keeping unit</param>
        /// <returns>Inventory item DTO</returns>
        Task<InventoryItemDto> GetBySkuAsync(string sku);

        /// <summary>
        /// Gets a paged and filtered list of inventory items
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged result of inventory item DTOs</returns>
        Task<PagedResultDto<InventoryItemDto>> GetListAsync(GetInventoryItemListDto input);

        /// <summary>
        /// Creates a new inventory item
        /// </summary>
        /// <param name="input">Inventory item creation data</param>
        /// <returns>Created inventory item DTO</returns>
        Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto input);

        /// <summary>
        /// Updates an existing inventory item
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="input">Inventory item update data</param>
        /// <returns>Updated inventory item DTO</returns>
        Task<InventoryItemDto> UpdateAsync(Guid id, UpdateInventoryItemDto input);

        /// <summary>
        /// Deletes an inventory item
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Activates an inventory item
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <returns>Updated inventory item DTO</returns>
        Task<InventoryItemDto> ActivateAsync(Guid id);

        /// <summary>
        /// Deactivates an inventory item
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <returns>Updated inventory item DTO</returns>
        Task<InventoryItemDto> DeactivateAsync(Guid id);

        /// <summary>
        /// Adjusts the stock level of an inventory item at a location
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="input">Stock level adjustment data</param>
        /// <returns>Updated inventory item DTO</returns>
        Task<InventoryItemDto> AdjustStockLevelAsync(Guid id, AdjustStockLevelDto input);

        /// <summary>
        /// Transfers stock of an inventory item between locations
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="input">Stock transfer data</param>
        /// <returns>Updated inventory item DTO</returns>
        Task<InventoryItemDto> TransferStockAsync(Guid id, TransferStockDto input);

        /// <summary>
        /// Gets the transaction history for an inventory item
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="maxResultCount">Maximum number of results</param>
        /// <param name="skipCount">Number of results to skip</param>
        /// <returns>Paged list of inventory transactions</returns>
        Task<PagedResultDto<InventoryTransactionDto>> GetTransactionHistoryAsync(
            Guid id, 
            int maxResultCount = 10, 
            int skipCount = 0);

        /// <summary>
        /// Gets the transaction history for an inventory item at a specific location
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="locationId">Location ID</param>
        /// <param name="maxResultCount">Maximum number of results</param>
        /// <param name="skipCount">Number of results to skip</param>
        /// <returns>Paged list of inventory transactions</returns>
        Task<PagedResultDto<InventoryTransactionDto>> GetLocationTransactionHistoryAsync(
            Guid id, 
            Guid locationId, 
            int maxResultCount = 10, 
            int skipCount = 0);

        /// <summary>
        /// Gets a list of inventory items that are below minimum stock level
        /// </summary>
        /// <returns>List of inventory items</returns>
        Task<ListResultDto<InventoryItemDto>> GetItemsBelowMinimumStockLevelAsync();

        /// <summary>
        /// Gets a list of inventory items that are below minimum stock level at a specific location
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <returns>List of inventory items</returns>
        Task<ListResultDto<InventoryItemDto>> GetItemsBelowMinimumStockLevelAtLocationAsync(Guid locationId);

        /// <summary>
        /// Generates a barcode for an inventory item
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="barcodeFormat">Barcode format (1=CODE128, 2=EAN13, 3=UPC, 4=QR)</param>
        /// <returns>Generated barcode value</returns>
        Task<string> GenerateBarcodeAsync(Guid id, int barcodeFormat = 1);
    }
}
