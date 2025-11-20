using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Inventory;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit
{
    /// <summary>
    /// Represents an inventory audit session for reconciling physical inventory with system records.
    /// </summary>
    public class InventoryAuditSessionAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID, if this audit session belongs to a specific tenant
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// ID of the storage location being audited
        /// </summary>
        public Guid LocationId { get; private set; }

        /// <summary>
        /// Name of the audit session
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description or purpose of the audit
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Current status of the audit
        /// </summary>
        public AuditStatus Status { get; private set; }

        /// <summary>
        /// Scheduled start time for the audit
        /// </summary>
        public DateTime ScheduledStartTime { get; private set; }

        /// <summary>
        /// Actual start time of the audit
        /// </summary>
        public DateTime? ActualStartTime { get; private set; }

        /// <summary>
        /// Completion time of the audit
        /// </summary>
        public DateTime? CompletionTime { get; private set; }

        /// <summary>
        /// ID of the user conducting the audit
        /// </summary>
        public Guid AuditedByUserId { get; private set; }

        /// <summary>
        /// Notes about the audit session
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// List of audit discrepancies found during the audit
        /// </summary>
        private readonly List<AuditDiscrepancy> _discrepancies;
        public IReadOnlyCollection<AuditDiscrepancy> Discrepancies => _discrepancies.AsReadOnly();

        protected InventoryAuditSessionAggregate()
        {
            _discrepancies = new List<AuditDiscrepancy>();
        }

        /// <summary>
        /// Creates a new inventory audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="name">Name of the audit session</param>
        /// <param name="auditedByUserId">ID of the user conducting the audit</param>
        /// <param name="scheduledStartTime">Scheduled start time</param>
        /// <param name="description">Description (optional)</param>
        /// <param name="notes">Notes (optional)</param>
        /// <param name="tenantId">Tenant ID (optional)</param>
        public InventoryAuditSessionAggregate(
            Guid id,
            Guid locationId,
            string name,
            Guid auditedByUserId,
            DateTime scheduledStartTime,
            string description = null,
            string notes = null,
            Guid? tenantId = null) : base(id)
        {
            LocationId = locationId;
            SetName(name);
            SetDescription(description);
            AuditedByUserId = auditedByUserId;
            ScheduledStartTime = scheduledStartTime;
            Notes = notes;
            Status = AuditStatus.Planned;
            TenantId = tenantId;
            _discrepancies = new List<AuditDiscrepancy>();

            AddDistributedEvent(new InventoryAuditPlannedEto
            {
                AuditSessionId = Id,
                LocationId = LocationId,
                Name = Name,
                ScheduledStartTime = ScheduledStartTime,
                AuditedByUserId = AuditedByUserId,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the audit session information
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="scheduledStartTime">Scheduled start time</param>
        /// <param name="notes">Notes</param>
        public void Update(
            string name,
            string description,
            DateTime scheduledStartTime,
            string notes = null)
        {
            if (Status != AuditStatus.Planned)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CannotUpdateInProgressAudit");
            }

            SetName(name);
            SetDescription(description);
            ScheduledStartTime = scheduledStartTime;
            Notes = notes;
        }

        /// <summary>
        /// Starts the audit session
        /// </summary>
        /// <param name="startTime">Start time (defaults to now)</param>
        public void StartAudit(DateTime? startTime = null)
        {
            if (Status != AuditStatus.Planned)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CannotStartAuditWithStatus", Status.ToString());
            }

            Status = AuditStatus.InProgress;
            ActualStartTime = startTime ?? DateTime.Now;

            AddDistributedEvent(new InventoryAuditStartedEto
            {
                AuditSessionId = Id,
                LocationId = LocationId,
                ActualStartTime = ActualStartTime.Value,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Adds a discrepancy found during the audit
        /// </summary>
        /// <param name="discrepancyId">Discrepancy ID</param>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="expectedQuantity">Expected quantity in system</param>
        /// <param name="actualQuantity">Actual quantity found</param>
        /// <param name="reason">Reason for discrepancy (optional)</param>
        /// <param name="notes">Notes about the discrepancy (optional)</param>
        /// <returns>The created discrepancy</returns>
        public AuditDiscrepancy AddDiscrepancy(
            Guid discrepancyId,
            Guid inventoryItemId,
            decimal expectedQuantity,
            decimal actualQuantity,
            string reason = null,
            string notes = null)
        {
            if (Status != AuditStatus.InProgress)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CannotAddDiscrepancyInStatus", Status.ToString());
            }

            var discrepancy = new AuditDiscrepancy(
                discrepancyId,
                Id,
                inventoryItemId,
                expectedQuantity,
                actualQuantity,
                reason,
                notes);

            _discrepancies.Add(discrepancy);

            return discrepancy;
        }

        /// <summary>
        /// Completes the audit session
        /// </summary>
        /// <param name="completionTime">Completion time (defaults to now)</param>
        /// <param name="completionNotes">Completion notes (optional)</param>
        public void CompleteAudit(DateTime? completionTime = null, string completionNotes = null)
        {
            if (Status != AuditStatus.InProgress)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CannotCompleteAuditInStatus", Status.ToString());
            }

            CompletionTime = completionTime ?? DateTime.Now;
            if (!string.IsNullOrEmpty(completionNotes))
            {
                Notes = string.IsNullOrEmpty(Notes) ? completionNotes : $"{Notes}\n\nCompletion notes: {completionNotes}";
            }

            Status = _discrepancies.Any() ? AuditStatus.CompleteWithDiscrepancies : AuditStatus.CompleteNoDiscrepancies;

            AddDistributedEvent(new InventoryAuditCompletedEto
            {
                AuditSessionId = Id,
                LocationId = LocationId,
                Status = Status.ToString(),
                CompletionTime = CompletionTime.Value,
                ItemsAudited = _discrepancies.Select(d => d.InventoryItemId).Distinct().Count(),
                DiscrepanciesFound = _discrepancies.Count,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the audit as reconciled after discrepancies have been addressed
        /// </summary>
        /// <param name="reconciliationNotes">Notes about the reconciliation</param>
        public void MarkAsReconciled(string reconciliationNotes)
        {
            if (Status != AuditStatus.CompleteWithDiscrepancies)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CannotReconcileAuditInStatus", Status.ToString());
            }

            Status = AuditStatus.Reconciled;
            Notes = string.IsNullOrEmpty(Notes) ? reconciliationNotes : $"{Notes}\n\nReconciliation notes: {reconciliationNotes}";

            AddDistributedEvent(new InventoryAuditReconciledEto
            {
                AuditSessionId = Id,
                LocationId = LocationId,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Cancels the audit session
        /// </summary>
        /// <param name="reason">Cancellation reason</param>
        public void CancelAudit(string reason)
        {
            if (Status == AuditStatus.Reconciled || Status == AuditStatus.CompleteNoDiscrepancies)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CannotCancelCompletedAudit");
            }

            Status = AuditStatus.Cancelled;
            Notes = string.IsNullOrEmpty(Notes) ? $"Cancelled: {reason}" : $"{Notes}\n\nCancelled: {reason}";

            AddDistributedEvent(new InventoryAuditCancelledEto
            {
                AuditSessionId = Id,
                LocationId = LocationId,
                Reason = reason,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Gets all discrepancies for a specific inventory item
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <returns>List of discrepancies</returns>
        public IReadOnlyList<AuditDiscrepancy> GetDiscrepanciesForItem(Guid inventoryItemId)
        {
            return _discrepancies.Where(d => d.InventoryItemId == inventoryItemId).ToList();
        }

        /// <summary>
        /// Gets a summary of all discrepancies
        /// </summary>
        /// <returns>Dictionary with inventory item IDs as keys and total quantity difference as values</returns>
        public Dictionary<Guid, decimal> GetDiscrepancySummary()
        {
            return _discrepancies
                .GroupBy(d => d.InventoryItemId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(d => d.ActualQuantity - d.ExpectedQuantity)
                );
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name)
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description)
            );
        }
    }
}
