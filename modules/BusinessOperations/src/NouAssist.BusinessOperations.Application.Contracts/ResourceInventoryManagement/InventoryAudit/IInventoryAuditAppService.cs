using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit
{
    /// <summary>
    /// Application service interface for managing inventory audits
    /// </summary>
    public interface IInventoryAuditAppService : IApplicationService
    {
        /// <summary>
        /// Gets an inventory audit session by ID
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <returns>Inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged and filtered list of inventory audit sessions
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged result of inventory audit session DTOs</returns>
        Task<PagedResultDto<InventoryAuditSessionDto>> GetListAsync(GetInventoryAuditSessionListDto input);

        /// <summary>
        /// Creates a new inventory audit session
        /// </summary>
        /// <param name="input">Inventory audit session creation data</param>
        /// <returns>Created inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> CreateAsync(CreateInventoryAuditSessionDto input);

        /// <summary>
        /// Updates an existing inventory audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="input">Inventory audit session update data</param>
        /// <returns>Updated inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> UpdateAsync(Guid id, UpdateInventoryAuditSessionDto input);

        /// <summary>
        /// Deletes an inventory audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Starts an inventory audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="input">Start audit data</param>
        /// <returns>Updated inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> StartAsync(Guid id, StartAuditDto input);

        /// <summary>
        /// Records a discrepancy during an audit
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="input">Discrepancy data</param>
        /// <returns>Created audit discrepancy DTO</returns>
        Task<AuditDiscrepancyDto> RecordDiscrepancyAsync(Guid id, RecordDiscrepancyDto input);

        /// <summary>
        /// Completes an inventory audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="input">Complete audit data</param>
        /// <returns>Updated inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> CompleteAsync(Guid id, CompleteAuditDto input);

        /// <summary>
        /// Reconciles discrepancies found during an audit by adjusting inventory
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="input">Reconcile audit data</param>
        /// <returns>Updated inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> ReconcileAsync(Guid id, ReconcileAuditDto input);

        /// <summary>
        /// Cancels an inventory audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <param name="input">Cancel audit data</param>
        /// <returns>Updated inventory audit session DTO</returns>
        Task<InventoryAuditSessionDto> CancelAsync(Guid id, CancelAuditDto input);

        /// <summary>
        /// Gets all discrepancies for an audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <returns>List of audit discrepancies</returns>
        Task<ListResultDto<AuditDiscrepancyDto>> GetDiscrepanciesAsync(Guid id);

        /// <summary>
        /// Gets a summary of discrepancies for an audit session
        /// </summary>
        /// <param name="id">Audit session ID</param>
        /// <returns>List of discrepancy summary items</returns>
        Task<ListResultDto<DiscrepancySummaryDto>> GetDiscrepancySummaryAsync(Guid id);

        /// <summary>
        /// Gets audit sessions for a specific location
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <param name="maxResultCount">Maximum number of results</param>
        /// <param name="skipCount">Number of results to skip</param>
        /// <returns>Paged list of audit sessions</returns>
        Task<PagedResultDto<InventoryAuditSessionDto>> GetLocationAuditsAsync(
            Guid locationId, 
            int maxResultCount = 10, 
            int skipCount = 0);

        /// <summary>
        /// Gets audit sessions with discrepancies for a specific inventory item
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <returns>List of audit sessions</returns>
        Task<ListResultDto<InventoryAuditSessionDto>> GetItemDiscrepanciesAsync(Guid inventoryItemId);
    }
}
