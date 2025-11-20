using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for discrepancy summary information
    /// </summary>
    public class DiscrepancySummaryDto
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        public Guid InventoryItemId { get; set; }
        
        /// <summary>
        /// SKU of the inventory item
        /// </summary>
        public string ItemSku { get; set; } = null!;
        
        /// <summary>
        /// Name of the inventory item
        /// </summary>
        public string ItemName { get; set; } = null!;
        
        /// <summary>
        /// Total expected quantity in the system
        /// </summary>
        public decimal TotalExpectedQuantity { get; set; }
        
        /// <summary>
        /// Total actual quantity found during the audit
        /// </summary>
        public decimal TotalActualQuantity { get; set; }
        
        /// <summary>
        /// Total difference between actual and expected quantities
        /// </summary>
        public decimal TotalQuantityDifference => TotalActualQuantity - TotalExpectedQuantity;
        
        /// <summary>
        /// Number of discrepancies for this item
        /// </summary>
        public int DiscrepancyCount { get; set; }
    }
}
