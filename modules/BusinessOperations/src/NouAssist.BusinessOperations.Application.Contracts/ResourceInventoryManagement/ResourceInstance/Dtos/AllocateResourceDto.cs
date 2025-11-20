using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for allocating a resource
    /// </summary>
    public class AllocateResourceDto
    {
        /// <summary>
        /// Entity the resource is allocated to (e.g., "Project", "Task")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string AllocatedToEntity { get; set; } = null!;

        /// <summary>
        /// ID of the entity the resource is allocated to
        /// </summary>
        [Required]
        public Guid AllocatedToEntityId { get; set; }

        /// <summary>
        /// Start time of the allocation
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the allocation (null if open-ended)
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Allocation percentage (for partial allocations)
        /// </summary>
        public decimal? AllocationPercentage { get; set; }

        /// <summary>
        /// Allocation notes
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
