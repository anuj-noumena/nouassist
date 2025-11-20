using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for item discrepancy summary
    /// </summary>
    public class ItemDiscrepancySummaryDto
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
        /// Total number of discrepancies for this item
        /// </summary>
        public int DiscrepancyCount { get; set; }
        
        /// <summary>
        /// Average quantity difference (can be positive or negative)
        /// </summary>
        public decimal AverageQuantityDifference { get; set; }
        
        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; } = null!;
    }
}
