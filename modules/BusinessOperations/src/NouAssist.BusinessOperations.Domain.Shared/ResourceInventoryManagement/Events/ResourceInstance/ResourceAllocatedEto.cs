using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceInstance
{
    /// <summary>
    /// Event raised when a resource is allocated
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceAllocated")]
    public class ResourceAllocatedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; set; }

        /// <summary>
        /// ID of the allocation record
        /// </summary>
        public Guid AllocationId { get; set; }

        /// <summary>
        /// Entity (project, task, etc.) the resource is allocated to
        /// </summary>
        public string AllocatedToEntity { get; set; }

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
    }
}
