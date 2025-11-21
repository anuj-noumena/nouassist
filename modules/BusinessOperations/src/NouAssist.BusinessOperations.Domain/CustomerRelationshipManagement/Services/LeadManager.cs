using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services
{
    /// <summary>
    /// Implementation of the lead management domain service
    /// </summary>
    public class LeadManager : DomainService, ILeadManager
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<LeadManager> _logger;

        public LeadManager(
            ILeadRepository leadRepository,
            IContactRepository contactRepository,
            IGuidGenerator guidGenerator,
            ILogger<LeadManager> logger)
        {
            _leadRepository = leadRepository;
            _contactRepository = contactRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task<Lead> CreateAsync(
            string name,
            ContactInfo contactInfo,
            LeadDetails details,
            LeadStatus status = LeadStatus.New,
            int score = 0)
        {
            // Check for duplicate email
            if (await _leadRepository.ExistsByEmailAsync(contactInfo.Email))
            {
                throw new DuplicateEmailException(contactInfo.Email, "Lead");
            }

            var lead = Lead.Create(
                _guidGenerator.Create(),
                name,
                contactInfo,
                details,
                status,
                score,
                CurrentTenant.Id);

            // Set source record
            lead.SetSourceRecord(
                details.Source,
                Clock.Now,
                details.CampaignId,
                null, // UTM params would come from application service
                null,
                null,
                null,
                null,
                null,
                null);

            return await _leadRepository.InsertAsync(lead, autoSave: true);
        }

        public async Task<Lead> UpdateAsync(
            Guid id,
            string name = null,
            ContactInfo contactInfo = null,
            LeadDetails details = null)
        {
            var lead = await _leadRepository.GetAsync(id);

            if (name != null)
            {
                lead.UpdateName(name);
            }

            if (contactInfo != null)
            {
                // Check for duplicate email if changed
                if (contactInfo.Email != lead.ContactInfo.Email &&
                    await _leadRepository.ExistsByEmailAsync(contactInfo.Email, id))
                {
                    throw new DuplicateEmailException(contactInfo.Email, "Lead");
                }

                lead.UpdateContactInfo(contactInfo);
            }

            if (details != null)
            {
                lead.UpdateDetails(details);
            }

            return await _leadRepository.UpdateAsync(lead, autoSave: true);
        }

        public async Task<Lead> ChangeStatusAsync(
            Guid id,
            LeadStatus newStatus)
        {
            var lead = await _leadRepository.GetAsync(id);
            lead.ChangeStatus(newStatus);
            return await _leadRepository.UpdateAsync(lead, autoSave: true);
        }

        public async Task<Lead> UpdateScoreAsync(
            Guid id,
            int score)
        {
            var lead = await _leadRepository.GetAsync(id);
            lead.UpdateScore(score);
            return await _leadRepository.UpdateAsync(lead, autoSave: true);
        }

        public async Task<(Lead Lead, Contact Contact)> ConvertToContactAsync(
            Guid id,
            Guid? organizationId = null,
            bool isPrimaryContact = false)
        {
            var lead = await _leadRepository.GetAsync(id);

            if (lead.Status == LeadStatus.Converted)
            {
                throw new LeadAlreadyConvertedException(id);
            }

            // Create a new contact based on the lead
            var contactId = _guidGenerator.Create();
            var contact = Contact.CreateFromLead(
                contactId,
                lead,
                organizationId,
                isPrimaryContact);

            // Mark the lead as converted
            lead.MarkAsConverted(contactId, Clock.Now);

            // Save both entities
            await _contactRepository.InsertAsync(contact, autoSave: true);
            await _leadRepository.UpdateAsync(lead, autoSave: true);

            _logger.LogInformation("Lead {LeadId} converted to contact {ContactId}", id, contactId);

            return (lead, contact);
        }

        public async Task<Lead> AddTagAsync(
            Guid id,
            string tag)
        {
            var lead = await _leadRepository.GetAsync(id);
            lead.AddTag(tag);
            return await _leadRepository.UpdateAsync(lead, autoSave: true);
        }

        public async Task<Lead> RemoveTagAsync(
            Guid id,
            string tag)
        {
            var lead = await _leadRepository.GetAsync(id);
            lead.RemoveTag(tag);
            return await _leadRepository.UpdateAsync(lead, autoSave: true);
        }
    }
}
