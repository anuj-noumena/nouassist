using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking
{
    /// <summary>
    /// Application service interface for managing resource bookings
    /// </summary>
    public interface IResourceBookingAppService : IApplicationService
    {
        /// <summary>
        /// Gets a resource booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Resource booking DTO</returns>
        Task<ResourceBookingDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a resource booking by booking code
        /// </summary>
        /// <param name="bookingCode">Booking code</param>
        /// <returns>Resource booking DTO</returns>
        Task<ResourceBookingDto> GetByBookingCodeAsync(string bookingCode);

        /// <summary>
        /// Gets a paged and filtered list of resource bookings
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged result of resource booking DTOs</returns>
        Task<PagedResultDto<ResourceBookingDto>> GetListAsync(GetResourceBookingListDto input);

        /// <summary>
        /// Creates a new resource booking
        /// </summary>
        /// <param name="input">Resource booking creation data</param>
        /// <returns>Created resource booking DTO</returns>
        Task<ResourceBookingDto> CreateAsync(CreateResourceBookingDto input);

        /// <summary>
        /// Updates an existing resource booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="input">Resource booking update data</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> UpdateAsync(Guid id, UpdateResourceBookingDto input);

        /// <summary>
        /// Deletes a resource booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Confirms a pending booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> ConfirmAsync(Guid id);

        /// <summary>
        /// Rejects a pending booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="input">Rejection data</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> RejectAsync(Guid id, RejectBookingDto input);

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="input">Cancellation data</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> CancelAsync(Guid id, CancelBookingDto input);

        /// <summary>
        /// Checks out a resource (marks booking as in use)
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> CheckOutAsync(Guid id);

        /// <summary>
        /// Checks in a resource (marks booking as completed)
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="input">Check-in data</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> CheckInAsync(Guid id, CheckInDto input);

        /// <summary>
        /// Extends a booking's end time
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="input">Extension data</param>
        /// <returns>Updated resource booking DTO</returns>
        Task<ResourceBookingDto> ExtendAsync(Guid id, ExtendBookingDto input);

        /// <summary>
        /// Gets bookings for a specific resource instance
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="maxResultCount">Maximum number of results</param>
        /// <param name="skipCount">Number of results to skip</param>
        /// <returns>Paged list of bookings</returns>
        Task<PagedResultDto<ResourceBookingDto>> GetResourceBookingsAsync(
            Guid resourceInstanceId, 
            int maxResultCount = 10, 
            int skipCount = 0);

        /// <summary>
        /// Gets bookings for the current user
        /// </summary>
        /// <param name="maxResultCount">Maximum number of results</param>
        /// <param name="skipCount">Number of results to skip</param>
        /// <returns>Paged list of bookings</returns>
        Task<PagedResultDto<ResourceBookingDto>> GetMyBookingsAsync(
            int maxResultCount = 10, 
            int skipCount = 0);

        /// <summary>
        /// Checks if a resource is available for booking during a specific time period
        /// </summary>
        /// <param name="resourceInstanceId">Resource instance ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="excludeBookingId">Booking ID to exclude from check (for updates)</param>
        /// <returns>True if the resource is available</returns>
        Task<bool> IsResourceAvailableAsync(
            Guid resourceInstanceId, 
            DateTime startTime, 
            DateTime endTime, 
            Guid? excludeBookingId = null);
    }
}
