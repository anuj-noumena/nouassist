using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for stock level information
    /// </summary>
    public class StockLevelDto
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// ID of the storage location
        /// </summary>
        public Guid LocationId { get; set; }
        
        /// <summary>
        /// Name of the storage location
        /// </summary>
        public string LocationName { get; set; } = null!;
        
        /// <summary>
        /// Code of the storage location
        /// </summary>
        public string LocationCode { get; set; } = null!;

        /// <summary>
        /// Quantity on hand at this location
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Last update time of this stock level
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
