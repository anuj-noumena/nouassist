using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos
{
    /// <summary>
    /// DTO for audit discrepancy information
    /// </summary>
    public class AuditDiscrepancyDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID of the audit session
        /// </summary>
        public Guid AuditSessionId { get; set; }

        /// <summary>
        /// ID of the inventory item with the discrepancy
        /// </summary>
        public Guid InventoryItemId { get; set; }
        
        /// <summary>
        /// SKU of the inventory item
        /// </summary>
        public string ItemSku { get; set; } = null!;
        
        /// <summary>
        /// Name of the inventory item
        /// </summary>
        public string ItemName { get; set; } = null!;

        /// <summary>
        /// Expected quantity in the system
        /// </summary>
        public decimal ExpectedQuantity { get; set; }

        /// <summary>
        /// Actual quantity found during the audit
        /// </summary>
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// Difference between actual and expected quantities
        /// </summary>
        public decimal QuantityDifference => ActualQuantity - ExpectedQuantity;

        /// <summary>
        /// Reason for the discrepancy
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Notes about the discrepancy
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Time when the discrepancy was recorded
        /// </summary>
        public DateTime RecordedAt { get; set; }

        /// <summary>
        /// Indicates if this discrepancy has been resolved
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Resolution notes
        /// </summary>
        public string? ResolutionNotes { get; set; }

        /// <summary>
        /// Time when the discrepancy was resolved
        /// </summary>
        public DateTime? ResolvedAt { get; set; }
    }
}
