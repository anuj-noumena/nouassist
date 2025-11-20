using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType
{
    /// <summary>
    /// Application service interface for managing resource types
    /// </summary>
    public interface IResourceTypeAppService : IApplicationService
    {
        /// <summary>
        /// Gets a resource type by ID
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <returns>Resource type DTO</returns>
        Task<ResourceTypeDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a resource type by code
        /// </summary>
        /// <param name="code">Resource type code</param>
        /// <returns>Resource type DTO</returns>
        Task<ResourceTypeDto> GetByCodeAsync(string code);

        /// <summary>
        /// Gets a paged and filtered list of resource types
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged result of resource type DTOs</returns>
        Task<PagedResultDto<ResourceTypeDto>> GetListAsync(GetResourceTypeListDto input);

        /// <summary>
        /// Creates a new resource type
        /// </summary>
        /// <param name="input">Resource type creation data</param>
        /// <returns>Created resource type DTO</returns>
        Task<ResourceTypeDto> CreateAsync(CreateResourceTypeDto input);

        /// <summary>
        /// Updates an existing resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <param name="input">Resource type update data</param>
        /// <returns>Updated resource type DTO</returns>
        Task<ResourceTypeDto> UpdateAsync(Guid id, UpdateResourceTypeDto input);

        /// <summary>
        /// Deletes a resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Activates a resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <returns>Updated resource type DTO</returns>
        Task<ResourceTypeDto> ActivateAsync(Guid id);

        /// <summary>
        /// Deactivates a resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <returns>Updated resource type DTO</returns>
        Task<ResourceTypeDto> DeactivateAsync(Guid id);

        /// <summary>
        /// Adds an attribute definition to a resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <param name="input">Attribute definition data</param>
        /// <returns>Updated resource type DTO</returns>
        Task<ResourceTypeDto> AddAttributeDefinitionAsync(Guid id, CreateResourceAttributeDefinitionDto input);

        /// <summary>
        /// Updates an attribute definition
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="input">Updated attribute definition data</param>
        /// <returns>Updated resource type DTO</returns>
        Task<ResourceTypeDto> UpdateAttributeDefinitionAsync(Guid id, string attributeName, CreateResourceAttributeDefinitionDto input);

        /// <summary>
        /// Removes an attribute definition from a resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Updated resource type DTO</returns>
        Task<ResourceTypeDto> RemoveAttributeDefinitionAsync(Guid id, string attributeName);
        
        /// <summary>
        /// Gets a list of resource categories
        /// </summary>
        /// <returns>List of resource categories</returns>
        Task<List<EnumValueDto>> GetResourceCategoriesAsync();
        
        /// <summary>
        /// Gets a list of attribute data types
        /// </summary>
        /// <returns>List of attribute data types</returns>
        Task<List<EnumValueDto>> GetAttributeDataTypesAsync();
    }
}
