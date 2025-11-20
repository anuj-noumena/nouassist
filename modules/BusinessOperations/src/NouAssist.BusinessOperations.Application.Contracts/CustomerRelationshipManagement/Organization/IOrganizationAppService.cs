using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Organization.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Organization
{
    /// <summary>
    /// Application service for organization management
    /// </summary>
    public interface IOrganizationAppService : IApplicationService
    {
        /// <summary>
        /// Gets an organization by ID
        /// </summary>
        Task<OrganizationDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of organizations with filtering and paging
        /// </summary>
        Task<PagedResultDto<OrganizationDto>> GetListAsync(GetOrganizationListDto input);
        
        /// <summary>
        /// Creates a new organization
        /// </summary>
        Task<OrganizationDto> CreateAsync(CreateOrganizationDto input);
        
        /// <summary>
        /// Updates an existing organization
        /// </summary>
        Task<OrganizationDto> UpdateAsync(Guid id, UpdateOrganizationDto input);
        
        /// <summary>
        /// Deletes an organization
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Sets the parent organization
        /// </summary>
        Task<OrganizationDto> SetParentOrganizationAsync(Guid id, SetParentOrganizationDto input);
        
        /// <summary>
        /// Removes the parent organization
        /// </summary>
        Task<OrganizationDto> RemoveParentOrganizationAsync(Guid id);
        
        /// <summary>
        /// Adds tags to an organization
        /// </summary>
        Task<OrganizationDto> AddTagsAsync(Guid id, OrganizationTagsDto input);
        
        /// <summary>
        /// Removes tags from an organization
        /// </summary>
        Task<OrganizationDto> RemoveTagsAsync(Guid id, OrganizationTagsDto input);
        
        /// <summary>
        /// Activates an organization
        /// </summary>
        Task<OrganizationDto> ActivateAsync(Guid id);
        
        /// <summary>
        /// Deactivates an organization
        /// </summary>
        Task<OrganizationDto> DeactivateAsync(Guid id);
        
        /// <summary>
        /// Gets contacts of an organization
        /// </summary>
        Task<PagedResultDto<ContactDto>> GetContactsAsync(Guid id, bool primaryOnly = false, int skipCount = 0, int maxResultCount = 10, string sorting = null);
    }
}
