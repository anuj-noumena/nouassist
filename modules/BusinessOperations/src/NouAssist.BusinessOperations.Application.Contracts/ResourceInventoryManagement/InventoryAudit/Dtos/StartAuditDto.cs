using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for starting an audit
    /// </summary>
    public class StartAuditDto
    {
        /// <summary>
        /// Start time (defaults to now if not provided)
        /// </summary>
        public DateTime? StartTime { get; set; }
    }
}
