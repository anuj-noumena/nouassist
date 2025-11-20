using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Inventory;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem
{
    /// <summary>
    /// Represents an inventory item definition in the system.
    /// Inventory items are stockable products or materials that can be tracked in inventory.
    /// </summary>
    public class InventoryItemDefinitionAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID, if this inventory item belongs to a specific tenant
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// Stock keeping unit (SKU) uniquely identifying this item
        /// </summary>
        public StockKeepingUnit Sku { get; private set; }

        /// <summary>
        /// Name of the inventory item
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the inventory item
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Path in the category hierarchy
        /// </summary>
        public CategoryPath CategoryPath { get; private set; }

        /// <summary>
        /// Barcode for the inventory item (optional)
        /// </summary>
        public string Barcode { get; private set; }

        /// <summary>
        /// Unit of measure used for this item
        /// </summary>
        public string UnitOfMeasure { get; private set; }

        /// <summary>
        /// Minimum stock level before alerts are triggered
        /// </summary>
        public decimal MinimumStockLevel { get; private set; }

        /// <summary>
        /// Target stock level for optimal inventory
        /// </summary>
        public decimal TargetStockLevel { get; private set; }

        /// <summary>
        /// Indicates if this inventory item is active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        public string Tags { get; private set; }

        /// <summary>
        /// Collection of stock levels by location
        /// </summary>
        private readonly List<StockLevel> _stockLevels;
        public IReadOnlyCollection<StockLevel> StockLevels => _stockLevels.AsReadOnly();

        /// <summary>
        /// Collection of inventory transactions
        /// </summary>
        private readonly List<InventoryTransaction> _transactions;
        public IReadOnlyCollection<InventoryTransaction> Transactions => _transactions.AsReadOnly();

        protected InventoryItemDefinitionAggregate()
        {
            _stockLevels = new List<StockLevel>();
            _transactions = new List<InventoryTransaction>();
        }

        /// <summary>
        /// Creates a new inventory item definition
        /// </summary>
        /// <param name="id">Inventory item ID</param>
        /// <param name="sku">Stock keeping unit</param>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="categoryPath">Category path</param>
        /// <param name="unitOfMeasure">Unit of measure</param>
        /// <param name="minimumStockLevel">Minimum stock level</param>
        /// <param name="targetStockLevel">Target stock level</param>
        /// <param name="barcode">Barcode (optional)</param>
        /// <param name="tags">Tags (optional)</param>
        /// <param name="tenantId">Tenant ID (optional)</param>
        public InventoryItemDefinitionAggregate(
            Guid id,
            string sku,
            string name,
            string description,
            CategoryPath categoryPath,
            string unitOfMeasure,
            decimal minimumStockLevel,
            decimal targetStockLevel,
            string barcode = null,
            string tags = null,
            Guid? tenantId = null) : base(id)
        {
            Sku = new StockKeepingUnit(sku);
            SetName(name);
            SetDescription(description);
            CategoryPath = categoryPath ?? new CategoryPath("Uncategorized");
            UnitOfMeasure = unitOfMeasure ?? "ea";
            SetMinimumStockLevel(minimumStockLevel);
            SetTargetStockLevel(targetStockLevel);
            Barcode = Check.Length(barcode, nameof(barcode), maxLength: Constants.RIMConstants.InventoryItems.MaxBarcodeLength);
            Tags = tags;
            IsActive = true;
            TenantId = tenantId;
            _stockLevels = new List<StockLevel>();
            _transactions = new List<InventoryTransaction>();

            AddDistributedEvent(new InventoryItemDefinedEto
            {
                Id = Id,
                Sku = Sku.Value,
                Name = Name,
                CategoryPath = CategoryPath.Path,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the inventory item information
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="categoryPath">Category path</param>
        /// <param name="unitOfMeasure">Unit of measure</param>
        /// <param name="minimumStockLevel">Minimum stock level</param>
        /// <param name="targetStockLevel">Target stock level</param>
        /// <param name="barcode">Barcode (optional)</param>
        /// <param name="tags">Tags (optional)</param>
        public void Update(
            string name,
            string description,
            CategoryPath categoryPath,
            string unitOfMeasure,
            decimal minimumStockLevel,
            decimal targetStockLevel,
            string barcode = null,
            string tags = null)
        {
            SetName(name);
            SetDescription(description);
            CategoryPath = categoryPath ?? CategoryPath;
            UnitOfMeasure = unitOfMeasure ?? UnitOfMeasure;
            SetMinimumStockLevel(minimumStockLevel);
            SetTargetStockLevel(targetStockLevel);
            Barcode = Check.Length(barcode, nameof(barcode), maxLength: Constants.RIMConstants.InventoryItems.MaxBarcodeLength);
            Tags = tags;

            AddDistributedEvent(new InventoryItemUpdatedEto
            {
                Id = Id,
                Sku = Sku.Value,
                Name = Name,
                CategoryPath = CategoryPath.Path,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Deactivates this inventory item
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
        }

        /// <summary>
        /// Activates this inventory item
        /// </summary>
        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
        }

        /// <summary>
        /// Gets the stock level for a specific location
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <returns>Stock level, or null if not found</returns>
        public StockLevel GetStockLevel(Guid locationId)
        {
            return _stockLevels.FirstOrDefault(sl => sl.LocationId == locationId);
        }

        /// <summary>
        /// Gets the total stock level across all locations
        /// </summary>
        /// <returns>Total quantity</returns>
        public decimal GetTotalStockLevel()
        {
            return _stockLevels.Sum(sl => sl.Quantity);
        }

        /// <summary>
        /// Adjusts the stock level at a specific location
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="locationId">Location ID</param>
        /// <param name="adjustmentAmount">Adjustment amount (positive or negative)</param>
        /// <param name="transactionType">Transaction type</param>
        /// <param name="transactionReference">Transaction reference (optional)</param>
        /// <param name="notes">Transaction notes (optional)</param>
        /// <param name="operatorId">ID of the operator performing the adjustment (optional)</param>
        /// <returns>Updated stock level and created transaction</returns>
        public (StockLevel, InventoryTransaction) AdjustStockLevel(
            Guid transactionId,
            Guid locationId,
            decimal adjustmentAmount,
            InventoryTransactionType transactionType,
            string transactionReference = null,
            string notes = null,
            Guid? operatorId = null)
        {
            // Find existing stock level or create new one
            var stockLevel = GetStockLevel(locationId);
            var previousQuantity = stockLevel?.Quantity ?? 0;
            var newQuantity = previousQuantity + adjustmentAmount;

            // Check for negative stock
            if (newQuantity < 0)
            {
                throw new NegativeStockException(Id, locationId, previousQuantity, Math.Abs(adjustmentAmount));
            }

            // Create or update stock level
            if (stockLevel == null)
            {
                stockLevel = new StockLevel(Id, locationId, newQuantity);
                _stockLevels.Add(stockLevel);
            }
            else
            {
                stockLevel.UpdateQuantity(newQuantity);
            }

            // Create transaction record
            var transaction = new InventoryTransaction(
                transactionId,
                Id,
                locationId,
                transactionType,
                previousQuantity,
                newQuantity,
                adjustmentAmount,
                UnitOfMeasure,
                transactionReference,
                notes,
                operatorId);

            _transactions.Add(transaction);

            // Publish event
            AddDistributedEvent(new StockLevelAdjustedEto
            {
                InventoryItemId = Id,
                LocationId = locationId,
                PreviousQuantity = previousQuantity,
                NewQuantity = newQuantity,
                AdjustmentAmount = adjustmentAmount,
                UnitOfMeasure = UnitOfMeasure,
                TransactionType = transactionType.ToString(),
                TransactionReference = transactionReference,
                TenantId = TenantId
            });

            return (stockLevel, transaction);
        }

        /// <summary>
        /// Transfer stock between locations
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="sourceLocationId">Source location ID</param>
        /// <param name="destinationLocationId">Destination location ID</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="transactionReference">Transaction reference (optional)</param>
        /// <param name="notes">Transaction notes (optional)</param>
        /// <param name="operatorId">ID of the operator performing the transfer (optional)</param>
        /// <returns>List of updated stock levels and created transactions</returns>
        public List<(StockLevel, InventoryTransaction)> TransferStock(
            Guid transactionId,
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

            var results = new List<(StockLevel, InventoryTransaction)>();

            // Decrease stock at source location
            var sourceTransaction = Guid.NewGuid();
            var sourceResult = AdjustStockLevel(
                sourceTransaction,
                sourceLocationId,
                -quantity,
                InventoryTransactionType.Transfer,
                transactionReference,
                $"Transfer to location {destinationLocationId}: {notes}",
                operatorId);
            
            results.Add(sourceResult);

            // Increase stock at destination location
            var destinationTransaction = Guid.NewGuid();
            var destinationResult = AdjustStockLevel(
                destinationTransaction,
                destinationLocationId,
                quantity,
                InventoryTransactionType.Transfer,
                transactionReference,
                $"Transfer from location {sourceLocationId}: {notes}",
                operatorId);
            
            results.Add(destinationResult);

            // Link the transactions as part of the same transfer
            sourceResult.Item2.SetRelatedTransactionId(destinationTransaction);
            destinationResult.Item2.SetRelatedTransactionId(sourceTransaction);

            return results;
        }

        /// <summary>
        /// Checks if the inventory item is below minimum stock level
        /// </summary>
        /// <returns>True if any location is below minimum stock level</returns>
        public bool IsBelowMinimumStockLevel()
        {
            return GetTotalStockLevel() < MinimumStockLevel;
        }

        /// <summary>
        /// Gets locations where stock is below minimum level
        /// </summary>
        /// <param name="locationMinimumLevels">Dictionary of location-specific minimum levels (optional)</param>
        /// <returns>List of location IDs where stock is below minimum</returns>
        public List<Guid> GetLocationsBelowMinimumLevel(Dictionary<Guid, decimal> locationMinimumLevels = null)
        {
            var belowMinimumLocations = new List<Guid>();

            foreach (var stockLevel in _stockLevels)
            {
                // Use location-specific minimum if provided, otherwise use the global minimum
                var minimumForLocation = locationMinimumLevels != null && 
                                         locationMinimumLevels.TryGetValue(stockLevel.LocationId, out var locMin) 
                    ? locMin 
                    : MinimumStockLevel;

                if (stockLevel.Quantity < minimumForLocation)
                {
                    belowMinimumLocations.Add(stockLevel.LocationId);
                }
            }

            return belowMinimumLocations;
        }

        /// <summary>
        /// Sets the SKU for this inventory item
        /// </summary>
        /// <param name="sku">New SKU</param>
        public void SetSku(string sku)
        {
            Sku = new StockKeepingUnit(sku);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: Constants.RIMConstants.InventoryItems.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: Constants.RIMConstants.InventoryItems.MaxDescriptionLength
            );
        }

        private void SetMinimumStockLevel(decimal minimumStockLevel)
        {
            if (minimumStockLevel < 0)
            {
                throw new RIMDomainException("RIM:Validation:MinimumStockLevelNotNegative");
            }
            
            MinimumStockLevel = minimumStockLevel;
        }

        private void SetTargetStockLevel(decimal targetStockLevel)
        {
            if (targetStockLevel < 0)
            {
                throw new RIMDomainException("RIM:Validation:TargetStockLevelNotNegative");
            }
            
            if (targetStockLevel < MinimumStockLevel)
            {
                throw new RIMDomainException("RIM:Validation:TargetStockLevelBelowMinimum");
            }
            
            TargetStockLevel = targetStockLevel;
        }
    }
}
