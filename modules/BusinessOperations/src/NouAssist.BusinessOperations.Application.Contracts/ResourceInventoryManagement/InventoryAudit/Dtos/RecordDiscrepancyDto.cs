using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for recording a discrepancy
    /// </summary>
    public class RecordDiscrepancyDto
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        [Required]
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Actual quantity found
        /// </summary>
        [Required]
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// Reason for discrepancy
        /// </summary>
        [StringLength(500)]
        public string? Reason { get; set; }

        /// <summary>
        /// Notes about the discrepancy
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
