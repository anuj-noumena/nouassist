using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact
{
    /// <summary>
    /// Application service for contact management
    /// </summary>
    public interface IContactAppService : IApplicationService
    {
        /// <summary>
        /// Gets a contact by ID
        /// </summary>
        Task<ContactDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of contacts with filtering and paging
        /// </summary>
        Task<PagedResultDto<ContactDto>> GetListAsync(GetContactListDto input);
        
        /// <summary>
        /// Creates a new contact
        /// </summary>
        Task<ContactDto> CreateAsync(CreateContactDto input);
        
        /// <summary>
        /// Updates an existing contact
        /// </summary>
        Task<ContactDto> UpdateAsync(Guid id, UpdateContactDto input);
        
        /// <summary>
        /// Deletes a contact
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Assigns a contact to an organization
        /// </summary>
        Task<ContactDto> AssignToOrganizationAsync(Guid id, AssignToOrganizationDto input);
        
        /// <summary>
        /// Removes a contact from an organization
        /// </summary>
        Task<ContactDto> RemoveFromOrganizationAsync(Guid id);
        
        /// <summary>
        /// Adds tags to a contact
        /// </summary>
        Task<ContactDto> AddTagsAsync(Guid id, ContactTagsDto input);
        
        /// <summary>
        /// Removes tags from a contact
        /// </summary>
        Task<ContactDto> RemoveTagsAsync(Guid id, ContactTagsDto input);
        
        /// <summary>
        /// Adds a contact to groups
        /// </summary>
        Task<ContactDto> AddToGroupsAsync(Guid id, ContactGroupsDto input);
        
        /// <summary>
        /// Removes a contact from groups
        /// </summary>
        Task<ContactDto> RemoveFromGroupsAsync(Guid id, ContactGroupsDto input);
        
        /// <summary>
        /// Activates a contact
        /// </summary>
        Task<ContactDto> ActivateAsync(Guid id);
        
        /// <summary>
        /// Deactivates a contact
        /// </summary>
        Task<ContactDto> DeactivateAsync(Guid id);
    }
}
