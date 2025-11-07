using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Contacts.Aggregates;
using NouAssist.Contacts.Data;
using NouAssist.Contacts.Dtos;
using NouAssist.Contacts.Permissions;
using NouAssist.Contacts.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Contacts.Contacts
{
    [Authorize(ContactsPermissions.Contacts.Default)]
    public class ContactAppService : ApplicationService, IContactAppService
    {
        private readonly IContactRepository _contactRepository;

        public ContactAppService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ContactDto> GetAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to view this contact");
            }

            return await MapToGetOutputDtoAsync(contact);
        }

        public async Task<PagedResultDto<ContactDto>> GetListAsync(GetContactListDto input)
        {
            var ownerId = CurrentUser.GetId();
            var queryable = await _contactRepository.GetQueryableAsync();

            // Filter by owner
            queryable = queryable.Where(c => c.OwnerId == ownerId);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(c =>
                    c.FirstName.Contains(input.Filter) ||
                    c.LastName.Contains(input.Filter) ||
                    c.OrganizationName.Contains(input.Filter) ||
                    c.PrimaryEmail.Contains(input.Filter));
            }

            if (input.ContactType.HasValue)
            {
                queryable = queryable.Where(c => c.ContactType == input.ContactType.Value);
            }

            if (input.Relationship.HasValue)
            {
                queryable = queryable.Where(c => c.Relationship == input.Relationship.Value);
            }

            if (input.ClientId.HasValue)
            {
                queryable = queryable.Where(c => c.ClientId == input.ClientId.Value);
            }

            if (input.IsActive.HasValue)
            {
                queryable = queryable.Where(c => c.IsActive == input.IsActive.Value);
            }

            if (input.NeedsFollowup == true)
            {
                queryable = queryable.Where(c => c.NextFollowupDate.HasValue && c.NextFollowupDate.Value <= DateTime.UtcNow);
            }

            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                queryable = queryable.Where(c => c.Tags.Contains(input.Tag));
            }

            var totalCount = queryable.Count();

            // Apply sorting
            queryable = !string.IsNullOrWhiteSpace(input.Sorting)
                ? queryable.OrderBy(input.Sorting)
                : queryable.OrderBy(c => c.DisplayName);

            // Apply paging
            var contacts = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var contactDtos = new List<ContactDto>();
            foreach (var contact in contacts)
            {
                contactDtos.Add(await MapToGetOutputDtoAsync(contact));
            }

            return new PagedResultDto<ContactDto>(totalCount, contactDtos);
        }

        [Authorize(ContactsPermissions.Contacts.Create)]
        public async Task<ContactDto> CreateAsync(CreateContactDto input)
        {
            var ownerId = CurrentUser.GetId();

            var contact = new Contact(
                GuidGenerator.Create(),
                ownerId,
                CurrentTenant.Id,
                input.FirstName,
                input.LastName,
                input.OrganizationName,
                input.PrimaryEmail,
                input.PrimaryPhone
            );

            contact.JobTitle = input.JobTitle;
            contact.Department = input.Department;
            contact.ClientId = input.ClientId;
            contact.Website = input.Website;
            contact.Notes = input.Notes;
            contact.ContactFrequencyDays = input.ContactFrequencyDays;
            contact.Importance = input.Importance;

            if (input.ContactType.HasValue)
            {
                contact.ContactType = input.ContactType.Value;
            }

            if (input.Relationship.HasValue)
            {
                contact.Relationship = input.Relationship.Value;
            }

            await _contactRepository.InsertAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> UpdateAsync(Guid id, UpdateContactDto input)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.FirstName = input.FirstName;
            contact.LastName = input.LastName;
            contact.OrganizationName = input.OrganizationName;
            contact.JobTitle = input.JobTitle;
            contact.Department = input.Department;
            contact.ClientId = input.ClientId;
            contact.Website = input.Website;
            contact.Notes = input.Notes;
            contact.ContactFrequencyDays = input.ContactFrequencyDays;
            contact.Importance = input.Importance;

            if (input.ContactType.HasValue)
            {
                contact.ContactType = input.ContactType.Value;
            }

            if (input.Relationship.HasValue)
            {
                contact.Relationship = input.Relationship.Value;
            }

            if (!string.IsNullOrEmpty(input.PrimaryEmail))
            {
                contact.SetPrimaryEmail(input.PrimaryEmail);
            }

            if (!string.IsNullOrEmpty(input.PrimaryPhone))
            {
                contact.SetPrimaryPhone(input.PrimaryPhone);
            }

            contact.UpdateDisplayName();

            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to delete this contact");
            }

            await _contactRepository.DeleteAsync(id);
        }

        [Authorize(ContactsPermissions.Contacts.Archive)]
        public async Task<ContactDto> ArchiveAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to archive this contact");
            }

            contact.Archive();
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Archive)]
        public async Task<ContactDto> UnarchiveAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to unarchive this contact");
            }

            contact.Unarchive();
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> RecordContactAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.RecordContact();
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> RecordMeetingAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.RecordMeeting();
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> ScheduleFollowupAsync(Guid id, DateTime date)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.ScheduleFollowup(date);
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> AddTagAsync(Guid id, string tag)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.AddTag(tag);
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> RemoveTagAsync(Guid id, string tag)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.RemoveTag(tag);
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> AddSocialProfileAsync(Guid id, SocialProfileDto profileDto)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            var profile = new SocialProfile(profileDto.Platform, profileDto.Username, profileDto.ProfileUrl);
            contact.AddSocialProfile(profile);
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        [Authorize(ContactsPermissions.Contacts.Edit)]
        public async Task<ContactDto> RemoveSocialProfileAsync(Guid id, SocialProfileDto profileDto)
        {
            var contact = await _contactRepository.GetAsync(id);

            if (contact.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Contacts:NotOwner", "You don't have permission to edit this contact");
            }

            contact.RemoveSocialProfile(profileDto.Platform, profileDto.Username);
            await _contactRepository.UpdateAsync(contact);

            return await MapToGetOutputDtoAsync(contact);
        }

        // Private helper methods
        private async Task<ContactDto> MapToGetOutputDtoAsync(Contact contact)
        {
            var dto = ObjectMapper.Map<Contact, ContactDto>(contact);
            // TODO: Load client name if ClientId is set
            return dto;
        }
    }
}
