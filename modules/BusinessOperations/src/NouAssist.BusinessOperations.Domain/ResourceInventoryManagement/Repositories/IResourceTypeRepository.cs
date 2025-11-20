using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Repository interface for the ResourceType aggregate
    /// </summary>
    public interface IResourceTypeRepository : IRepository<ResourceTypeAggregate, Guid>
    {
        /// <summary>
        /// Finds a resource type by its code
        /// </summary>
        /// <param name="code">Resource type code</param>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Resource type or null if not found</returns>
        Task<ResourceTypeAggregate> FindByCodeAsync(
            string code,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a resource type with the specified code exists
        /// </summary>
        /// <param name="code">Resource type code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByCodeAsync(
            string code,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource types that match the specified category
        /// </summary>
        /// <param name="category">Resource category</param>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource types</returns>
        Task<List<ResourceTypeAggregate>> GetListByCategoryAsync(
            string category,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of active resource types
        /// </summary>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active resource types</returns>
        Task<List<ResourceTypeAggregate>> GetActiveListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of resource types by their category path
        /// </summary>
        /// <param name="categoryPath">Category path</param>
        /// <param name="includeChildCategories">Whether to include resource types in child categories</param>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource types</returns>
        Task<List<ResourceTypeAggregate>> GetListByCategoryPathAsync(
            string categoryPath,
            bool includeChildCategories = false,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }
}
