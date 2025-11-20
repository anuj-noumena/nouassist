using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IStorageLocationRepository
    /// </summary>
    public class EfCoreStorageLocationRepository 
        : EfCoreRepository<RIMDbContext, StorageLocationAggregate, Guid>, IStorageLocationRepository
    {
        /// <summary>
        /// Creates a new instance of EfCoreStorageLocationRepository
        /// </summary>
        /// <param name="dbContextProvider">Database context provider</param>
        public EfCoreStorageLocationRepository(
            IDbContextProvider<RIMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Includes details (child locations) when fetching storage locations
        /// </summary>
        /// <param name="includeDetails">Whether to include details</param>
        /// <returns>Queryable with optional details</returns>
        public override async Task<IQueryable<StorageLocationAggregate>> WithDetailsAsync(bool includeDetails = true)
        {
            if (!includeDetails)
            {
                return await base.WithDetailsAsync(includeDetails);
            }

            return (await GetQueryableAsync()).Include(sl => sl.ChildLocations);
        }

        /// <summary>
        /// Finds a storage location by its code
        /// </summary>
        /// <param name="code">Location code</param>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Storage location or null if not found</returns>
        public async Task<StorageLocationAggregate> FindByCodeAsync(
            string code,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .FirstOrDefaultAsync(sl => sl.Code == code, cancellationToken);
        }

        /// <summary>
        /// Checks if a storage location with the specified code exists
        /// </summary>
        /// <param name="code">Location code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        public async Task<bool> ExistsByCodeAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .AnyAsync(sl => sl.Code == code, cancellationToken);
        }

        /// <summary>
        /// Gets a list of storage locations by location type
        /// </summary>
        /// <param name="locationType">Location type</param>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of storage locations</returns>
        public async Task<List<StorageLocationAggregate>> GetListByLocationTypeAsync(
            string locationType,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(sl => sl.LocationType == locationType)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of active storage locations
        /// </summary>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of active storage locations</returns>
        public async Task<List<StorageLocationAggregate>> GetActiveListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(sl => sl.IsActive)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of child locations for a parent location
        /// </summary>
        /// <param name="parentLocationId">Parent location ID</param>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of child storage locations</returns>
        public async Task<List<StorageLocationAggregate>> GetChildLocationsAsync(
            Guid parentLocationId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(sl => sl.ParentLocationId == parentLocationId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of root locations (with no parent)
        /// </summary>
        /// <param name="includeDetails">Whether to include details (child locations)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of root storage locations</returns>
        public async Task<List<StorageLocationAggregate>> GetRootLocationsAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync(includeDetails);
            return await query
                .Where(sl => sl.ParentLocationId == null)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the full location hierarchy as a nested structure
        /// </summary>
        /// <param name="rootLocationId">Root location ID (optional, if not specified returns entire hierarchy)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Hierarchical structure of locations</returns>
        public async Task<List<LocationHierarchyDto>> GetLocationHierarchyAsync(
            Guid? rootLocationId = null,
            CancellationToken cancellationToken = default)
        {
            // Get all locations with child relationships
            var query = await WithDetailsAsync(true);
            List<StorageLocationAggregate> allLocations;

            if (rootLocationId.HasValue)
            {
                // Get the root location and all its descendants
                var rootLocation = await query
                    .FirstOrDefaultAsync(sl => sl.Id == rootLocationId.Value, cancellationToken);

                if (rootLocation == null)
                {
                    return new List<LocationHierarchyDto>();
                }

                // Get all locations with their parent-child relationships
                allLocations = await query
                    .Where(sl => sl.Id == rootLocationId.Value || 
                                 sl.ParentLocationId == rootLocationId.Value ||
                                 query.Any(parent => parent.Id == sl.ParentLocationId && 
                                                   (parent.Id == rootLocationId.Value || 
                                                    parent.ParentLocationId == rootLocationId.Value)))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                // Get all locations
                allLocations = await query.ToListAsync(cancellationToken);
            }

            // Build the hierarchy
            var locationDtoMap = allLocations.ToDictionary(
                l => l.Id,
                l => new LocationHierarchyDto
                {
                    Id = l.Id,
                    Code = l.Code,
                    Name = l.Name,
                    LocationType = l.LocationType,
                    Children = new List<LocationHierarchyDto>()
                });

            // Add child locations to their parents
            foreach (var location in allLocations.Where(l => l.ParentLocationId.HasValue))
            {
                if (locationDtoMap.TryGetValue(location.ParentLocationId.Value, out var parentDto))
                {
                    parentDto.Children.Add(locationDtoMap[location.Id]);
                }
            }

            // Return only root locations (either the specified root or all roots)
            if (rootLocationId.HasValue)
            {
                return new List<LocationHierarchyDto> { locationDtoMap[rootLocationId.Value] };
            }
            else
            {
                return locationDtoMap.Values
                    .Where(dto => !allLocations.Any(l => l.Id == dto.Id && l.ParentLocationId.HasValue))
                    .ToList();
            }
        }
    }
}
