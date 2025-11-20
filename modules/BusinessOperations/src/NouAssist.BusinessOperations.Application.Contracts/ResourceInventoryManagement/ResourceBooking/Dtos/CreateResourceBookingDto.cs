using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for creating a new resource booking
    /// </summary>
    public class CreateResourceBookingDto
    {
        /// <summary>
        /// ID of the resource instance being booked
        /// </summary>
        [Required]
        public Guid ResourceInstanceId { get; set; }

        /// <summary>
        /// ID of the user who is assigned to use the resource (if different from current user)
        /// </summary>
        public Guid? AssignedToUserId { get; set; }

        /// <summary>
        /// Start time of the booking
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the booking
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Purpose of the booking
        /// </summary>
        [StringLength(RIMConstants.ResourceBookings.MaxPurposeLength)]
        public string? Purpose { get; set; }

        /// <summary>
        /// Related entity (project, task, etc.) the resource is booked for
        /// </summary>
        [StringLength(100)]
        public string? RelatedEntity { get; set; }

        /// <summary>
        /// ID of the related entity
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Booking notes
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
        
        /// <summary>
        /// Indicates if this is a recurring booking
        /// </summary>
        public bool IsRecurring { get; set; }
        
        /// <summary>
        /// ID of the parent booking (for recurring bookings)
        /// </summary>
        public Guid? RecurringBookingId { get; set; }
    }
}
