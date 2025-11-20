using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for inventory consumption report
    /// </summary>
    public class InventoryConsumptionReportDto
    {
        /// <summary>
        /// Start date of the report period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the report period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Location ID filter (if any)
        /// </summary>
        public Guid? LocationId { get; set; }
        
        /// <summary>
        /// Location name (if filtered by location)
        /// </summary>
        public string? LocationName { get; set; }
        
        /// <summary>
        /// Category path filter (if any)
        /// </summary>
        public string? CategoryPath { get; set; }
        
        /// <summary>
        /// Total number of items in the report
        /// </summary>
        public int TotalItemCount { get; set; }
        
        /// <summary>
        /// Total consumption quantity across all items
        /// </summary>
        public decimal TotalConsumptionQuantity { get; set; }
        
        /// <summary>
        /// Average daily consumption across all items
        /// </summary>
        public decimal AverageDailyConsumption { get; set; }
        
        /// <summary>
        /// List of inventory item consumption details
        /// </summary>
        public List<ItemConsumptionDetailDto> Items { get; set; } = new();
        
        /// <summary>
        /// Daily consumption totals
        /// </summary>
        public List<DailyConsumptionDto> DailyConsumption { get; set; } = new();
    }
}
