using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services
{
    /// <summary>
    /// Implementation of the organization management domain service
    /// </summary>
    public class OrganizationManager : DomainService, IOrganizationManager
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<OrganizationManager> _logger;

        public OrganizationManager(
            IOrganizationRepository organizationRepository,
            IContactRepository contactRepository,
            IGuidGenerator guidGenerator,
            ILogger<OrganizationManager> logger)
        {
            _organizationRepository = organizationRepository;
            _contactRepository = contactRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task<Organization> CreateAsync(
            string name,
            ContactInfo contactInfo,
            OrganizationInfo details,
            string registrationNumber = null,
            Guid? parentOrganizationId = null)
        {
            // Check for duplicate name
            if (await _organizationRepository.ExistsByNameAsync(name))
            {
                throw new BusinessException("CRM:ErrorMessage:DuplicateOrganizationName")
                    .WithData("name", name);
            }

            // Check parent organization exists if specified
            if (parentOrganizationId.HasValue)
            {
                await _organizationRepository.GetAsync(parentOrganizationId.Value);
            }

            var organization = Organization.Create(
                _guidGenerator.Create(),
                name,
                contactInfo,
                details,
                registrationNumber,
                parentOrganizationId,
                CurrentTenant.Id);

            return await _organizationRepository.InsertAsync(organization, autoSave: true);
        }

        public async Task<Organization> UpdateAsync(
            Guid id,
            string name = null,
            ContactInfo contactInfo = null,
            OrganizationInfo details = null,
            string registrationNumber = null)
        {
            var organization = await _organizationRepository.GetAsync(id);

            if (name != null && name != organization.Name)
            {
                // Check for duplicate name if changed
                if (await _organizationRepository.ExistsByNameAsync(name, id))
                {
                    throw new BusinessException("CRM:ErrorMessage:DuplicateOrganizationName")
                        .WithData("name", name);
                }

                organization.UpdateName(name);
            }

            if (contactInfo != null)
            {
                organization.UpdateContactInfo(contactInfo);
            }

            if (details != null)
            {
                organization.UpdateDetails(details);
            }

            if (registrationNumber != null)
            {
                organization.UpdateRegistrationNumber(registrationNumber);
            }

            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<Organization> SetParentOrganizationAsync(
            Guid id,
            Guid parentId)
        {
            // Check if organization exists
            var organization = await _organizationRepository.GetAsync(id);
            
            // Check if parent organization exists
            await _organizationRepository.GetAsync(parentId);
            
            // Check for circular reference
            if (id == parentId)
            {
                throw new BusinessException("CRM:ErrorMessage:CircularOrganizationReference")
                    .WithData("id", id);
            }
            
            // TODO: For a complete implementation, we should check for deep circular references
            // by traversing up the parent chain, but that's beyond the scope of this demo

            organization.SetParentOrganization(parentId);
            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<Organization> RemoveParentOrganizationAsync(
            Guid id)
        {
            var organization = await _organizationRepository.GetAsync(id);
            organization.RemoveParentOrganization();
            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<Organization> AddTagAsync(
            Guid id,
            string tag)
        {
            var organization = await _organizationRepository.GetAsync(id);
            organization.AddTag(tag);
            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<Organization> RemoveTagAsync(
            Guid id,
            string tag)
        {
            var organization = await _organizationRepository.GetAsync(id);
            organization.RemoveTag(tag);
            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<Organization> ActivateAsync(
            Guid id)
        {
            var organization = await _organizationRepository.GetAsync(id);
            organization.Activate();
            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<Organization> DeactivateAsync(
            Guid id)
        {
            var organization = await _organizationRepository.GetAsync(id);
            organization.Deactivate();
            return await _organizationRepository.UpdateAsync(organization, autoSave: true);
        }

        public async Task<List<Contact>> GetContactsAsync(
            Guid id,
            bool primaryOnly = false,
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null)
        {
            // Check if organization exists
            await _organizationRepository.GetAsync(id);
            
            // Get contacts for this organization
            return await _contactRepository.GetByOrganizationAsync(
                id,
                primaryOnly,
                skipCount,
                maxResultCount,
                sorting);
        }
    }
}
