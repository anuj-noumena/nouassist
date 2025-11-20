using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for resource booking statistics
    /// </summary>
    public class ResourceBookingStatDto
    {
        /// <summary>
        /// Resource instance ID
        /// </summary>
        public Guid ResourceId { get; set; }
        
        /// <summary>
        /// Resource name
        /// </summary>
        public string ResourceName { get; set; } = null!;
        
        /// <summary>
        /// Total number of bookings
        /// </summary>
        public int BookingCount { get; set; }
        
        /// <summary>
        /// Total booking duration in minutes
        /// </summary>
        public int TotalDuration { get; set; }
        
        /// <summary>
        /// Average booking duration in minutes
        /// </summary>
        public decimal AverageDuration { get; set; }
    }
}
