using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for inventory stock report
    /// </summary>
    public class InventoryStockReportDto
    {
        /// <summary>
        /// Report generation date
        /// </summary>
        public DateTime ReportDate { get; set; }
        
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
        /// Number of items below minimum stock level
        /// </summary>
        public int ItemsBelowMinimumCount { get; set; }
        
        /// <summary>
        /// Total inventory value (if costs are available)
        /// </summary>
        public decimal? TotalInventoryValue { get; set; }
        
        /// <summary>
        /// List of inventory item stock details
        /// </summary>
        public List<InventoryStockDetailDto> Items { get; set; } = new();
    }
}
