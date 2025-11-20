using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Inventory;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Location;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for inventory management operations
    /// </summary>
    public class InventoryManager : DomainService
    {
        private readonly IInventoryItemRepository _inventoryRepository;
        private readonly IStorageLocationRepository _locationRepository;
        private readonly ILogger<InventoryManager> _logger;

        /// <summary>
        /// Creates a new instance of InventoryManager
        /// </summary>
        /// <param name="inventoryRepository">Inventory item repository</param>
        /// <param name="locationRepository">Storage location repository</param>
        /// <param name="logger">Logger</param>
        public InventoryManager(
            IInventoryItemRepository inventoryRepository,
            IStorageLocationRepository locationRepository,
            ILogger<InventoryManager> logger)
        {
            _inventoryRepository = inventoryRepository;
            _locationRepository = locationRepository;
            _logger = logger;
        }

        /// <summary>
        /// Adjusts the stock level of an inventory item at a location
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="adjustmentAmount">Adjustment amount (positive or negative)</param>
        /// <param name="transactionType">Transaction type</param>
        /// <param name="transactionReference">Transaction reference (optional)</param>
        /// <param name="notes">Transaction notes (optional)</param>
        /// <param name="operatorId">ID of the operator performing the adjustment (optional)</param>
        /// <returns>Updated stock level and created transaction</returns>
        public async Task<(StockLevel, InventoryTransaction)> AdjustStockLevelAsync(
            Guid inventoryItemId,
            Guid locationId,
            decimal adjustmentAmount,
            InventoryTransactionType transactionType,
            string transactionReference = null,
            string notes = null,
            Guid? operatorId = null)
        {
            // Validate inventory item exists
            var inventoryItem = await _inventoryRepository.GetAsync(inventoryItemId);
            if (inventoryItem == null)
            {
                throw new InventoryItemNotFoundException(inventoryItemId);
            }

            // Validate location exists
            var location = await _locationRepository.GetAsync(locationId);
            if (location == null)
            {
                throw new StorageLocationNotFoundException(locationId);
            }

            // Generate a new transaction ID
            var transactionId = GuidGenerator.Create();

            // Adjust the stock level
            var result = inventoryItem.AdjustStockLevel(
                transactionId,
                locationId,
                adjustmentAmount,
                transactionType,
                transactionReference,
                notes,
                operatorId);

            // Save changes
            await _inventoryRepository.UpdateAsync(inventoryItem);

            _logger.LogInformation(
                "Adjusted stock level for item {ItemId} at location {LocationId}: {AdjustmentAmount} {TransactionType}",
                inventoryItemId, locationId, adjustmentAmount, transactionType);

            return result;
        }

        /// <summary>
        /// Transfers stock of an inventory item between locations
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="sourceLocationId">Source location ID</param>
        /// <param name="destinationLocationId">Destination location ID</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="transactionReference">Transaction reference (optional)</param>
        /// <param name="notes">Transaction notes (optional)</param>
        /// <param name="operatorId">ID of the operator performing the transfer (optional)</param>
        /// <returns>List of updated stock levels and created transactions</returns>
        public async Task<List<(StockLevel, InventoryTransaction)>> TransferStockAsync(
            Guid inventoryItemId,
            Guid sourceLocationId,
            Guid destinationLocationId,
            decimal quantity,
            string transactionReference = null,
            string notes = null,
            Guid? operatorId = null)
        {
            if (quantity <= 0)
            {
                throw new RIMDomainException("RIM:Validation:QuantityMustBePositive");
            }

            // Validate inventory item exists
            var inventoryItem = await _inventoryRepository.GetAsync(inventoryItemId);
            if (inventoryItem == null)
            {
                throw new InventoryItemNotFoundException(inventoryItemId);
            }

            // Validate source location exists
            var sourceLocation = await _locationRepository.GetAsync(sourceLocationId);
            if (sourceLocation == null)
            {
                throw new StorageLocationNotFoundException(sourceLocationId);
            }

            // Validate destination location exists
            var destinationLocation = await _locationRepository.GetAsync(destinationLocationId);
            if (destinationLocation == null)
            {
                throw new StorageLocationNotFoundException(destinationLocationId);
            }

            // Check current stock at source location
            var sourceStockLevel = inventoryItem.GetStockLevel(sourceLocationId);
            if (sourceStockLevel == null || sourceStockLevel.Quantity < quantity)
            {
                throw new NegativeStockException(
                    inventoryItemId,
                    sourceLocationId,
                    sourceStockLevel?.Quantity ?? 0,
                    quantity);
            }

            // Generate a new transaction ID
            var transactionId = GuidGenerator.Create();

            // Transfer the stock
            var results = inventoryItem.TransferStock(
                transactionId,
                sourceLocationId,
                destinationLocationId,
                quantity,
                transactionReference,
                notes,
                operatorId);

            // Save changes
            await _inventoryRepository.UpdateAsync(inventoryItem);

            _logger.LogInformation(
                "Transferred {Quantity} of item {ItemId} from location {SourceLocationId} to {DestinationLocationId}",
                quantity, inventoryItemId, sourceLocationId, destinationLocationId);

            return results;
        }

        /// <summary>
        /// Gets inventory items that are below minimum stock level
        /// </summary>
        /// <returns>List of inventory items below minimum stock level</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetItemsBelowMinimumStockLevelAsync()
        {
            return await _inventoryRepository.GetItemsBelowMinimumStockLevelAsync();
        }

        /// <summary>
        /// Gets inventory items at a specific location that are below minimum stock level
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <returns>List of inventory items below minimum stock level at the specified location</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> GetItemsBelowMinimumStockLevelAtLocationAsync(Guid locationId)
        {
            return await _inventoryRepository.GetItemsBelowMinimumStockLevelAtLocationAsync(locationId);
        }
    }
}
