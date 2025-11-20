using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Repository interface for the StorageLocation aggregate
    /// </summary>
    public interface IStorageLocationRepository : IRepository<StorageLocationAggregate, Guid>
    {
        /// <summary>
        /// Finds a storage location by its code
        /// </summary>
        /// <param name="code">Location code</param>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Storage location or null if not found</returns>
        Task<StorageLocationAggregate> FindByCodeAsync(
            string code,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a storage location with the specified code exists
        /// </summary>
        /// <param name="code">Location code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByCodeAsync(
            string code,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of storage locations by location type
        /// </summary>
        /// <param name="locationType">Location type</param>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of storage locations</returns>
        Task<List<StorageLocationAggregate>> GetListByLocationTypeAsync(
            string locationType,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of active storage locations
        /// </summary>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active storage locations</returns>
        Task<List<StorageLocationAggregate>> GetActiveListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of child locations for a parent location
        /// </summary>
        /// <param name="parentLocationId">Parent location ID</param>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of child storage locations</returns>
        Task<List<StorageLocationAggregate>> GetChildLocationsAsync(
            Guid parentLocationId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of root locations (with no parent)
        /// </summary>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of root storage locations</returns>
        Task<List<StorageLocationAggregate>> GetRootLocationsAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the full location hierarchy as a nested structure
        /// </summary>
        /// <param name="rootLocationId">Root location ID (optional, if not specified returns entire hierarchy)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Hierarchical structure of locations</returns>
        Task<List<LocationHierarchyDto>> GetLocationHierarchyAsync(
            Guid? rootLocationId = null,
            CancellationToken cancellationToken = default);
    }
}
