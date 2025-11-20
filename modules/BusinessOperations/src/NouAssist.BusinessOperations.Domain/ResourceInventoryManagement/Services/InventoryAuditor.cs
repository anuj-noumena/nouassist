using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Inventory;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Location;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for inventory audit operations
    /// </summary>
    public class InventoryAuditor : DomainService
    {
        private readonly IInventoryAuditRepository _auditRepository;
        private readonly IInventoryItemRepository _inventoryRepository;
        private readonly IStorageLocationRepository _locationRepository;
        private readonly InventoryManager _inventoryManager;
        private readonly ILogger<InventoryAuditor> _logger;

        /// <summary>
        /// Creates a new instance of InventoryAuditor
        /// </summary>
        /// <param name="auditRepository">Inventory audit repository</param>
        /// <param name="inventoryRepository">Inventory item repository</param>
        /// <param name="locationRepository">Storage location repository</param>
        /// <param name="inventoryManager">Inventory manager</param>
        /// <param name="logger">Logger</param>
        public InventoryAuditor(
            IInventoryAuditRepository auditRepository,
            IInventoryItemRepository inventoryRepository,
            IStorageLocationRepository locationRepository,
            InventoryManager inventoryManager,
            ILogger<InventoryAuditor> logger)
        {
            _auditRepository = auditRepository;
            _inventoryRepository = inventoryRepository;
            _locationRepository = locationRepository;
            _inventoryManager = inventoryManager;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new inventory audit session
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="name">Name of the audit session</param>
        /// <param name="auditedByUserId">ID of the user conducting the audit</param>
        /// <param name="scheduledStartTime">Scheduled start time</param>
        /// <param name="description">Description (optional)</param>
        /// <param name="notes">Notes (optional)</param>
        /// <returns>The created audit session</returns>
        public async Task<InventoryAuditSessionAggregate> CreateAuditSessionAsync(
            Guid locationId,
            string name,
            Guid auditedByUserId,
            DateTime scheduledStartTime,
            string description = null,
            string notes = null)
        {
            // Validate location exists
            var location = await _locationRepository.GetAsync(locationId);
            if (location == null)
            {
                throw new StorageLocationNotFoundException(locationId);
            }

            // Generate a new audit session ID
            var auditSessionId = GuidGenerator.Create();

            // Create the audit session
            var auditSession = new InventoryAuditSessionAggregate(
                auditSessionId,
                locationId,
                name,
                auditedByUserId,
                scheduledStartTime,
                description,
                notes);

            // Save the audit session
            await _auditRepository.InsertAsync(auditSession);

            _logger.LogInformation(
                "Created inventory audit session {AuditSessionId} for location {LocationId} scheduled at {ScheduledStartTime}",
                auditSessionId, locationId, scheduledStartTime);

            return auditSession;
        }

        /// <summary>
        /// Starts an inventory audit session
        /// </summary>
        /// <param name="auditSessionId">Audit session ID</param>
        /// <param name="startTime">Start time (defaults to now)</param>
        /// <returns>The updated audit session</returns>
        public async Task<InventoryAuditSessionAggregate> StartAuditSessionAsync(
            Guid auditSessionId,
            DateTime? startTime = null)
        {
            var auditSession = await _auditRepository.GetAsync(auditSessionId);
            if (auditSession == null)
            {
                throw new ArgumentException($"Audit session with ID {auditSessionId} not found", nameof(auditSessionId));
            }

            auditSession.StartAudit(startTime);
            await _auditRepository.UpdateAsync(auditSession);

            _logger.LogInformation(
                "Started inventory audit session {AuditSessionId} for location {LocationId}",
                auditSessionId, auditSession.LocationId);

            return auditSession;
        }

        /// <summary>
        /// Records a discrepancy during an audit
        /// </summary>
        /// <param name="auditSessionId">Audit session ID</param>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="actualQuantity">Actual quantity found</param>
        /// <param name="reason">Reason for discrepancy (optional)</param>
        /// <param name="notes">Notes about the discrepancy (optional)</param>
        /// <returns>The created discrepancy</returns>
        public async Task<AuditDiscrepancy> RecordDiscrepancyAsync(
            Guid auditSessionId,
            Guid inventoryItemId,
            decimal actualQuantity,
            string reason = null,
            string notes = null)
        {
            var auditSession = await _auditRepository.GetAsync(auditSessionId);
            if (auditSession == null)
            {
                throw new ArgumentException($"Audit session with ID {auditSessionId} not found", nameof(auditSessionId));
            }

            // Get the inventory item to find expected quantity
            var inventoryItem = await _inventoryRepository.GetAsync(inventoryItemId);
            if (inventoryItem == null)
            {
                throw new InventoryItemNotFoundException(inventoryItemId);
            }

            // Get the current stock level at the audited location
            var stockLevel = inventoryItem.GetStockLevel(auditSession.LocationId);
            var expectedQuantity = stockLevel?.Quantity ?? 0;

            // If actual matches expected, no discrepancy to record
            if (actualQuantity == expectedQuantity)
            {
                return null;
            }

            // Generate a new discrepancy ID
            var discrepancyId = GuidGenerator.Create();

            // Record the discrepancy
            var discrepancy = auditSession.AddDiscrepancy(
                discrepancyId,
                inventoryItemId,
                expectedQuantity,
                actualQuantity,
                reason,
                notes);

            // Save the audit session
            await _auditRepository.UpdateAsync(auditSession);

            _logger.LogInformation(
                "Recorded inventory discrepancy for item {ItemId} in audit {AuditSessionId}: expected {ExpectedQuantity}, found {ActualQuantity}",
                inventoryItemId, auditSessionId, expectedQuantity, actualQuantity);

            return discrepancy;
        }

        /// <summary>
        /// Completes an inventory audit session
        /// </summary>
        /// <param name="auditSessionId">Audit session ID</param>
        /// <param name="completionTime">Completion time (defaults to now)</param>
        /// <param name="completionNotes">Completion notes (optional)</param>
        /// <returns>The updated audit session</returns>
        public async Task<InventoryAuditSessionAggregate> CompleteAuditSessionAsync(
            Guid auditSessionId,
            DateTime? completionTime = null,
            string completionNotes = null)
        {
            var auditSession = await _auditRepository.GetAsync(auditSessionId);
            if (auditSession == null)
            {
                throw new ArgumentException($"Audit session with ID {auditSessionId} not found", nameof(auditSessionId));
            }

            auditSession.CompleteAudit(completionTime, completionNotes);
            await _auditRepository.UpdateAsync(auditSession);

            _logger.LogInformation(
                "Completed inventory audit session {AuditSessionId} for location {LocationId} with {DiscrepancyCount} discrepancies",
                auditSessionId, auditSession.LocationId, auditSession.Discrepancies.Count);

            return auditSession;
        }

        /// <summary>
        /// Reconciles discrepancies found during an audit by adjusting inventory
        /// </summary>
        /// <param name="auditSessionId">Audit session ID</param>
        /// <param name="reconciliationNotes">Notes about the reconciliation</param>
        /// <param name="operatorId">ID of the operator performing the reconciliation</param>
        /// <returns>The updated audit session and list of inventory transactions</returns>
        public async Task<(InventoryAuditSessionAggregate, List<InventoryTransaction>)> ReconcileAuditAsync(
            Guid auditSessionId,
            string reconciliationNotes,
            Guid operatorId)
        {
            var auditSession = await _auditRepository.GetAsync(auditSessionId);
            if (auditSession == null)
            {
                throw new ArgumentException($"Audit session with ID {auditSessionId} not found", nameof(auditSessionId));
            }

            if (auditSession.Status != AuditStatus.CompleteWithDiscrepancies)
            {
                throw new ArgumentException($"Audit session must be completed with discrepancies to reconcile", nameof(auditSessionId));
            }

            // Group discrepancies by inventory item
            var discrepancySummary = auditSession.GetDiscrepancySummary();
            var transactions = new List<InventoryTransaction>();

            // Process each item with discrepancies
            foreach (var discrepancy in discrepancySummary)
            {
                var inventoryItemId = discrepancy.Key;
                var quantityDifference = discrepancy.Value;

                // Skip if no adjustment needed
                if (quantityDifference == 0)
                {
                    continue;
                }

                try
                {
                    // Adjust inventory to match the audit findings
                    var result = await _inventoryManager.AdjustStockLevelAsync(
                        inventoryItemId,
                        auditSession.LocationId,
                        quantityDifference,
                        InventoryTransactionType.Reconciliation,
                        $"Audit reconciliation: {auditSessionId}",
                        reconciliationNotes,
                        operatorId);

                    transactions.Add(result.Item2);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Failed to reconcile inventory for item {ItemId} in audit {AuditSessionId}",
                        inventoryItemId, auditSessionId);
                    // Continue with other items even if one fails
                }
            }

            // Mark the audit as reconciled
            auditSession.MarkAsReconciled(reconciliationNotes);
            await _auditRepository.UpdateAsync(auditSession);

            _logger.LogInformation(
                "Reconciled inventory audit session {AuditSessionId} for location {LocationId} with {TransactionCount} adjustments",
                auditSessionId, auditSession.LocationId, transactions.Count);

            return (auditSession, transactions);
        }

        /// <summary>
        /// Cancels an inventory audit session
        /// </summary>
        /// <param name="auditSessionId">Audit session ID</param>
        /// <param name="reason">Cancellation reason</param>
        /// <returns>The updated audit session</returns>
        public async Task<InventoryAuditSessionAggregate> CancelAuditSessionAsync(
            Guid auditSessionId,
            string reason)
        {
            var auditSession = await _auditRepository.GetAsync(auditSessionId);
            if (auditSession == null)
            {
                throw new ArgumentException($"Audit session with ID {auditSessionId} not found", nameof(auditSessionId));
            }

            auditSession.CancelAudit(reason);
            await _auditRepository.UpdateAsync(auditSession);

            _logger.LogInformation(
                "Cancelled inventory audit session {AuditSessionId} for location {LocationId}: {Reason}",
                auditSessionId, auditSession.LocationId, reason);

            return auditSession;
        }
    }
}
