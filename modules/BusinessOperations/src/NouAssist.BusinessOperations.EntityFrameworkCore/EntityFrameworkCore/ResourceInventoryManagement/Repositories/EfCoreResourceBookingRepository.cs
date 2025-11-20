using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IResourceBookingRepository
    /// </summary>
    public class EfCoreResourceBookingRepository 
        : EfCoreRepository<RIMDbContext, ResourceBookingAggregate, Guid>, IResourceBookingRepository
    {
        /// <summary>
        /// Creates a new instance of EfCoreResourceBookingRepository
        /// </summary>
        /// <param name="dbContextProvider">Database context provider</param>
        public EfCoreResourceBookingRepository(
            IDbContextProvider<RIMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Finds a booking by its booking code
        /// </summary>
        /// <param name="bookingCode">Booking code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Booking or null if not found</returns>
        public async Task<ResourceBookingAggregate> FindByBookingCodeAsync(
            string bookingCode,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(rb => rb.BookingCode == bookingCode, cancellationToken);
        }

        /// <summary>
        /// Gets a list of bookings for a specific resource instance
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetListByResourceInstanceAsync(
            Guid resourceInstanceId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rb => rb.ResourceInstanceId == resourceInstanceId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets active bookings for a resource instance during a specific time period
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetActiveBookingsForResourceAsync(
            Guid resourceInstanceId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rb => rb.ResourceInstanceId == resourceInstanceId &&
                             rb.Status != BookingStatus.Cancelled &&
                             rb.Status != BookingStatus.Rejected &&
                             rb.Status != BookingStatus.Completed &&
                             EF.Property<DateTime>(rb, "StartTime") < endTime &&
                             EF.Property<DateTime>(rb, "EndTime") > startTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of bookings by user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetListByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rb => rb.BookedByUserId == userId || rb.AssignedToUserId == userId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of bookings by status
        /// </summary>
        /// <param name="status">Booking status</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetListByStatusAsync(
            BookingStatus status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rb => rb.Status == status)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of bookings for a time period
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetListForTimeRangeAsync(
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rb => rb.Status != BookingStatus.Cancelled &&
                             rb.Status != BookingStatus.Rejected &&
                             EF.Property<DateTime>(rb, "StartTime") < endTime &&
                             EF.Property<DateTime>(rb, "EndTime") > startTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of bookings for a related entity
        /// </summary>
        /// <param name="relatedEntity">Related entity type</param>
        /// <param name="relatedEntityId">Related entity ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of bookings</returns>
        public async Task<List<ResourceBookingAggregate>> GetListByRelatedEntityAsync(
            string relatedEntity,
            Guid relatedEntityId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rb => rb.RelatedEntity == relatedEntity && rb.RelatedEntityId == relatedEntityId)
                .ToListAsync(cancellationToken);
        }
    }
}
