using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for location stock information
    /// </summary>
    public class LocationStockDto
    {
        /// <summary>
        /// Location ID
        /// </summary>
        public Guid LocationId { get; set; }
        
        /// <summary>
        /// Location code
        /// </summary>
        public string LocationCode { get; set; } = null!;
        
        /// <summary>
        /// Location name
        /// </summary>
        public string LocationName { get; set; } = null!;
        
        /// <summary>
        /// Quantity at this location
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Last updated time
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
