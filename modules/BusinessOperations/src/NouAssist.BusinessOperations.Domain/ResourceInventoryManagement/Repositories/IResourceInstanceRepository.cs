using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Repository interface for the ResourceInstance aggregate
    /// </summary>
    public interface IResourceInstanceRepository : IRepository<ResourceInstanceAggregate, Guid>
    {
        /// <summary>
        /// Finds a resource instance by its identifier
        /// </summary>
        /// <param name="identifier">Resource instance identifier</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Resource instance or null if not found</returns>
        Task<ResourceInstanceAggregate> FindByIdentifierAsync(
            string identifier,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource instances by resource type
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        Task<List<ResourceInstanceAggregate>> GetListByResourceTypeAsync(
            Guid resourceTypeId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource instances by status
        /// </summary>
        /// <param name="status">Resource status</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        Task<List<ResourceInstanceAggregate>> GetListByStatusAsync(
            ResourceStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource instances for multiple resource types
        /// </summary>
        /// <param name="resourceTypeIds">List of resource type IDs</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        Task<List<ResourceInstanceAggregate>> GetListByResourceTypesAsync(
            List<Guid> resourceTypeIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource instances assigned to a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        Task<List<ResourceInstanceAggregate>> GetListByUserIdAsync(
            Guid userId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource instances by a list of IDs
        /// </summary>
        /// <param name="ids">List of resource instance IDs</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        Task<List<ResourceInstanceAggregate>> GetListAsync(
            List<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds available resources matching specified criteria
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID (optional)</param>
        /// <param name="startTime">Availability start time</param>
        /// <param name="endTime">Availability end time</param>
        /// <param name="attributes">Required attributes (optional)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of available resource instances</returns>
        Task<List<ResourceInstanceAggregate>> FindAvailableResourcesAsync(
            Guid? resourceTypeId,
            DateTime startTime,
            DateTime endTime,
            Dictionary<string, string> attributes = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates multiple resource instances in a batch
        /// </summary>
        /// <param name="resources">List of resource instances to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task UpdateManyAsync(
            IEnumerable<ResourceInstanceAggregate> resources,
            CancellationToken cancellationToken = default);
    }
}
