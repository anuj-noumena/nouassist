using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Repository interface for the ResourceBooking aggregate
    /// </summary>
    public interface IResourceBookingRepository : IRepository<ResourceBookingAggregate, Guid>
    {
        /// <summary>
        /// Finds a booking by its booking code
        /// </summary>
        /// <param name="bookingCode">Booking code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Booking or null if not found</returns>
        Task<ResourceBookingAggregate> FindByBookingCodeAsync(
            string bookingCode,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of bookings for a specific resource instance
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        Task<List<ResourceBookingAggregate>> GetListByResourceInstanceAsync(
            Guid resourceInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets active bookings for a resource instance during a specific time period
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active bookings</returns>
        Task<List<ResourceBookingAggregate>> GetActiveBookingsForResourceAsync(
            Guid resourceInstanceId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of bookings by user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        Task<List<ResourceBookingAggregate>> GetListByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of bookings by status
        /// </summary>
        /// <param name="status">Booking status</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        Task<List<ResourceBookingAggregate>> GetListByStatusAsync(
            BookingStatus status,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of bookings for a time period
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        Task<List<ResourceBookingAggregate>> GetListForTimeRangeAsync(
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of bookings for a related entity
        /// </summary>
        /// <param name="relatedEntity">Related entity type</param>
        /// <param name="relatedEntityId">Related entity ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        Task<List<ResourceBookingAggregate>> GetListByRelatedEntityAsync(
            string relatedEntity,
            Guid relatedEntityId,
            CancellationToken cancellationToken = default);
    }
}
