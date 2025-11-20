using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Repository interface for the InventoryAuditSession aggregate
    /// </summary>
    public interface IInventoryAuditRepository : IRepository<InventoryAuditSessionAggregate, Guid>
    {
        /// <summary>
        /// Gets a list of audit sessions for a specific location
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        Task<List<InventoryAuditSessionAggregate>> GetListByLocationAsync(
            Guid locationId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of audit sessions by user
        /// </summary>
        /// <param name="auditedByUserId">User ID</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        Task<List<InventoryAuditSessionAggregate>> GetListByUserAsync(
            Guid auditedByUserId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of audit sessions by status
        /// </summary>
        /// <param name="status">Audit status</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        Task<List<InventoryAuditSessionAggregate>> GetListByStatusAsync(
            AuditStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of audit sessions for a time period
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        Task<List<InventoryAuditSessionAggregate>> GetListForTimeRangeAsync(
            DateTime startTime,
            DateTime endTime,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets audit sessions that have discrepancies for a specific inventory item
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        Task<List<InventoryAuditSessionAggregate>> GetListWithDiscrepanciesForItemAsync(
            Guid inventoryItemId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
