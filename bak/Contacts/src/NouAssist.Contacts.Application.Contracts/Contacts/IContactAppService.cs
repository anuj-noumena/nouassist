using System;
using System.Threading.Tasks;
using NouAssist.Contacts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Contacts.Contacts
{
    public interface IContactAppService : IApplicationService
    {
        Task<ContactDto> GetAsync(Guid id);
        Task<PagedResultDto<ContactDto>> GetListAsync(GetContactListDto input);
        Task<ContactDto> CreateAsync(CreateContactDto input);
        Task<ContactDto> UpdateAsync(Guid id, UpdateContactDto input);
        Task DeleteAsync(Guid id);

        Task<ContactDto> ArchiveAsync(Guid id);
        Task<ContactDto> UnarchiveAsync(Guid id);

        Task<ContactDto> RecordContactAsync(Guid id);
        Task<ContactDto> RecordMeetingAsync(Guid id);
        Task<ContactDto> ScheduleFollowupAsync(Guid id, DateTime date);

        Task<ContactDto> AddTagAsync(Guid id, string tag);
        Task<ContactDto> RemoveTagAsync(Guid id, string tag);

        Task<ContactDto> AddSocialProfileAsync(Guid id, SocialProfileDto profile);
        Task<ContactDto> RemoveSocialProfileAsync(Guid id, SocialProfileDto profile);
    }
}
