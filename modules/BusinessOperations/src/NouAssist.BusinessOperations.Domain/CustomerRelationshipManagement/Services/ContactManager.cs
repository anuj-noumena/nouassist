using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services
{
    /// <summary>
    /// Implementation of the contact management domain service
    /// </summary>
    public class ContactManager : DomainService, IContactManager
    {
        private readonly IContactRepository _contactRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ContactManager> _logger;

        public ContactManager(
            IContactRepository contactRepository,
            IOrganizationRepository organizationRepository,
            IGuidGenerator guidGenerator,
            ILogger<ContactManager> logger)
        {
            _contactRepository = contactRepository;
            _organizationRepository = organizationRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task<Contact> CreateAsync(
            string name,
            ContactInfo contactInfo,
            string jobTitle = null,
            Guid? organizationId = null,
            bool isPrimaryContact = false,
            string notes = null)
        {
            // Check for duplicate email
            if (await _contactRepository.ExistsByEmailAsync(contactInfo.Email))
            {
                throw new DuplicateEmailException(contactInfo.Email, "Contact");
            }

            // Check organization exists if specified
            if (organizationId.HasValue)
            {
                await _organizationRepository.GetAsync(organizationId.Value);
            }

            var contact = Contact.Create(
                _guidGenerator.Create(),
                name,
                contactInfo,
                jobTitle,
                organizationId,
                isPrimaryContact,
                null, // Not converted from lead
                notes,
                CurrentTenant.Id);

            return await _contactRepository.InsertAsync(contact, autoSave: true);
        }

        public async Task<Contact> UpdateAsync(
            Guid id,
            string name = null,
            ContactInfo contactInfo = null,
            string jobTitle = null,
            string notes = null)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (name != null)
            {
                contact.UpdateName(name);
            }

            if (contactInfo != null)
            {
                // Check for duplicate email if changed
                if (contactInfo.Email != contact.ContactInfo.Email &&
                    await _contactRepository.ExistsByEmailAsync(contactInfo.Email, id))
                {
                    throw new DuplicateEmailException(contactInfo.Email, "Contact");
                }

                contact.UpdateContactInfo(contactInfo);
            }

            if (jobTitle != null)
            {
                contact.UpdateJobTitle(jobTitle);
            }

            if (notes != null)
            {
                contact.UpdateNotes(notes);
            }

            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> AssignToOrganizationAsync(
            Guid id,
            Guid organizationId,
            bool isPrimaryContact = false)
        {
            var contact = await _contactRepository.GetAsync(id);
            
            // Check organization exists
            await _organizationRepository.GetAsync(organizationId);
            
            contact.SetOrganization(organizationId, isPrimaryContact);
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> RemoveFromOrganizationAsync(
            Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.RemoveFromOrganization();
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> AddToGroupAsync(
            Guid id,
            Guid groupId)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.AddToGroup(groupId);
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> RemoveFromGroupAsync(
            Guid id,
            Guid groupId)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.RemoveFromGroup(groupId);
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> AddTagAsync(
            Guid id,
            string tag)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.AddTag(tag);
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> RemoveTagAsync(
            Guid id,
            string tag)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.RemoveTag(tag);
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> ActivateAsync(
            Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.Activate();
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }

        public async Task<Contact> DeactivateAsync(
            Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);
            contact.Deactivate();
            return await _contactRepository.UpdateAsync(contact, autoSave: true);
        }
    }
}
