using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IResourceTypeRepository
    /// </summary>
    public class EfCoreResourceTypeRepository 
        : EfCoreRepository<RIMDbContext, ResourceTypeAggregate, Guid>, IResourceTypeRepository
    {
        /// <summary>
        /// Creates a new instance of EfCoreResourceTypeRepository
        /// </summary>
        /// <param name="dbContextProvider">Database context provider</param>
        public EfCoreResourceTypeRepository(
            IDbContextProvider<RIMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Includes details (attribute definitions) when fetching resource types
        /// </summary>
        /// <param name="includeDetails">Whether to include details</param>
        /// <returns>Queryable with optional details</returns>
        public override async Task<IQueryable<ResourceTypeAggregate>> WithDetailsAsync(bool includeDetails = true)
        {
            if (!includeDetails)
            {
                return await base.WithDetailsAsync(includeDetails);
            }

            return (await GetQueryableAsync()).Include(rt => rt.AttributeDefinitions);
        }

        /// <summary>
        /// Finds a resource type by its code
        /// </summary>
        /// <param name="code">Resource type code</param>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Resource type or null if not found</returns>
        public async Task<ResourceTypeAggregate> FindByCodeAsync(
            string code,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .FirstOrDefaultAsync(rt => rt.Code == code, cancellationToken);
        }

        /// <summary>
        /// Checks if a resource type with the specified code exists
        /// </summary>
        /// <param name="code">Resource type code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        public async Task<bool> ExistsByCodeAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .AnyAsync(rt => rt.Code == code, cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource types that match the specified category
        /// </summary>
        /// <param name="category">Resource category</param>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource types</returns>
        public async Task<List<ResourceTypeAggregate>> GetListByCategoryAsync(
            string category,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(rt => rt.Category.ToString() == category)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of active resource types
        /// </summary>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active resource types</returns>
        public async Task<List<ResourceTypeAggregate>> GetActiveListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(rt => rt.IsActive)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource types by their category path
        /// </summary>
        /// <param name="categoryPath">Category path</param>
        /// <param name="includeChildCategories">Whether to include resource types in child categories</param>
        /// <param name="includeDetails">Whether to include details (attribute definitions)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource types</returns>
        public async Task<List<ResourceTypeAggregate>> GetListByCategoryPathAsync(
            string categoryPath,
            bool includeChildCategories = false,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);

            if (includeChildCategories)
            {
                // Include all resource types where the category path starts with the specified path
                return await query
                    .Where(rt => EF.Property<string>(rt, "CategoryPath").StartsWith(categoryPath))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                // Include only resource types with an exact category path match
                return await query
                    .Where(rt => EF.Property<string>(rt, "CategoryPath") == categoryPath)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
