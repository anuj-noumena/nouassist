using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for rejecting a booking
    /// </summary>
    public class RejectBookingDto
    {
        /// <summary>
        /// Reason for rejection
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = null!;
    }
}
