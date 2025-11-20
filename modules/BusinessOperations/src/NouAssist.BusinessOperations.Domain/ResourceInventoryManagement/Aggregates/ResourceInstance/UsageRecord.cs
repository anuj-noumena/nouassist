using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance
{
    /// <summary>
    /// Represents a usage record for a resource instance
    /// </summary>
    public class UsageRecord : Entity<Guid>
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; private set; }

        /// <summary>
        /// Type of usage
        /// </summary>
        public string UsageType { get; private set; }

        /// <summary>
        /// Start time of the usage
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// End time of the usage
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Usage quantity (if applicable)
        /// </summary>
        public decimal? Quantity { get; private set; }

        /// <summary>
        /// Unit of measure for the quantity
        /// </summary>
        public string UnitOfMeasure { get; private set; }

        /// <summary>
        /// Related entity type (e.g., "Project", "Task")
        /// </summary>
        public string RelatedEntityType { get; private set; }

        /// <summary>
        /// ID of the related entity
        /// </summary>
        public Guid? RelatedEntityId { get; private set; }

        /// <summary>
        /// Usage notes
        /// </summary>
        public string Notes { get; private set; }

        protected UsageRecord()
        {
        }

        /// <summary>
        /// Creates a new usage record
        /// </summary>
        /// <param name="id">Usage record ID</param>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="usageType">Type of usage</param>
        /// <param name="startTime">Usage start time</param>
        /// <param name="endTime">Usage end time</param>
        /// <param name="quantity">Usage quantity (optional)</param>
        /// <param name="unitOfMeasure">Unit of measure (optional)</param>
        /// <param name="relatedEntityType">Related entity type (optional)</param>
        /// <param name="relatedEntityId">Related entity ID (optional)</param>
        /// <param name="notes">Usage notes (optional)</param>
        public UsageRecord(
            Guid id,
            Guid resourceInstanceId,
            string usageType,
            DateTime startTime,
            DateTime endTime,
            decimal? quantity = null,
            string unitOfMeasure = null,
            string relatedEntityType = null,
            Guid? relatedEntityId = null,
            string notes = null) : base(id)
        {
            ResourceInstanceId = resourceInstanceId;
            UsageType = usageType;
            StartTime = startTime;
            EndTime = endTime;
            Quantity = quantity;
            UnitOfMeasure = unitOfMeasure;
            RelatedEntityType = relatedEntityType;
            RelatedEntityId = relatedEntityId;
            Notes = notes;
        }

        /// <summary>
        /// Updates the usage record
        /// </summary>
        /// <param name="quantity">Updated quantity</param>
        /// <param name="notes">Updated notes</param>
        public void Update(decimal? quantity, string notes)
        {
            Quantity = quantity;
            Notes = notes;
        }

        /// <summary>
        /// Gets the duration of the usage
        /// </summary>
        /// <returns>Time span duration</returns>
        public TimeSpan GetDuration()
        {
            return EndTime - StartTime;
        }
    }
}
