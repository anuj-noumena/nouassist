using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for daily item consumption
    /// </summary>
    public class DailyItemConsumptionDto
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Consumption quantity
        /// </summary>
        public decimal Consumption { get; set; }
    }
}
