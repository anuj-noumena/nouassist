using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for cancelling an audit
    /// </summary>
    public class CancelAuditDto
    {
        /// <summary>
        /// Reason for cancellation
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = null!;
    }
}
