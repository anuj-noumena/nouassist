using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IInventoryAuditRepository
    /// </summary>
    public class EfCoreInventoryAuditRepository 
        : EfCoreRepository<RIMDbContext, InventoryAuditSessionAggregate, Guid>, IInventoryAuditRepository
    {
        /// <summary>
        /// Creates a new instance of EfCoreInventoryAuditRepository
        /// </summary>
        /// <param name="dbContextProvider">Database context provider</param>
        public EfCoreInventoryAuditRepository(
            IDbContextProvider<RIMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Includes details (discrepancies) when fetching audit sessions
        /// </summary>
        /// <param name="includeDetails">Whether to include details</param>
        /// <returns>Queryable with optional details</returns>
        public override async Task<IQueryable<InventoryAuditSessionAggregate>> WithDetailsAsync(bool includeDetails = true)
        {
            if (!includeDetails)
            {
                return await base.WithDetailsAsync(includeDetails);
            }

            return (await GetQueryableAsync()).Include(ia => ia.Discrepancies);
        }

        /// <summary>
        /// Gets a list of audit sessions for a specific location
        /// </summary>
        /// <param name="locationId">Storage location ID</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        public async Task<List<InventoryAuditSessionAggregate>> GetListByLocationAsync(
            Guid locationId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ia => ia.LocationId == locationId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of audit sessions by user
        /// </summary>
        /// <param name="auditedByUserId">User ID</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        public async Task<List<InventoryAuditSessionAggregate>> GetListByUserAsync(
            Guid auditedByUserId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ia => ia.AuditedByUserId == auditedByUserId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of audit sessions by status
        /// </summary>
        /// <param name="status">Audit status</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        public async Task<List<InventoryAuditSessionAggregate>> GetListByStatusAsync(
            AuditStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ia => ia.Status == status)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of audit sessions for a time period
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        public async Task<List<InventoryAuditSessionAggregate>> GetListForTimeRangeAsync(
            DateTime startTime,
            DateTime endTime,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ia => ia.ScheduledStartTime >= startTime && ia.ScheduledStartTime <= endTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets audit sessions that have discrepancies for a specific inventory item
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="includeDetails">Whether to include details (discrepancies)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of audit sessions</returns>
        public async Task<List<InventoryAuditSessionAggregate>> GetListWithDiscrepanciesForItemAsync(
            Guid inventoryItemId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(true);
            
            // Find audit sessions with discrepancies for the specified item
            var auditSessions = await query
                .Where(ia => ia.Discrepancies.Any(d => d.InventoryItemId == inventoryItemId))
                .ToListAsync(cancellationToken);

            // If details shouldn't be included, reload without details
            if (!includeDetails)
            {
                var sessionIds = auditSessions.Select(ia => ia.Id).ToList();
                query = await GetQueryableAsync();
                return await query
                    .Where(ia => sessionIds.Contains(ia.Id))
                    .ToListAsync(cancellationToken);
            }

            return auditSessions;
        }
    }
}
