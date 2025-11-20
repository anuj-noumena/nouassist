using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for extending a booking's end time
    /// </summary>
    public class ExtendBookingDto
    {
        /// <summary>
        /// New end time for the booking
        /// </summary>
        [Required]
        public DateTime NewEndTime { get; set; }
    }
}
