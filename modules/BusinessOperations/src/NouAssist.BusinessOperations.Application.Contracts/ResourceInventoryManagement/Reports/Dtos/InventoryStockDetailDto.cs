using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for inventory stock detail
    /// </summary>
    public class InventoryStockDetailDto
    {
        /// <summary>
        /// Inventory item ID
        /// </summary>
        public Guid ItemId { get; set; }
        
        /// <summary>
        /// Stock keeping unit (SKU)
        /// </summary>
        public string Sku { get; set; } = null!;
        
        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Category path
        /// </summary>
        public string CategoryPath { get; set; } = null!;
        
        /// <summary>
        /// Total quantity across all locations
        /// </summary>
        public decimal TotalQuantity { get; set; }
        
        /// <summary>
        /// Minimum stock level
        /// </summary>
        public decimal MinimumStockLevel { get; set; }
        
        /// <summary>
        /// Target stock level
        /// </summary>
        public decimal TargetStockLevel { get; set; }
        
        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; } = null!;
        
        /// <summary>
        /// Whether this item is below minimum stock level
        /// </summary>
        public bool IsBelowMinimum { get; set; }
        
        /// <summary>
        /// Stock level by location
        /// </summary>
        public List<LocationStockDto> LocationStock { get; set; } = new();
    }
}
