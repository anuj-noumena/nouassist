using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using NouAssist.CoreProductivity.TimeManagement.CalendarEvents.Dtos;
using NouAssist.CoreProductivity.TimeManagement.Data;
using NouAssist.CoreProductivity.TimeManagement.Enums;
using NouAssist.CoreProductivity.TimeManagement.Permissions;
using NouAssist.CoreProductivity.TimeManagement.Services;
using NouAssist.CoreProductivity.TimeManagement.Specifications;
using NouAssist.CoreProductivity.TimeManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.TimeManagement.CalendarEvents
{
    [Authorize(CalendarPermissions.CalendarEvents.Default)]
    public class CalendarEventAppService : ApplicationService, ICalendarEventAppService
    {
        private readonly ICalendarEventRepository _eventRepository;
        private readonly ITimeblockRepository _timeblockRepository;
        private readonly CalendarEventSchedulingService _schedulingService;

        public CalendarEventAppService(
            ICalendarEventRepository eventRepository,
            ITimeblockRepository timeblockRepository,
            CalendarEventSchedulingService schedulingService)
        {
            _eventRepository = eventRepository;
            _timeblockRepository = timeblockRepository;
            _schedulingService = schedulingService;
        }

        public async Task<CalendarEventDto> GetAsync(Guid id)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        public async Task<PagedResultDto<CalendarEventDto>> GetListAsync(GetCalendarEventListDto input)
        {
            var spec = CreateListSpecification(input);

            var totalCount = await _eventRepository.GetCountAsync(spec);
            var events = await _eventRepository.GetListAsync(
                spec,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting ?? "StartAt");

            return new PagedResultDto<CalendarEventDto>(
                totalCount,
                ObjectMapper.Map<List<CalendarEvent>, List<CalendarEventDto>>(events)
            );
        }

        [Authorize(CalendarPermissions.CalendarEvents.Create)]
        public async Task<CalendarEventDto> CreateAsync(CreateCalendarEventDto input)
        {
            // Validate scheduling conflicts
            var existingEvents = await _eventRepository.GetEventsInRangeAsync(
                input.CalendarId,
                input.StartAt,
                input.EndAt);

            var existingTimeblocks = await _timeblockRepository.GetTimeblocksInRangeAsync(
                CurrentUser.GetId(),
                input.StartAt,
                input.EndAt);

            var recurrenceRule = input.RecurrenceRule != null
                ? new RecurrenceRule(
                    input.RecurrenceRule.Frequency,
                    input.RecurrenceRule.Interval,
                    input.RecurrenceRule.Until,
                    input.RecurrenceRule.Count,
                    input.RecurrenceRule.ByDay,
                    input.RecurrenceRule.ByMonthDay,
                    input.RecurrenceRule.ByMonth)
                : null;

            var calendarEvent = new CalendarEvent(
                GuidGenerator.Create(),
                input.CalendarId,
                CurrentUser.GetId(),
                input.Title,
                input.StartAt,
                input.EndAt,
                CurrentTenant.Id,
                input.EventLocation,
                input.Description,
                input.IsAllDay,
                input.IsRecurring,
                recurrenceRule,
                input.Timezone);

            calendarEvent.AllowOverlap = input.AllowOverlap;

            if (!input.AllowOverlap)
            {
                var validationResult = _schedulingService.ValidateEventScheduling(
                    calendarEvent,
                    existingEvents,
                    existingTimeblocks);

                if (!validationResult.IsValid)
                {
                    throw new UserFriendlyException(string.Join(", ", validationResult.Errors));
                }
            }

            await _eventRepository.InsertAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(CalendarPermissions.CalendarEvents.Edit)]
        public async Task<CalendarEventDto> UpdateAsync(Guid id, UpdateCalendarEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);

            calendarEvent.SetTitle(input.Title);
            calendarEvent.Description = input.Description;
            calendarEvent.EventLocation = input.EventLocation;
            calendarEvent.SetTimeRange(input.StartAt, input.EndAt, input.IsAllDay);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(CalendarPermissions.CalendarEvents.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _eventRepository.DeleteAsync(id);
        }

        [Authorize(CalendarPermissions.CalendarEvents.Edit)]
        public async Task<CalendarEventDto> RescheduleAsync(Guid id, RescheduleEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            calendarEvent.Reschedule(input.NewStartAt, input.NewEndAt);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(CalendarPermissions.CalendarEvents.Cancel)]
        public async Task<CalendarEventDto> CancelAsync(Guid id, CancelEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            calendarEvent.Cancel(CurrentUser.Id.ToString(), input.CancellationReason);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(CalendarPermissions.CalendarEvents.Edit)]
        public async Task<CalendarEventDto> LinkEntityAsync(Guid id, LinkEntityToEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            calendarEvent.LinkToEntity(input.EntityId, input.EntityType);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(CalendarPermissions.CalendarEvents.Edit)]
        public async Task<CalendarEventDto> UnlinkEntityAsync(Guid id, Guid entityId, string entityType)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            var parsedEntityType = Enum.Parse<LinkedEntityType>(entityType);
            calendarEvent.UnlinkEntity(entityId, parsedEntityType);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        public async Task<List<CalendarEventDto>> GetEventsInRangeAsync(
            Guid calendarId,
            DateTime startDate,
            DateTime endDate)
        {
            var events = await _eventRepository.GetEventsInRangeAsync(calendarId, startDate, endDate);
            return ObjectMapper.Map<List<CalendarEvent>, List<CalendarEventDto>>(events);
        }

        private ISpecification<CalendarEvent> CreateListSpecification(GetCalendarEventListDto input)
        {
            var spec = new Specification<CalendarEvent>(e => true);

            if (input.CalendarId.HasValue)
            {
                spec = spec.And(new Specification<CalendarEvent>(e => e.CalendarId == input.CalendarId.Value));
            }

            if (input.StartDate.HasValue && input.EndDate.HasValue)
            {
                spec = spec.And(new EventsInDateRangeSpecification(input.StartDate.Value, input.EndDate.Value));
            }

            if (input.IsRecurring.HasValue)
            {
                spec = spec.And(new Specification<CalendarEvent>(e => e.IsRecurring == input.IsRecurring.Value));
            }

            if (!input.IncludeCancelled)
            {
                spec = spec.And(new Specification<CalendarEvent>(e => !e.IsCancelled));
            }

            return spec;
        }
    }
}
