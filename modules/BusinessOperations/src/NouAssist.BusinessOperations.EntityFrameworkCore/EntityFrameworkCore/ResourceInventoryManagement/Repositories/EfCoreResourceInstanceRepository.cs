using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IResourceInstanceRepository
    /// </summary>
    public class EfCoreResourceInstanceRepository 
        : EfCoreRepository<RIMDbContext, ResourceInstanceAggregate, Guid>, IResourceInstanceRepository
    {
        /// <summary>
        /// Creates a new instance of EfCoreResourceInstanceRepository
        /// </summary>
        /// <param name="dbContextProvider">Database context provider</param>
        public EfCoreResourceInstanceRepository(
            IDbContextProvider<RIMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Includes details (allocation records, usage records) when fetching resource instances
        /// </summary>
        /// <param name="includeDetails">Whether to include details</param>
        /// <returns>Queryable with optional details</returns>
        public override async Task<IQueryable<ResourceInstanceAggregate>> WithDetailsAsync(bool includeDetails = true)
        {
            if (!includeDetails)
            {
                return await base.WithDetailsAsync(includeDetails);
            }

            return (await GetQueryableAsync())
                .Include(ri => ri.AllocationRecords)
                .Include(ri => ri.UsageRecords);
        }

        /// <summary>
        /// Finds a resource instance by its identifier
        /// </summary>
        /// <param name="identifier">Resource instance identifier</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Resource instance or null if not found</returns>
        public async Task<ResourceInstanceAggregate> FindByIdentifierAsync(
            string identifier,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .FirstOrDefaultAsync(ri => ri.Identifier == identifier, cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource instances by resource type
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> GetListByResourceTypeAsync(
            Guid resourceTypeId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ri => ri.ResourceTypeId == resourceTypeId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource instances by status
        /// </summary>
        /// <param name="status">Resource status</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> GetListByStatusAsync(
            ResourceStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ri => ri.Status == status)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource instances for multiple resource types
        /// </summary>
        /// <param name="resourceTypeIds">List of resource type IDs</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> GetListByResourceTypesAsync(
            List<Guid> resourceTypeIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            if (resourceTypeIds == null || resourceTypeIds.Count == 0)
            {
                return new List<ResourceInstanceAggregate>();
            }

            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ri => resourceTypeIds.Contains(ri.ResourceTypeId))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource instances assigned to a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> GetListByUserIdAsync(
            Guid userId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ri => ri.UserId != null && EF.Property<Guid>(ri.UserId, "Value") == userId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of resource instances by a list of IDs
        /// </summary>
        /// <param name="ids">List of resource instance IDs</param>
        /// <param name="includeDetails">Whether to include details (allocation records, usage records)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> GetListAsync(
            List<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<ResourceInstanceAggregate>();
            }

            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(ri => ids.Contains(ri.Id))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Finds available resources matching specified criteria
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID (optional)</param>
        /// <param name="startTime">Availability start time</param>
        /// <param name="endTime">Availability end time</param>
        /// <param name="attributes">Required attributes (optional)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of available resource instances</returns>
        public async Task<List<ResourceInstanceAggregate>> FindAvailableResourcesAsync(
            Guid? resourceTypeId,
            DateTime startTime,
            DateTime endTime,
            Dictionary<string, string> attributes = null,
            CancellationToken cancellationToken = default)
        {
            // Get all resources that are not retired or unavailable
            var query = await WithDetailsAsync(true);
            query = query.Where(ri => ri.Status != ResourceStatus.Retired && ri.Status != ResourceStatus.Unavailable);

            // Filter by resource type if specified
            if (resourceTypeId.HasValue)
            {
                query = query.Where(ri => ri.ResourceTypeId == resourceTypeId.Value);
            }

            // Get all resources that match the basic criteria
            var resources = await query.ToListAsync(cancellationToken);

            // Further filter in memory for complex criteria
            var result = resources.Where(ri => 
            {
                // Check if resource is available for the time period
                // For available resources, no allocation check is needed
                if (ri.Status == ResourceStatus.Available)
                {
                    return true;
                }

                // For allocated resources, check if the allocation ends before the requested start time
                if (ri.Status == ResourceStatus.Allocated)
                {
                    var currentAllocation = ri.CurrentAllocation;
                    if (currentAllocation == null)
                    {
                        return true; // No current allocation despite status
                    }

                    // If allocation has no end time, it's open-ended and not available
                    if (!currentAllocation.EndTime.HasValue)
                    {
                        return false;
                    }

                    // Check if allocation end time is before the requested start time
                    return currentAllocation.EndTime.Value <= startTime;
                }

                return false;
            }).ToList();

            // Filter by attributes if specified
            if (attributes != null && attributes.Count > 0)
            {
                result = result.Where(ri => 
                {
                    foreach (var attribute in attributes)
                    {
                        var attributeValue = ri.GetAttributeValue(attribute.Key);
                        if (attributeValue == null || attributeValue.StringValue != attribute.Value)
                        {
                            return false;
                        }
                    }
                    return true;
                }).ToList();
            }

            return result;
        }

        /// <summary>
        /// Updates multiple resource instances in a batch
        /// </summary>
        /// <param name="resources">List of resource instances to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task UpdateManyAsync(
            IEnumerable<ResourceInstanceAggregate> resources,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            foreach (var resource in resources)
            {
                var entry = dbContext.Entry(resource);
                if (entry.State == EntityState.Detached)
                {
                    dbContext.Update(resource);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
