using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for logging resource usage
    /// </summary>
    public class LogResourceUsageDto
    {
        /// <summary>
        /// Type of usage
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UsageType { get; set; } = null!;

        /// <summary>
        /// Start time of the usage
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the usage
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Usage quantity (if applicable)
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Unit of measure for the quantity
        /// </summary>
        [StringLength(50)]
        public string? UnitOfMeasure { get; set; }

        /// <summary>
        /// Related entity type (e.g., "Project", "Task")
        /// </summary>
        [StringLength(100)]
        public string? RelatedEntityType { get; set; }

        /// <summary>
        /// ID of the related entity
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Usage notes
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
