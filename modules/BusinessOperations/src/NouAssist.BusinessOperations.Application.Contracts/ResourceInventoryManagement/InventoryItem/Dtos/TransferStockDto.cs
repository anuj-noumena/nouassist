using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for transferring stock between locations
    /// </summary>
    public class TransferStockDto
    {
        /// <summary>
        /// Source location ID
        /// </summary>
        [Required]
        public Guid SourceLocationId { get; set; }

        /// <summary>
        /// Destination location ID
        /// </summary>
        [Required]
        public Guid DestinationLocationId { get; set; }

        /// <summary>
        /// Quantity to transfer
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

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
