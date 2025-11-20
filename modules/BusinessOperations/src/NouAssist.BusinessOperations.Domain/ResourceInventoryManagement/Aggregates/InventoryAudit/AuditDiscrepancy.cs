using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit
{
    /// <summary>
    /// Represents a discrepancy found during an inventory audit
    /// </summary>
    public class AuditDiscrepancy : Entity<Guid>
    {
        /// <summary>
        /// ID of the audit session
        /// </summary>
        public Guid AuditSessionId { get; private set; }

        /// <summary>
        /// ID of the inventory item with the discrepancy
        /// </summary>
        public Guid InventoryItemId { get; private set; }

        /// <summary>
        /// Expected quantity in the system
        /// </summary>
        public decimal ExpectedQuantity { get; private set; }

        /// <summary>
        /// Actual quantity found during the audit
        /// </summary>
        public decimal ActualQuantity { get; private set; }

        /// <summary>
        /// Difference between actual and expected quantities
        /// </summary>
        public decimal QuantityDifference => ActualQuantity - ExpectedQuantity;

        /// <summary>
        /// Reason for the discrepancy
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Notes about the discrepancy
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// Time when the discrepancy was recorded
        /// </summary>
        public DateTime RecordedAt { get; private set; }

        /// <summary>
        /// Indicates if this discrepancy has been resolved
        /// </summary>
        public bool IsResolved { get; private set; }

        /// <summary>
        /// Resolution notes
        /// </summary>
        public string ResolutionNotes { get; private set; }

        /// <summary>
        /// Time when the discrepancy was resolved
        /// </summary>
        public DateTime? ResolvedAt { get; private set; }

        protected AuditDiscrepancy()
        {
        }

        /// <summary>
        /// Creates a new audit discrepancy
        /// </summary>
        /// <param name="id">Discrepancy ID</param>
        /// <param name="auditSessionId">Audit session ID</param>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="expectedQuantity">Expected quantity</param>
        /// <param name="actualQuantity">Actual quantity</param>
        /// <param name="reason">Reason for discrepancy (optional)</param>
        /// <param name="notes">Notes about the discrepancy (optional)</param>
        public AuditDiscrepancy(
            Guid id,
            Guid auditSessionId,
            Guid inventoryItemId,
            decimal expectedQuantity,
            decimal actualQuantity,
            string reason = null,
            string notes = null) : base(id)
        {
            AuditSessionId = auditSessionId;
            InventoryItemId = inventoryItemId;
            ExpectedQuantity = expectedQuantity;
            ActualQuantity = actualQuantity;
            Reason = reason;
            Notes = notes;
            RecordedAt = DateTime.Now;
            IsResolved = false;
        }

        /// <summary>
        /// Updates the discrepancy details
        /// </summary>
        /// <param name="actualQuantity">Updated actual quantity</param>
        /// <param name="reason">Updated reason</param>
        /// <param name="notes">Updated notes</param>
        public void Update(decimal actualQuantity, string reason, string notes)
        {
            if (IsResolved)
            {
                throw new ArgumentException("Cannot update a resolved discrepancy");
            }

            ActualQuantity = actualQuantity;
            Reason = reason;
            Notes = notes;
        }

        /// <summary>
        /// Marks the discrepancy as resolved
        /// </summary>
        /// <param name="resolutionNotes">Notes about how the discrepancy was resolved</param>
        public void Resolve(string resolutionNotes)
        {
            if (IsResolved)
            {
                return;
            }

            IsResolved = true;
            ResolutionNotes = resolutionNotes;
            ResolvedAt = DateTime.Now;
        }
    }
}
