using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance
{
    /// <summary>
    /// Represents an allocation record for a resource instance
    /// </summary>
    public class AllocationRecord : Entity<Guid>
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; private set; }

        /// <summary>
        /// Entity the resource is allocated to (e.g., "Project", "Task")
        /// </summary>
        public string AllocatedToEntity { get; private set; }

        /// <summary>
        /// ID of the entity the resource is allocated to
        /// </summary>
        public Guid AllocatedToEntityId { get; private set; }

        /// <summary>
        /// Start time of the allocation
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// End time of the allocation (null if open-ended)
        /// </summary>
        public DateTime? EndTime { get; private set; }

        /// <summary>
        /// Allocation percentage (for partial allocations)
        /// </summary>
        public decimal? AllocationPercentage { get; private set; }

        /// <summary>
        /// Allocation notes
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// Notes added when ending the allocation
        /// </summary>
        public string EndNotes { get; private set; }

        protected AllocationRecord()
        {
        }

        /// <summary>
        /// Creates a new allocation record
        /// </summary>
        /// <param name="id">Allocation record ID</param>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="allocatedToEntity">Entity the resource is allocated to</param>
        /// <param name="allocatedToEntityId">ID of the entity the resource is allocated to</param>
        /// <param name="startTime">Allocation start time</param>
        /// <param name="endTime">Allocation end time (optional)</param>
        /// <param name="allocationPercentage">Allocation percentage (optional)</param>
        /// <param name="notes">Allocation notes (optional)</param>
        public AllocationRecord(
            Guid id,
            Guid resourceInstanceId,
            string allocatedToEntity,
            Guid allocatedToEntityId,
            DateTime startTime,
            DateTime? endTime = null,
            decimal? allocationPercentage = null,
            string notes = null) : base(id)
        {
            ResourceInstanceId = resourceInstanceId;
            SetAllocatedToEntity(allocatedToEntity);
            AllocatedToEntityId = allocatedToEntityId;
            StartTime = startTime;
            EndTime = endTime;
            AllocationPercentage = allocationPercentage;
            Notes = notes;
        }

        /// <summary>
        /// Ends the allocation
        /// </summary>
        /// <param name="endTime">End time</param>
        /// <param name="endNotes">End notes (optional)</param>
        public void End(DateTime endTime, string endNotes = null)
        {
            if (EndTime.HasValue)
            {
                throw new RIMDomainException("RIM:ErrorMessage:AllocationAlreadyEnded");
            }

            if (endTime < StartTime)
            {
                throw new RIMDomainException("RIM:ErrorMessage:InvalidEndTime");
            }

            EndTime = endTime;
            EndNotes = endNotes;
        }

        /// <summary>
        /// Updates the allocation percentage
        /// </summary>
        /// <param name="allocationPercentage">New allocation percentage</param>
        public void UpdateAllocationPercentage(decimal? allocationPercentage)
        {
            AllocationPercentage = allocationPercentage;
        }

        /// <summary>
        /// Extends the allocation end time
        /// </summary>
        /// <param name="newEndTime">New end time</param>
        public void Extend(DateTime newEndTime)
        {
            if (!EndTime.HasValue || newEndTime > EndTime.Value)
            {
                EndTime = newEndTime;
            }
            else
            {
                throw new RIMDomainException("RIM:ErrorMessage:NewEndTimeNotLater");
            }
        }

        /// <summary>
        /// Checks if the allocation is active at the specified time
        /// </summary>
        /// <param name="checkTime">Time to check</param>
        /// <returns>True if the allocation is active at the specified time</returns>
        public bool IsActiveAt(DateTime checkTime)
        {
            return checkTime >= StartTime && (!EndTime.HasValue || checkTime <= EndTime.Value);
        }

        private void SetAllocatedToEntity(string allocatedToEntity)
        {
            AllocatedToEntity = Check.NotNullOrWhiteSpace(
                allocatedToEntity,
                nameof(allocatedToEntity)
            );
        }
    }
}
