using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for cancelling a booking
    /// </summary>
    public class CancelBookingDto
    {
        /// <summary>
        /// Reason for cancellation
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = null!;
    }
}
