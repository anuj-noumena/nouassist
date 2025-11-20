using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for resource booking information
    /// </summary>
    public class ResourceBookingDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the resource instance being booked
        /// </summary>
        public Guid ResourceInstanceId { get; set; }
        
        /// <summary>
        /// Name of the resource instance
        /// </summary>
        public string ResourceName { get; set; } = null!;

        /// <summary>
        /// ID of the user who booked the resource
        /// </summary>
        public Guid BookedByUserId { get; set; }
        
        /// <summary>
        /// Name of the user who booked the resource
        /// </summary>
        public string? BookedByUserName { get; set; }

        /// <summary>
        /// ID of the user who is assigned to use the resource (if different from BookedByUserId)
        /// </summary>
        public Guid? AssignedToUserId { get; set; }
        
        /// <summary>
        /// Name of the user who is assigned to use the resource
        /// </summary>
        public string? AssignedToUserName { get; set; }

        /// <summary>
        /// Code uniquely identifying this booking
        /// </summary>
        public string BookingCode { get; set; } = null!;

        /// <summary>
        /// Start time of the booking
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the booking
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Current status of the booking
        /// </summary>
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Purpose of the booking
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Related entity (project, task, etc.) the resource is booked for
        /// </summary>
        public string? RelatedEntity { get; set; }

        /// <summary>
        /// ID of the related entity
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Booking notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Reason for cancellation or rejection (if applicable)
        /// </summary>
        public string? CancellationReason { get; set; }

        /// <summary>
        /// Indicates if this is a recurring booking
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// ID of the parent booking (for recurring bookings)
        /// </summary>
        public Guid? RecurringBookingId { get; set; }

        /// <summary>
        /// Date/time when the resource was checked out (when Status became InUse)
        /// </summary>
        public DateTime? CheckedOutAt { get; set; }

        /// <summary>
        /// Date/time when the resource was checked in (when Status became Completed)
        /// </summary>
        public DateTime? CheckedInAt { get; set; }
    }
}
