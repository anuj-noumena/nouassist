using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Inventory
{
    /// <summary>
    /// Event raised when an inventory audit is completed
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAuditCompleted")]
    public class InventoryAuditCompletedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the audit session
        /// </summary>
        public Guid AuditSessionId { get; set; }

        /// <summary>
        /// ID of the location audited
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Status of the completed audit
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Completion time of the audit
        /// </summary>
        public DateTime CompletionTime { get; set; }

        /// <summary>
        /// Number of items audited
        /// </summary>
        public int ItemsAudited { get; set; }

        /// <summary>
        /// Number of discrepancies found
        /// </summary>
        public int DiscrepanciesFound { get; set; }
    }
}
