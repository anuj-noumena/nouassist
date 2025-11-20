using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for booking activity report
    /// </summary>
    public class BookingActivityReportDto
    {
        /// <summary>
        /// Start date of the report period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the report period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Resource type ID filter (if any)
        /// </summary>
        public Guid? ResourceTypeId { get; set; }
        
        /// <summary>
        /// Resource type name (if filtered by type)
        /// </summary>
        public string? ResourceTypeName { get; set; }
        
        /// <summary>
        /// Total number of bookings in the report
        /// </summary>
        public int TotalBookingCount { get; set; }
        
        /// <summary>
        /// Number of completed bookings
        /// </summary>
        public int CompletedBookingCount { get; set; }
        
        /// <summary>
        /// Number of cancelled bookings
        /// </summary>
        public int CancelledBookingCount { get; set; }
        
        /// <summary>
        /// Number of rejected bookings
        /// </summary>
        public int RejectedBookingCount { get; set; }
        
        /// <summary>
        /// Average booking duration in minutes
        /// </summary>
        public decimal AverageBookingDuration { get; set; }
        
        /// <summary>
        /// Daily booking statistics
        /// </summary>
        public List<DailyBookingStatDto> DailyStats { get; set; } = new();
        
        /// <summary>
        /// Resource booking statistics
        /// </summary>
        public List<ResourceBookingStatDto> ResourceStats { get; set; } = new();
    }
}
