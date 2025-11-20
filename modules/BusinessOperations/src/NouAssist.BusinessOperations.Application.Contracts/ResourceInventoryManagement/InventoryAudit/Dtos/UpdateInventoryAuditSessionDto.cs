using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for updating an inventory audit session
    /// </summary>
    public class UpdateInventoryAuditSessionDto
    {
        /// <summary>
        /// Name of the audit session
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description or purpose of the audit
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Scheduled start time for the audit
        /// </summary>
        [Required]
        public DateTime ScheduledStartTime { get; set; }

        /// <summary>
        /// Notes about the audit session
        /// </summary>
        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
