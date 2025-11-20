using System;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Booking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking
{
    /// <summary>
    /// Represents a booking/reservation for a resource instance.
    /// </summary>
    public class ResourceBookingAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID, if this booking belongs to a specific tenant
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// ID of the resource instance being booked
        /// </summary>
        public Guid ResourceInstanceId { get; private set; }

        /// <summary>
        /// ID of the user who booked the resource
        /// </summary>
        public Guid BookedByUserId { get; private set; }

        /// <summary>
        /// ID of the user who is assigned to use the resource (if different from BookedByUserId)
        /// </summary>
        public Guid? AssignedToUserId { get; private set; }

        /// <summary>
        /// Code uniquely identifying this booking
        /// </summary>
        public string BookingCode { get; private set; }

        /// <summary>
        /// Time slot for the booking
        /// </summary>
        public BookingTimeSlot TimeSlot { get; private set; }

        /// <summary>
        /// Current status of the booking
        /// </summary>
        public BookingStatus Status { get; private set; }

        /// <summary>
        /// Purpose of the booking
        /// </summary>
        public string Purpose { get; private set; }

        /// <summary>
        /// Related entity (project, task, etc.) the resource is booked for
        /// </summary>
        public string RelatedEntity { get; private set; }

        /// <summary>
        /// ID of the related entity
        /// </summary>
        public Guid? RelatedEntityId { get; private set; }

        /// <summary>
        /// Booking notes
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// Reason for cancellation or rejection (if applicable)
        /// </summary>
        public string CancellationReason { get; private set; }

        /// <summary>
        /// Indicates if this is a recurring booking
        /// </summary>
        public bool IsRecurring { get; private set; }

        /// <summary>
        /// ID of the parent booking (for recurring bookings)
        /// </summary>
        public Guid? RecurringBookingId { get; private set; }

        /// <summary>
        /// Date/time when the resource was checked out (when Status became InUse)
        /// </summary>
        public DateTime? CheckedOutAt { get; private set; }

        /// <summary>
        /// Date/time when the resource was checked in (when Status became Completed)
        /// </summary>
        public DateTime? CheckedInAt { get; private set; }

        protected ResourceBookingAggregate()
        {
        }

        /// <summary>
        /// Creates a new resource booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="bookedByUserId">ID of the user booking the resource</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="bookingCode">Booking code (optional, will be generated if not provided)</param>
        /// <param name="assignedToUserId">ID of the user assigned to use the resource (optional)</param>
        /// <param name="purpose">Purpose of the booking (optional)</param>
        /// <param name="relatedEntity">Related entity (optional)</param>
        /// <param name="relatedEntityId">ID of the related entity (optional)</param>
        /// <param name="notes">Booking notes (optional)</param>
        /// <param name="isRecurring">Whether this is a recurring booking (optional)</param>
        /// <param name="recurringBookingId">ID of the parent booking for recurring bookings (optional)</param>
        /// <param name="tenantId">Tenant ID (optional)</param>
        public ResourceBookingAggregate(
            Guid id,
            Guid resourceInstanceId,
            Guid bookedByUserId,
            DateTime startTime,
            DateTime endTime,
            string bookingCode = null,
            Guid? assignedToUserId = null,
            string purpose = null,
            string relatedEntity = null,
            Guid? relatedEntityId = null,
            string notes = null,
            bool isRecurring = false,
            Guid? recurringBookingId = null,
            Guid? tenantId = null) : base(id)
        {
            ResourceInstanceId = resourceInstanceId;
            BookedByUserId = bookedByUserId;
            AssignedToUserId = assignedToUserId;
            TimeSlot = new BookingTimeSlot(startTime, endTime);
            SetBookingCode(bookingCode ?? GenerateBookingCode());
            SetPurpose(purpose);
            RelatedEntity = relatedEntity;
            RelatedEntityId = relatedEntityId;
            Notes = notes;
            Status = BookingStatus.Pending;
            IsRecurring = isRecurring;
            RecurringBookingId = recurringBookingId;
            TenantId = tenantId;

            AddDistributedEvent(new ResourceBookedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                BookedByUserId = BookedByUserId,
                StartTime = TimeSlot.StartTime,
                EndTime = TimeSlot.EndTime,
                Status = Status.ToString(),
                Purpose = Purpose,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the booking information
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="assignedToUserId">ID of the user assigned to use the resource (optional)</param>
        /// <param name="purpose">Purpose of the booking (optional)</param>
        /// <param name="notes">Booking notes (optional)</param>
        public void Update(
            DateTime startTime,
            DateTime endTime,
            Guid? assignedToUserId = null,
            string purpose = null,
            string notes = null)
        {
            // Can only update if not in use, completed, or cancelled
            if (Status == BookingStatus.InUse || Status == BookingStatus.Completed || Status == BookingStatus.Cancelled)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot update booking with status {Status}");
            }

            TimeSlot = new BookingTimeSlot(startTime, endTime);
            AssignedToUserId = assignedToUserId;
            SetPurpose(purpose);
            Notes = notes;

            AddDistributedEvent(new ResourceBookingUpdatedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                StartTime = TimeSlot.StartTime,
                EndTime = TimeSlot.EndTime,
                Status = Status.ToString(),
                Purpose = Purpose,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Confirms the booking
        /// </summary>
        public void Confirm()
        {
            if (Status != BookingStatus.Pending)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot confirm booking with status {Status}");
            }

            Status = BookingStatus.Confirmed;

            AddDistributedEvent(new ResourceBookingStatusChangedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                Status = Status.ToString(),
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Rejects the booking
        /// </summary>
        /// <param name="reason">Rejection reason</param>
        public void Reject(string reason)
        {
            if (Status != BookingStatus.Pending)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot reject booking with status {Status}");
            }

            Status = BookingStatus.Rejected;
            CancellationReason = reason;

            AddDistributedEvent(new ResourceBookingStatusChangedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                Status = Status.ToString(),
                Reason = reason,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Cancels the booking
        /// </summary>
        /// <param name="reason">Cancellation reason</param>
        public void Cancel(string reason)
        {
            if (Status == BookingStatus.Completed || Status == BookingStatus.Cancelled || Status == BookingStatus.Rejected)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot cancel booking with status {Status}");
            }

            Status = BookingStatus.Cancelled;
            CancellationReason = reason;

            AddDistributedEvent(new ResourceBookingStatusChangedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                Status = Status.ToString(),
                Reason = reason,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the resource as checked out (in use)
        /// </summary>
        /// <param name="checkedOutAt">Check-out time (defaults to now)</param>
        public void CheckOut(DateTime? checkedOutAt = null)
        {
            if (Status != BookingStatus.Confirmed)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot check out booking with status {Status}");
            }

            Status = BookingStatus.InUse;
            CheckedOutAt = checkedOutAt ?? DateTime.Now;

            AddDistributedEvent(new ResourceBookingStatusChangedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                Status = Status.ToString(),
                CheckedOutAt = CheckedOutAt,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the resource as checked in (booking completed)
        /// </summary>
        /// <param name="checkedInAt">Check-in time (defaults to now)</param>
        /// <param name="notes">Additional notes about the usage</param>
        public void CheckIn(DateTime? checkedInAt = null, string notes = null)
        {
            if (Status != BookingStatus.InUse)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot check in booking with status {Status}");
            }

            Status = BookingStatus.Completed;
            CheckedInAt = checkedInAt ?? DateTime.Now;
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}\n\nCheck-in notes: {notes}";
            }

            AddDistributedEvent(new ResourceBookingStatusChangedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                Status = Status.ToString(),
                CheckedInAt = CheckedInAt,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the assigned user
        /// </summary>
        /// <param name="assignedToUserId">ID of the user assigned to use the resource</param>
        public void UpdateAssignedUser(Guid assignedToUserId)
        {
            if (Status == BookingStatus.Completed || Status == BookingStatus.Cancelled || Status == BookingStatus.Rejected)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot update assigned user for booking with status {Status}");
            }

            AssignedToUserId = assignedToUserId;
        }

        /// <summary>
        /// Checks if this booking overlaps with another booking time slot
        /// </summary>
        /// <param name="otherTimeSlot">Other booking time slot</param>
        /// <returns>True if the bookings overlap</returns>
        public bool OverlapsWith(BookingTimeSlot otherTimeSlot)
        {
            return TimeSlot.OverlapsWith(otherTimeSlot);
        }

        /// <summary>
        /// Checks if the booking is active at the specified time
        /// </summary>
        /// <param name="checkTime">Time to check</param>
        /// <returns>True if the booking is active at the specified time</returns>
        public bool IsActiveAt(DateTime checkTime)
        {
            return Status == BookingStatus.Confirmed || Status == BookingStatus.InUse
                && checkTime >= TimeSlot.StartTime && checkTime <= TimeSlot.EndTime;
        }

        /// <summary>
        /// Extends the booking end time
        /// </summary>
        /// <param name="newEndTime">New end time</param>
        public void Extend(DateTime newEndTime)
        {
            if (Status != BookingStatus.Confirmed && Status != BookingStatus.InUse)
            {
                throw new ResourceBookingException(ResourceInstanceId, 
                    $"Cannot extend booking with status {Status}");
            }

            if (newEndTime <= TimeSlot.EndTime)
            {
                throw new RIMDomainException("RIM:ErrorMessage:NewEndTimeNotLater");
            }

            TimeSlot = new BookingTimeSlot(TimeSlot.StartTime, newEndTime);

            AddDistributedEvent(new ResourceBookingExtendedEto
            {
                Id = Id,
                ResourceInstanceId = ResourceInstanceId,
                NewEndTime = newEndTime,
                TenantId = TenantId
            });
        }

        private void SetBookingCode(string bookingCode)
        {
            BookingCode = Check.NotNullOrWhiteSpace(
                bookingCode,
                nameof(bookingCode),
                maxLength: Constants.RIMConstants.ResourceBookings.MaxBookingCodeLength
            );
        }

        private void SetPurpose(string purpose)
        {
            Purpose = Check.Length(
                purpose,
                nameof(purpose),
                maxLength: Constants.RIMConstants.ResourceBookings.MaxPurposeLength
            );
        }

        private string GenerateBookingCode()
        {
            // Generate a unique booking code using time-based elements
            var timestamp = DateTime.Now.ToString("yyMMddHHmm");
            var randomPart = new Random().Next(1000, 9999).ToString();
            return $"BK-{timestamp}-{randomPart}";
        }
    }
}
