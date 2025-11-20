using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for completing an audit
    /// </summary>
    public class CompleteAuditDto
    {
        /// <summary>
        /// Completion time (defaults to now if not provided)
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// Completion notes
        /// </summary>
        [StringLength(1000)]
        public string? CompletionNotes { get; set; }
    }
}
