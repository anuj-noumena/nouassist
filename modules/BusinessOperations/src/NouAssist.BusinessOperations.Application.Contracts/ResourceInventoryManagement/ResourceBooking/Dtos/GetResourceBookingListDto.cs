using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for getting a list of resource bookings with filtering and sorting
    /// </summary>
    public class GetResourceBookingListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter bookings by booking code, purpose, or notes
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by resource instance ID
        /// </summary>
        public Guid? ResourceInstanceId { get; set; }
        
        /// <summary>
        /// Filter by booking status
        /// </summary>
        public BookingStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by booked by user ID
        /// </summary>
        public Guid? BookedByUserId { get; set; }
        
        /// <summary>
        /// Filter by assigned to user ID
        /// </summary>
        public Guid? AssignedToUserId { get; set; }
        
        /// <summary>
        /// Filter by start date (lower bound)
        /// </summary>
        public DateTime? FromDate { get; set; }
        
        /// <summary>
        /// Filter by end date (upper bound)
        /// </summary>
        public DateTime? ToDate { get; set; }
        
        /// <summary>
        /// Filter by related entity and ID
        /// </summary>
        public string? RelatedEntity { get; set; }
        
        /// <summary>
        /// Filter by related entity ID
        /// </summary>
        public Guid? RelatedEntityId { get; set; }
    }
}
