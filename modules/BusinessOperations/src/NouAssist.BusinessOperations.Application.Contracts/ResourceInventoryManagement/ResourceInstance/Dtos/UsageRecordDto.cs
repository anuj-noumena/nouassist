using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for usage record information
    /// </summary>
    public class UsageRecordDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; set; }

        /// <summary>
        /// Type of usage
        /// </summary>
        public string UsageType { get; set; } = null!;

        /// <summary>
        /// Start time of the usage
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the usage
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Usage quantity (if applicable)
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Unit of measure for the quantity
        /// </summary>
        public string? UnitOfMeasure { get; set; }

        /// <summary>
        /// Related entity type (e.g., "Project", "Task")
        /// </summary>
        public string? RelatedEntityType { get; set; }

        /// <summary>
        /// ID of the related entity
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Usage notes
        /// </summary>
        public string? Notes { get; set; }
    }
}
