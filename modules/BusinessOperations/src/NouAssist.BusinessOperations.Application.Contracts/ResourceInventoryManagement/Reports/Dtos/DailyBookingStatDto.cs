using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for daily booking statistics
    /// </summary>
    public class DailyBookingStatDto
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Number of bookings
        /// </summary>
        public int BookingCount { get; set; }
        
        /// <summary>
        /// Number of completed bookings
        /// </summary>
        public int CompletedCount { get; set; }
        
        /// <summary>
        /// Number of cancelled bookings
        /// </summary>
        public int CancelledCount { get; set; }
    }
}
