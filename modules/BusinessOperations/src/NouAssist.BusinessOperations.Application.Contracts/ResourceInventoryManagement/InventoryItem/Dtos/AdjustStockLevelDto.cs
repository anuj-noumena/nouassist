using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for adjusting stock level
    /// </summary>
    public class AdjustStockLevelDto
    {
        /// <summary>
        /// ID of the storage location
        /// </summary>
        [Required]
        public Guid LocationId { get; set; }

        /// <summary>
        /// Adjustment amount (positive or negative)
        /// </summary>
        [Required]
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// Transaction type
        /// </summary>
        [Required]
        public InventoryTransactionType TransactionType { get; set; }

        /// <summary>
        /// Transaction reference (optional)
        /// </summary>
        [StringLength(100)]
        public string? TransactionReference { get; set; }

        /// <summary>
        /// Transaction notes (optional)
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
