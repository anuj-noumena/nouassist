using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.TimeManagement.Meetings.Aggregates;
using NouAssist.TimeManagement.Meetings.Data;
using NouAssist.TimeManagement.Meetings.Meetings.Dtos;
using NouAssist.TimeManagement.Meetings.Permissions;
using NouAssist.TimeManagement.Meetings.Services;
using NouAssist.TimeManagement.Meetings.Specifications;
using NouAssist.TimeManagement.Meetings.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;

namespace NouAssist.TimeManagement.Meetings.Meetings
{
    [Authorize(MeetingsPermissions.Meetings.Default)]
    public class MeetingAppService : ApplicationService, IMeetingAppService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly MeetingSchedulerService _schedulerService;
        private readonly MeetingAttendanceService _attendanceService;

        public MeetingAppService(
            IMeetingRepository meetingRepository,
            MeetingSchedulerService schedulerService,
            MeetingAttendanceService attendanceService)
        {
            _meetingRepository = meetingRepository;
            _schedulerService = schedulerService;
            _attendanceService = attendanceService;
        }

        public async Task<MeetingDto> GetAsync(Guid id)
        {
            var meeting = await _meetingRepository.GetWithFullDetailsAsync(id);
            return await MapToDto(meeting);
        }

        public async Task<PagedResultDto<MeetingDto>> GetListAsync(GetMeetingListDto input)
        {
            var spec = CreateListSpecification(input);

            var meetings = await _meetingRepository.GetListAsync(spec);
            var totalCount = await _meetingRepository.CountAsync(spec);

            var dtos = new List<MeetingDto>();
            foreach (var meeting in meetings)
            {
                dtos.Add(await MapToDto(meeting));
            }

            return new PagedResultDto<MeetingDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<MeetingDto>> GetMyMeetingsAsync(GetMeetingListDto input)
        {
            var spec = CreateListSpecification(input)
                .And(new Specification<Meeting>(m => 
                    m.OwnerId == CurrentUser.Id.Value || 
                    m.OrganizerId == CurrentUser.Id.Value));

            var meetings = await _meetingRepository.GetListAsync(spec);
            var totalCount = await _meetingRepository.CountAsync(spec);

            var dtos = new List<MeetingDto>();
            foreach (var meeting in meetings)
            {
                dtos.Add(await MapToDto(meeting));
            }

            return new PagedResultDto<MeetingDto>(totalCount, dtos);
        }

        public async Task<List<MeetingDto>> GetUpcomingMeetingsAsync()
        {
            var meetings = await _meetingRepository.GetUpcomingMeetingsAsync(CurrentUser.Id.Value);
            
            var dtos = new List<MeetingDto>();
            foreach (var meeting in meetings)
            {
                dtos.Add(await MapToDto(meeting));
            }

            return dtos;
        }

        public async Task<List<MeetingDto>> GetTodaysMeetingsAsync()
        {
            var meetings = await _meetingRepository.GetMeetingsForDateAsync(
                DateTime.UtcNow.Date, 
                CurrentUser.Id.Value);
            
            var dtos = new List<MeetingDto>();
            foreach (var meeting in meetings)
            {
                dtos.Add(await MapToDto(meeting));
            }

            return dtos;
        }

        [Authorize(MeetingsPermissions.Meetings.Create)]
        public async Task<MeetingDto> CreateAsync(CreateMeetingDto input)
        {
            // Load existing meetings to check for conflicts
            var existingMeetings = await _meetingRepository.GetListAsync(
                new MeetingsInDateRangeSpecification(
                    input.StartAt.AddHours(-24), 
                    input.EndAt.AddHours(24)));

            var result = _schedulerService.ScheduleMeeting(
                input.Title,
                input.StartAt,
                input.EndAt,
                CurrentUser.Id.Value,
                input.MeetingType,
                existingMeetings,
                CurrentTenant.Id
            );

            if (result.HasConflicts)
            {
                Logger.LogWarning($"Meeting scheduled with {result.Conflicts.Count} conflicts");
            }

            var meeting = result.Meeting;

            if (!string.IsNullOrEmpty(input.Description))
            {
                // Use a method to set description if available
            }

            if (!string.IsNullOrEmpty(input.Location))
            {
                meeting.SetLocation(input.Location);
            }

            if (!string.IsNullOrEmpty(input.MeetingUrl))
            {
                meeting.SetMeetingUrl(input.MeetingUrl);
            }

            if (input.Frequency != Enums.MeetingFrequency.OneTime)
            {
                meeting.SetRecurring(input.Frequency);
            }

            // Add attendees
            if (input.Attendees != null && input.Attendees.Any())
            {
                var attendees = input.Attendees.Select(a => new MeetingAttendee(
                    a.UserId,
                    a.Email,
                    a.Name,
                    a.Role,
                    Enums.AttendeeStatus.Pending,
                    a.IsRequired,
                    a.UserId == CurrentUser.Id.Value
                )).ToList();

                var attendanceResult = _attendanceService.AddAttendees(meeting, attendees);
                
                if (attendanceResult.HasErrors)
                {
                    Logger.LogWarning($"Some attendees could not be added: {string.Join(", ", attendanceResult.Errors)}");
                }
            }

            // Add agenda items
            if (input.AgendaItems != null && input.AgendaItems.Any())
            {
                var orderIndex = 1;
                foreach (var agendaDto in input.AgendaItems)
                {
                    var agendaItem = new MeetingAgendaItem(
                        agendaDto.Title,
                        agendaDto.Description,
                        agendaDto.DurationMinutes,
                        orderIndex++
                    );
                    meeting.AddAgendaItem(agendaItem);
                }
            }

            await _meetingRepository.InsertAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.Update)]
        public async Task<MeetingDto> UpdateAsync(Guid id, UpdateMeetingDto input)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            // Check permission
            if (!_schedulerService.CanUserEditMeeting(meeting, CurrentUser.Id.Value))
            {
                throw new UserFriendlyException("You don't have permission to edit this meeting");
            }

            meeting.SetTitle(input.Title);
            meeting.SetSchedule(input.StartAt, input.EndAt);
            
            if (!string.IsNullOrEmpty(input.Location))
            {
                meeting.SetLocation(input.Location);
            }

            if (!string.IsNullOrEmpty(input.MeetingUrl))
            {
                meeting.SetMeetingUrl(input.MeetingUrl);
            }

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            if (!_schedulerService.CanUserEditMeeting(meeting, CurrentUser.Id.Value))
            {
                throw new UserFriendlyException("You don't have permission to delete this meeting");
            }

            await _meetingRepository.DeleteAsync(id);
        }

        [Authorize(MeetingsPermissions.Meetings.Start)]
        public async Task<MeetingDto> StartAsync(Guid id)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            meeting.Start();

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.Complete)]
        public async Task<MeetingDto> CompleteAsync(Guid id, CompleteMeetingDto input)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            meeting.Complete(input.Notes, input.Decisions, input.Summary);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.Cancel)]
        public async Task<MeetingDto> CancelAsync(Guid id, string reason)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            if (!_schedulerService.CanUserEditMeeting(meeting, CurrentUser.Id.Value))
            {
                throw new UserFriendlyException("You don't have permission to cancel this meeting");
            }

            meeting.Cancel(reason);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.ManageAttendees)]
        public async Task<MeetingDto> AddAttendeeAsync(Guid id, AddMeetingAttendeeDto input)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            var attendee = new MeetingAttendee(
                input.UserId,
                input.Email,
                input.Name,
                input.Role,
                Enums.AttendeeStatus.Pending,
                input.IsRequired,
                false
            );

            meeting.AddAttendee(attendee);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.ManageAttendees)]
        public async Task<MeetingDto> RemoveAttendeeAsync(Guid id, Guid userId)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            meeting.RemoveAttendee(userId);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        public async Task<MeetingDto> UpdateAttendeeStatusAsync(
            Guid id, 
            UpdateAttendeeStatusDto input)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            meeting.UpdateAttendeeStatus(CurrentUser.Id.Value, input.Status);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        [Authorize(MeetingsPermissions.Meetings.ManageAgenda)]
        public async Task<MeetingDto> AddAgendaItemAsync(Guid id, CreateAgendaItemDto input)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            var agendaItem = new MeetingAgendaItem(
                input.Title,
                input.Description,
                input.DurationMinutes,
                meeting.AgendaItems.Count + 1
            );

            meeting.AddAgendaItem(agendaItem);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        public async Task<MeetingDto> AddActionItemAsync(Guid id, AddActionItemDto input)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            var actionItem = new MeetingActionItem(
                input.Description,
                input.AssignedToUserId,
                input.DueDate
            );

            meeting.AddActionItem(actionItem);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        public async Task<MeetingDto> CompleteActionItemAsync(Guid id, Guid actionItemId)
        {
            var meeting = await _meetingRepository.GetAsync(id);

            meeting.CompleteActionItem(actionItemId);

            await _meetingRepository.UpdateAsync(meeting);

            return await MapToDto(meeting);
        }

        private ISpecification<Meeting> CreateListSpecification(GetMeetingListDto input)
        {
            var spec = Specification.None<Meeting>();

            if (input.Status.HasValue)
            {
                spec = spec.And(new Specification<Meeting>(m => m.Status == input.Status.Value));
            }

            if (input.MeetingType.HasValue)
            {
                spec = spec.And(new MeetingsByTypeSpecification(input.MeetingType.Value));
            }

            if (input.ProjectId.HasValue)
            {
                spec = spec.And(new MeetingsByProjectSpecification(input.ProjectId.Value));
            }

            if (input.FromDate.HasValue && input.ToDate.HasValue)
            {
                spec = spec.And(new MeetingsInDateRangeSpecification(
                    input.FromDate.Value, 
                    input.ToDate.Value));
            }

            if (input.OnlyUpcoming.HasValue && input.OnlyUpcoming.Value)
            {
                spec = spec.And(new UpcomingMeetingsSpecification());
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                spec = spec.And(new Specification<Meeting>(
                    m => m.Title.Contains(input.Filter) || 
                         (m.Description != null && m.Description.Contains(input.Filter))));
            }

            return spec;
        }

        private async Task<MeetingDto> MapToDto(Meeting meeting)
        {
            var dto = ObjectMapper.Map<Meeting, MeetingDto>(meeting);

            // In real scenario, load user names from identity service
            dto.OwnerName = $"User-{meeting.OwnerId}";
            if (meeting.OrganizerId.HasValue)
            {
                dto.OrganizerName = $"User-{meeting.OrganizerId.Value}";
            }

            return dto;
        }
    }
}
