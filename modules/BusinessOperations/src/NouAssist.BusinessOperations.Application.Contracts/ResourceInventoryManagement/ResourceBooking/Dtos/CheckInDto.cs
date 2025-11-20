using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos
{
    /// <summary>
    /// DTO for checking in a resource
    /// </summary>
    public class CheckInDto
    {
        /// <summary>
        /// Check-in time (defaults to now if not provided)
        /// </summary>
        public DateTime? CheckedInAt { get; set; }
        
        /// <summary>
        /// Notes about the usage
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
