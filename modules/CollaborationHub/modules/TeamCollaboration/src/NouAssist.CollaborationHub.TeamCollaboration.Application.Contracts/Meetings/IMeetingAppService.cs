using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.TeamCollaboration.Meetings.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.TeamCollaboration.Meetings
{
    public interface IMeetingAppService : IApplicationService
    {
        Task<MeetingDto> GetAsync(Guid id);
        Task<PagedResultDto<MeetingDto>> GetListAsync(GetMeetingListDto input);
        Task<PagedResultDto<MeetingDto>> GetMyMeetingsAsync(GetMeetingListDto input);
        Task<List<MeetingDto>> GetUpcomingMeetingsAsync();
        Task<List<MeetingDto>> GetTodaysMeetingsAsync();
        Task<MeetingDto> CreateAsync(CreateMeetingDto input);
        Task<MeetingDto> UpdateAsync(Guid id, UpdateMeetingDto input);
        Task DeleteAsync(Guid id);
        Task<MeetingDto> StartAsync(Guid id);
        Task<MeetingDto> CompleteAsync(Guid id, CompleteMeetingDto input);
        Task<MeetingDto> CancelAsync(Guid id, string reason);
        Task<MeetingDto> AddAttendeeAsync(Guid id, AddMeetingAttendeeDto input);
        Task<MeetingDto> RemoveAttendeeAsync(Guid id, Guid userId);
        Task<MeetingDto> UpdateAttendeeStatusAsync(Guid id, UpdateAttendeeStatusDto input);
        Task<MeetingDto> AddAgendaItemAsync(Guid id, CreateAgendaItemDto input);
        Task<MeetingDto> AddActionItemAsync(Guid id, AddActionItemDto input);
        Task<MeetingDto> CompleteActionItemAsync(Guid id, Guid actionItemId);
    }
}
