using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for inventory item information
    /// </summary>
    public class InventoryItemDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Stock keeping unit (SKU) uniquely identifying this item
        /// </summary>
        public string Sku { get; set; } = null!;

        /// <summary>
        /// Name of the inventory item
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the inventory item
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Path in the category hierarchy
        /// </summary>
        public string CategoryPath { get; set; } = null!;

        /// <summary>
        /// Barcode for the inventory item (optional)
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// Unit of measure used for this item
        /// </summary>
        public string UnitOfMeasure { get; set; } = null!;

        /// <summary>
        /// Minimum stock level before alerts are triggered
        /// </summary>
        public decimal MinimumStockLevel { get; set; }

        /// <summary>
        /// Target stock level for optimal inventory
        /// </summary>
        public decimal TargetStockLevel { get; set; }

        /// <summary>
        /// Indicates if this inventory item is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// Total quantity across all locations
        /// </summary>
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Stock levels by location
        /// </summary>
        public List<StockLevelDto> StockLevels { get; set; } = new();
    }
}
