using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for reconciling an audit
    /// </summary>
    public class ReconcileAuditDto
    {
        /// <summary>
        /// Notes about the reconciliation
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string ReconciliationNotes { get; set; } = null!;
    }
}
