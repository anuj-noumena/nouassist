using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for item consumption detail
    /// </summary>
    public class ItemConsumptionDetailDto
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
        /// Total consumption quantity
        /// </summary>
        public decimal TotalConsumption { get; set; }
        
        /// <summary>
        /// Average daily consumption
        /// </summary>
        public decimal AverageDailyConsumption { get; set; }
        
        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; } = null!;
        
        /// <summary>
        /// Estimated days until stock out based on current level and consumption rate
        /// </summary>
        public int? EstimatedDaysUntilStockout { get; set; }
        
        /// <summary>
        /// Consumption trend (increasing, decreasing, stable)
        /// </summary>
        public string ConsumptionTrend { get; set; } = null!;
        
        /// <summary>
        /// Daily consumption data
        /// </summary>
        public List<DailyItemConsumptionDto> DailyConsumption { get; set; } = new();
    }
}
