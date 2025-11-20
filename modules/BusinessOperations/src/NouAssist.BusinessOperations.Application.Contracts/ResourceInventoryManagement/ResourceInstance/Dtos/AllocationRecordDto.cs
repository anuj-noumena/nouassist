using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for allocation record information
    /// </summary>
    public class AllocationRecordDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; set; }

        /// <summary>
        /// Entity the resource is allocated to (e.g., "Project", "Task")
        /// </summary>
        public string AllocatedToEntity { get; set; } = null!;

        /// <summary>
        /// ID of the entity the resource is allocated to
        /// </summary>
        public Guid AllocatedToEntityId { get; set; }

        /// <summary>
        /// Start time of the allocation
        /// </summary>
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
        public string? Notes { get; set; }

        /// <summary>
        /// Notes added when ending the allocation
        /// </summary>
        public string? EndNotes { get; set; }
    }
}
