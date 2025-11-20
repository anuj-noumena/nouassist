using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation
{
    /// <summary>
    /// Application service interface for managing storage locations
    /// </summary>
    public interface IStorageLocationAppService : IApplicationService
    {
        /// <summary>
        /// Gets a storage location by ID
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <returns>Storage location DTO</returns>
        Task<StorageLocationDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a storage location by code
        /// </summary>
        /// <param name="code">Location code</param>
        /// <returns>Storage location DTO</returns>
        Task<StorageLocationDto> GetByCodeAsync(string code);

        /// <summary>
        /// Gets a paged and filtered list of storage locations
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged result of storage location DTOs</returns>
        Task<PagedResultDto<StorageLocationDto>> GetListAsync(GetStorageLocationListDto input);

        /// <summary>
        /// Creates a new storage location
        /// </summary>
        /// <param name="input">Storage location creation data</param>
        /// <returns>Created storage location DTO</returns>
        Task<StorageLocationDto> CreateAsync(CreateStorageLocationDto input);

        /// <summary>
        /// Updates an existing storage location
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <param name="input">Storage location update data</param>
        /// <returns>Updated storage location DTO</returns>
        Task<StorageLocationDto> UpdateAsync(Guid id, UpdateStorageLocationDto input);

        /// <summary>
        /// Deletes a storage location
        /// </summary>
        /// <param name="id">Storage location ID</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Activates a storage location
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <returns>Updated storage location DTO</returns>
        Task<StorageLocationDto> ActivateAsync(Guid id);

        /// <summary>
        /// Deactivates a storage location
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <returns>Updated storage location DTO</returns>
        Task<StorageLocationDto> DeactivateAsync(Guid id);

        /// <summary>
        /// Changes the parent location
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <param name="input">Parent change data</param>
        /// <returns>Updated storage location DTO</returns>
        Task<StorageLocationDto> ChangeParentAsync(Guid id, ChangeParentDto input);

        /// <summary>
        /// Updates the capacity information
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <param name="input">Capacity update data</param>
        /// <returns>Updated storage location DTO</returns>
        Task<StorageLocationDto> UpdateCapacityAsync(Guid id, UpdateCapacityDto input);

        /// <summary>
        /// Gets a list of child locations for a parent location
        /// </summary>
        /// <param name="parentId">Parent location ID</param>
        /// <returns>List of child storage locations</returns>
        Task<ListResultDto<StorageLocationDto>> GetChildLocationsAsync(Guid parentId);

        /// <summary>
        /// Gets a list of root locations (with no parent)
        /// </summary>
        /// <returns>List of root storage locations</returns>
        Task<ListResultDto<StorageLocationDto>> GetRootLocationsAsync();

        /// <summary>
        /// Gets the full location hierarchy as a tree
        /// </summary>
        /// <param name="rootId">Root location ID (optional, if not specified returns entire hierarchy)</param>
        /// <returns>Tree of locations</returns>
        Task<List<LocationTreeNodeDto>> GetLocationHierarchyAsync(Guid? rootId = null);

        /// <summary>
        /// Gets a list of all location types in use
        /// </summary>
        /// <returns>List of location types</returns>
        Task<ListResultDto<string>> GetLocationTypesAsync();
    }
}
