using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for daily consumption totals
    /// </summary>
    public class DailyConsumptionDto
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Total consumption quantity for all items
        /// </summary>
        public decimal TotalConsumption { get; set; }
        
        /// <summary>
        /// Number of distinct items consumed
        /// </summary>
        public int ItemCount { get; set; }
    }
}
