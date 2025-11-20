using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for updating an inventory item
    /// </summary>
    public class UpdateInventoryItemDto
    {
        /// <summary>
        /// Name of the inventory item
        /// </summary>
        [Required]
        [StringLength(RIMConstants.InventoryItems.MaxNameLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the inventory item
        /// </summary>
        [StringLength(RIMConstants.InventoryItems.MaxDescriptionLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Path in the category hierarchy
        /// </summary>
        public string? CategoryPath { get; set; }

        /// <summary>
        /// Barcode for the inventory item (optional)
        /// </summary>
        [StringLength(RIMConstants.InventoryItems.MaxBarcodeLength)]
        public string? Barcode { get; set; }

        /// <summary>
        /// Unit of measure used for this item
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UnitOfMeasure { get; set; } = "ea";

        /// <summary>
        /// Minimum stock level before alerts are triggered
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal MinimumStockLevel { get; set; }

        /// <summary>
        /// Target stock level for optimal inventory
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal TargetStockLevel { get; set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        [StringLength(500)]
        public string? Tags { get; set; }
    }
}
