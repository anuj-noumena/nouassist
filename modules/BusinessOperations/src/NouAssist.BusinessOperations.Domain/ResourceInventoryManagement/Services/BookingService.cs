using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Booking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for resource booking operations
    /// </summary>
    public class BookingService : DomainService
    {
        private readonly IResourceBookingRepository _bookingRepository;
        private readonly IResourceInstanceRepository _resourceRepository;
        private readonly ILogger<BookingService> _logger;

        /// <summary>
        /// Creates a new instance of BookingService
        /// </summary>
        /// <param name="bookingRepository">Resource booking repository</param>
        /// <param name="resourceRepository">Resource instance repository</param>
        /// <param name="logger">Logger</param>
        public BookingService(
            IResourceBookingRepository bookingRepository,
            IResourceInstanceRepository resourceRepository,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _resourceRepository = resourceRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new resource booking
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="bookedByUserId">ID of the user booking the resource</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="assignedToUserId">ID of the user assigned to use the resource (optional)</param>
        /// <param name="purpose">Purpose of the booking (optional)</param>
        /// <param name="relatedEntity">Related entity (optional)</param>
        /// <param name="relatedEntityId">ID of the related entity (optional)</param>
        /// <param name="notes">Booking notes (optional)</param>
        /// <returns>The created booking</returns>
        public async Task<ResourceBookingAggregate> BookResourceAsync(
            Guid resourceInstanceId,
            Guid bookedByUserId,
            DateTime startTime,
            DateTime endTime,
            Guid? assignedToUserId = null,
            string purpose = null,
            string relatedEntity = null,
            Guid? relatedEntityId = null,
            string notes = null)
        {
            // Validate resource exists
            var resource = await _resourceRepository.GetAsync(resourceInstanceId);
            if (resource == null)
            {
                throw new ResourceInstanceNotFoundException(resourceInstanceId);
            }

            // Check if resource is available (not retired or unavailable)
            if (resource.Status == ResourceStatus.Retired)
            {
                throw new ResourceBookingException(resourceInstanceId, "Cannot book a retired resource");
            }

            // Check for booking conflicts
            var timeSlot = new BookingTimeSlot(startTime, endTime);
            var existingBookings = await _bookingRepository.GetActiveBookingsForResourceAsync(
                resourceInstanceId,
                startTime,
                endTime);

            if (existingBookings.Count > 0)
            {
                throw new ResourceBookingException(resourceInstanceId, "Resource has overlapping bookings for the requested time period");
            }

            // Generate a new booking ID
            var bookingId = GuidGenerator.Create();

            // Create the booking
            var booking = new ResourceBookingAggregate(
                bookingId,
                resourceInstanceId,
                bookedByUserId,
                startTime,
                endTime,
                null, // Let the system generate a booking code
                assignedToUserId,
                purpose,
                relatedEntity,
                relatedEntityId,
                notes);

            // Save the booking
            await _bookingRepository.InsertAsync(booking);

            _logger.LogInformation(
                "Resource {ResourceId} booked from {StartTime} to {EndTime} by user {UserId}",
                resourceInstanceId, startTime, endTime, bookedByUserId);

            return booking;
        }

        /// <summary>
        /// Confirms a pending booking
        /// </summary>
        /// <param name="bookingId">Booking ID</param>
        /// <returns>The updated booking</returns>
        public async Task<ResourceBookingAggregate> ConfirmBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found", nameof(bookingId));
            }

            booking.Confirm();
            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation(
                "Booking {BookingId} for resource {ResourceId} confirmed",
                bookingId, booking.ResourceInstanceId);

            return booking;
        }

        /// <summary>
        /// Rejects a pending booking
        /// </summary>
        /// <param name="bookingId">Booking ID</param>
        /// <param name="reason">Rejection reason</param>
        /// <returns>The updated booking</returns>
        public async Task<ResourceBookingAggregate> RejectBookingAsync(Guid bookingId, string reason)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found", nameof(bookingId));
            }

            booking.Reject(reason);
            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation(
                "Booking {BookingId} for resource {ResourceId} rejected: {Reason}",
                bookingId, booking.ResourceInstanceId, reason);

            return booking;
        }

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="bookingId">Booking ID</param>
        /// <param name="reason">Cancellation reason</param>
        /// <returns>The updated booking</returns>
        public async Task<ResourceBookingAggregate> CancelBookingAsync(Guid bookingId, string reason)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found", nameof(bookingId));
            }

            booking.Cancel(reason);
            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation(
                "Booking {BookingId} for resource {ResourceId} cancelled: {Reason}",
                bookingId, booking.ResourceInstanceId, reason);

            return booking;
        }

        /// <summary>
        /// Marks a resource as checked out (in use)
        /// </summary>
        /// <param name="bookingId">Booking ID</param>
        /// <param name="checkedOutAt">Check-out time (defaults to now)</param>
        /// <returns>The updated booking</returns>
        public async Task<ResourceBookingAggregate> CheckOutResourceAsync(Guid bookingId, DateTime? checkedOutAt = null)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found", nameof(bookingId));
            }

            booking.CheckOut(checkedOutAt);
            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation(
                "Resource {ResourceId} checked out for booking {BookingId}",
                booking.ResourceInstanceId, bookingId);

            return booking;
        }

        /// <summary>
        /// Marks a resource as checked in (booking completed)
        /// </summary>
        /// <param name="bookingId">Booking ID</param>
        /// <param name="checkedInAt">Check-in time (defaults to now)</param>
        /// <param name="notes">Additional notes about the usage</param>
        /// <returns>The updated booking</returns>
        public async Task<ResourceBookingAggregate> CheckInResourceAsync(
            Guid bookingId, 
            DateTime? checkedInAt = null, 
            string notes = null)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found", nameof(bookingId));
            }

            booking.CheckIn(checkedInAt, notes);
            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation(
                "Resource {ResourceId} checked in for booking {BookingId}",
                booking.ResourceInstanceId, bookingId);

            return booking;
        }

        /// <summary>
        /// Extends a booking's end time
        /// </summary>
        /// <param name="bookingId">Booking ID</param>
        /// <param name="newEndTime">New end time</param>
        /// <returns>The updated booking</returns>
        public async Task<ResourceBookingAggregate> ExtendBookingAsync(Guid bookingId, DateTime newEndTime)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException($"Booking with ID {bookingId} not found", nameof(bookingId));
            }

            // Check for conflicts with the extended time
            var existingBookings = await _bookingRepository.GetActiveBookingsForResourceAsync(
                booking.ResourceInstanceId,
                booking.TimeSlot.EndTime,
                newEndTime);

            // Filter out the current booking
            existingBookings.RemoveAll(b => b.Id == bookingId);

            if (existingBookings.Count > 0)
            {
                throw new ResourceBookingException(booking.ResourceInstanceId, 
                    "Cannot extend booking due to conflicts with other bookings");
            }

            booking.Extend(newEndTime);
            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation(
                "Booking {BookingId} for resource {ResourceId} extended to {NewEndTime}",
                bookingId, booking.ResourceInstanceId, newEndTime);

            return booking;
        }

        /// <summary>
        /// Gets active bookings for a resource within a time period
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <returns>List of active bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetActiveBookingsForResourceAsync(
            Guid resourceInstanceId,
            DateTime startTime,
            DateTime endTime)
        {
            return await _bookingRepository.GetActiveBookingsForResourceAsync(
                resourceInstanceId,
                startTime,
                endTime);
        }

        /// <summary>
        /// Checks if a resource is available for booking during a specific time period
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <returns>True if the resource is available</returns>
        public async Task<bool> IsResourceAvailableForBookingAsync(
            Guid resourceInstanceId,
            DateTime startTime,
            DateTime endTime)
        {
            // Check if resource exists and is not retired or unavailable
            var resource = await _resourceRepository.GetAsync(resourceInstanceId);
            if (resource == null)
            {
                throw new ResourceInstanceNotFoundException(resourceInstanceId);
            }

            if (resource.Status == ResourceStatus.Retired || resource.Status == ResourceStatus.Unavailable)
            {
                return false;
            }

            // Check for conflicts with existing bookings
            var existingBookings = await _bookingRepository.GetActiveBookingsForResourceAsync(
                resourceInstanceId,
                startTime,
                endTime);

            return existingBookings.Count == 0;
        }
    }
}
