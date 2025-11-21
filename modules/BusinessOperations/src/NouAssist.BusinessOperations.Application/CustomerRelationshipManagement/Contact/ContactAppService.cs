using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Localization;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Permissions;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact
{
    [Authorize(CRMPermissions.Contact.Default)]
    public class ContactAppService : ApplicationService, IContactAppService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactManager _contactManager;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILogger<ContactAppService> _logger;

        public ContactAppService(
            IContactRepository contactRepository,
            IContactManager contactManager,
            IOrganizationRepository organizationRepository,
            ILogger<ContactAppService> logger)
        {
            _contactRepository = contactRepository;
            _contactManager = contactManager;
            _organizationRepository = organizationRepository;
            _logger = logger;
            
            LocalizationResource = typeof(CRMResource);
        }

        [Authorize(CRMPermissions.Contact.View)]
        public virtual async Task<ContactDto> GetAsync(Guid id)
        {
            try
            {
                var contact = await _contactRepository.GetAsync(id);
                
                var dto = ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
                
                // Populate organization name if organization is assigned
                if (contact.OrganizationId.HasValue)
                {
                    try
                    {
                        var organization = await _organizationRepository.GetAsync(contact.OrganizationId.Value);
                        dto.OrganizationName = organization.Name;
                    }
                    catch (Exception)
                    {
                        // Organization might be deleted, just continue without the name
                    }
                }
                
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.View)]
        public virtual async Task<PagedResultDto<ContactDto>> GetListAsync(GetContactListDto input)
        {
            try
            {
                // Get queryable and apply filters
                var queryable = await _contactRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(c => 
                        c.Name.Contains(input.Filter) || 
                        EF.Property<string>(c.ContactInfo, "Email").Contains(input.Filter) ||
                        c.JobTitle.Contains(input.Filter));
                }
                
                if (input.OrganizationId.HasValue)
                {
                    queryable = queryable.Where(c => c.OrganizationId == input.OrganizationId.Value);
                    
                    if (input.PrimaryOnly.HasValue && input.PrimaryOnly.Value)
                    {
                        queryable = queryable.Where(c => c.IsPrimaryContact);
                    }
                }
                
                if (input.IsActive.HasValue)
                {
                    queryable = queryable.Where(c => c.IsActive == input.IsActive.Value);
                }
                
                if (!string.IsNullOrWhiteSpace(input.Tags))
                {
                    var tags = input.Tags.Split(',').Select(t => t.Trim()).ToList();
                    foreach (var tag in tags)
                    {
                        queryable = queryable.Where(c => c.Tags.Contains(tag));
                    }
                }
                
                if (input.GroupId.HasValue)
                {
                    queryable = queryable.Where(c => c.GroupIds.Contains(input.GroupId.Value));
                }
                
                if (input.CreatedFrom.HasValue)
                {
                    queryable = queryable.Where(c => c.CreationTime >= input.CreatedFrom.Value);
                }
                
                if (input.CreatedTo.HasValue)
                {
                    queryable = queryable.Where(c => c.CreationTime <= input.CreatedTo.Value);
                }
                
                // Get count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                var contacts = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? "Name")
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount));
                
                // Map to DTOs
                var contactDtos = ObjectMapper.Map<List<Aggregates.Contact>, List<ContactDto>>(contacts);
                
                // Populate organization names
                if (contactDtos.Any(c => c.OrganizationId.HasValue))
                {
                    var orgIds = contactDtos.Where(c => c.OrganizationId.HasValue)
                        .Select(c => c.OrganizationId!.Value)
                        .Distinct()
                        .ToList();
                        
                    var organizations = await _organizationRepository.GetListAsync(o => orgIds.Contains(o.Id));
                    var orgDict = organizations.ToDictionary(o => o.Id, o => o.Name);
                    
                    foreach (var contactDto in contactDtos.Where(c => c.OrganizationId.HasValue))
                    {
                        if (orgDict.TryGetValue(contactDto.OrganizationId!.Value, out var orgName))
                        {
                            contactDto.OrganizationName = orgName;
                        }
                    }
                }
                
                return new PagedResultDto<ContactDto>
                {
                    TotalCount = totalCount,
                    Items = contactDtos
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Create)]
        public virtual async Task<ContactDto> CreateAsync(CreateContactDto input)
        {
            try
            {
                // Map DTOs to value objects
                var contactInfo = new ContactInfo(
                    input.ContactInfo.Email,
                    input.ContactInfo.Phone,
                    input.ContactInfo.AlternativePhone,
                    input.ContactInfo.Website,
                    input.ContactInfo.Address,
                    input.ContactInfo.City,
                    input.ContactInfo.State,
                    input.ContactInfo.PostalCode,
                    input.ContactInfo.Country);
                
                // Create contact using domain service
                var contact = await _contactManager.CreateAsync(
                    input.Name,
                    contactInfo,
                    input.JobTitle,
                    input.OrganizationId,
                    input.IsPrimaryContact,
                    input.Notes);
                
                // Add tags if provided
                if (input.Tags != null && input.Tags.Any())
                {
                    foreach (var tag in input.Tags)
                    {
                        await _contactManager.AddTagAsync(contact.Id, tag);
                    }
                    
                    // Refresh contact to get updated tags
                    contact = await _contactRepository.GetAsync(contact.Id);
                }
                
                // Map to DTO
                var contactDto = ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
                
                // Add organization name if available
                if (contact.OrganizationId.HasValue)
                {
                    try
                    {
                        var organization = await _organizationRepository.GetAsync(contact.OrganizationId.Value);
                        contactDto.OrganizationName = organization.Name;
                    }
                    catch (Exception)
                    {
                        // Organization might not be found, just continue without the name
                    }
                }
                
                return contactDto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> UpdateAsync(Guid id, UpdateContactDto input)
        {
            try
            {
                var contact = await _contactRepository.GetAsync(id);
                
                // Update contact
                if (!string.IsNullOrWhiteSpace(input.Name))
                {
                    contact.UpdateName(input.Name);
                }
                
                if (!string.IsNullOrWhiteSpace(input.JobTitle))
                {
                    contact.UpdateJobTitle(input.JobTitle);
                }
                
                if (!string.IsNullOrWhiteSpace(input.Notes))
                {
                    contact.UpdateNotes(input.Notes);
                }
                
                // Update contact info if provided
                if (input.ContactInfo != null)
                {
                    ContactInfo contactInfo = null;
                    
                    // Only create a new ContactInfo if at least one property is provided
                    if (!string.IsNullOrWhiteSpace(input.ContactInfo.Email) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Phone) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.AlternativePhone) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Website) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Address) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.City) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.State) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.PostalCode) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Country))
                    {
                        // For fields not provided, use existing values
                        contactInfo = new ContactInfo(
                            input.ContactInfo.Email ?? contact.ContactInfo.Email,
                            input.ContactInfo.Phone ?? contact.ContactInfo.Phone,
                            input.ContactInfo.AlternativePhone ?? contact.ContactInfo.AlternativePhone,
                            input.ContactInfo.Website ?? contact.ContactInfo.Website,
                            input.ContactInfo.Address ?? contact.ContactInfo.Address,
                            input.ContactInfo.City ?? contact.ContactInfo.City,
                            input.ContactInfo.State ?? contact.ContactInfo.State,
                            input.ContactInfo.PostalCode ?? contact.ContactInfo.PostalCode,
                            input.ContactInfo.Country ?? contact.ContactInfo.Country);
                        
                        contact.UpdateContactInfo(contactInfo);
                    }
                }
                
                // Save the updated contact
                await _contactRepository.UpdateAsync(contact);
                
                // Map to DTO
                var contactDto = ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
                
                // Add organization name if available
                if (contact.OrganizationId.HasValue)
                {
                    try
                    {
                        var organization = await _organizationRepository.GetAsync(contact.OrganizationId.Value);
                        contactDto.OrganizationName = organization.Name;
                    }
                    catch (Exception)
                    {
                        // Organization might not be found, just continue without the name
                    }
                }
                
                return contactDto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _contactRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> AssignToOrganizationAsync(Guid id, AssignToOrganizationDto input)
        {
            try
            {
                // Check if organization exists
                await _organizationRepository.GetAsync(input.OrganizationId);
                
                // Assign contact to organization
                var contact = await _contactManager.AssignToOrganizationAsync(
                    id,
                    input.OrganizationId,
                    input.IsPrimaryContact);
                
                // Map to DTO
                var contactDto = ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
                
                // Add organization name
                try
                {
                    var organization = await _organizationRepository.GetAsync(input.OrganizationId);
                    contactDto.OrganizationName = organization.Name;
                }
                catch (Exception)
                {
                    // Organization might not be found, just continue without the name
                }
                
                return contactDto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact or Organization"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> RemoveFromOrganizationAsync(Guid id)
        {
            try
            {
                var contact = await _contactManager.RemoveFromOrganizationAsync(id);
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> AddTagsAsync(Guid id, ContactTagsDto input)
        {
            try
            {
                var contact = await _contactRepository.GetAsync(id);
                
                foreach (var tag in input.Tags)
                {
                    await _contactManager.AddTagAsync(id, tag);
                }
                
                // Refresh contact to get updated tags
                contact = await _contactRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> RemoveTagsAsync(Guid id, ContactTagsDto input)
        {
            try
            {
                var contact = await _contactRepository.GetAsync(id);
                
                foreach (var tag in input.Tags)
                {
                    await _contactManager.RemoveTagAsync(id, tag);
                }
                
                // Refresh contact to get updated tags
                contact = await _contactRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.ManageGroups)]
        public virtual async Task<ContactDto> AddToGroupsAsync(Guid id, ContactGroupsDto input)
        {
            try
            {
                var contact = await _contactRepository.GetAsync(id);
                
                foreach (var groupId in input.GroupIds)
                {
                    await _contactManager.AddToGroupAsync(id, groupId);
                }
                
                // Refresh contact to get updated group IDs
                contact = await _contactRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.ManageGroups)]
        public virtual async Task<ContactDto> RemoveFromGroupsAsync(Guid id, ContactGroupsDto input)
        {
            try
            {
                var contact = await _contactRepository.GetAsync(id);
                
                foreach (var groupId in input.GroupIds)
                {
                    await _contactManager.RemoveFromGroupAsync(id, groupId);
                }
                
                // Refresh contact to get updated group IDs
                contact = await _contactRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> ActivateAsync(Guid id)
        {
            try
            {
                var contact = await _contactManager.ActivateAsync(id);
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Contact.Update)]
        public virtual async Task<ContactDto> DeactivateAsync(Guid id)
        {
            try
            {
                var contact = await _contactManager.DeactivateAsync(id);
                return ObjectMapper.Map<Aggregates.Contact, ContactDto>(contact);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Contact"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in ContactAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
