using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for ending a resource allocation
    /// </summary>
    public class EndAllocationDto
    {
        /// <summary>
        /// End time of the allocation
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// End allocation notes
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
