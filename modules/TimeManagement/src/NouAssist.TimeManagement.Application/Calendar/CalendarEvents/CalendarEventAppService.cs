using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.CalendarEvents.Dtos;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.Calendar.Enums;
using NouAssist.TimeManagement.Calendar.Services;
using NouAssist.TimeManagement.Calendar.Specifications;
using NouAssist.TimeManagement.Calendar.ValueObjects;
using NouAssist.TimeManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;
using Volo.Abp.Users;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace NouAssist.TimeManagement.Calendar.CalendarEvents
{
    [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Default)]
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

            var totalCount = await _eventRepository.CountAsync(spec.ToExpression());
// 1. Get the Queryable (Use WithDetailsAsync to load related data like Attendees if needed)
            var queryable = await _eventRepository.WithDetailsAsync();

// 2. Apply Specification
            queryable = queryable.Where(spec.ToExpression());

// 3. Apply Sorting
// Default to "StartAt" if input.Sorting is null
            queryable = queryable.OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "StartAt" : input.Sorting);

// 4. Apply Paging
            queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

// 5. Execute
            var events = await _eventRepository.AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<CalendarEventDto>(
                totalCount,
                ObjectMapper.Map<List<CalendarEvent>, List<CalendarEventDto>>(events)
            );
        }

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Create)]
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

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Edit)]
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

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _eventRepository.DeleteAsync(id);
        }

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Edit)]
        public async Task<CalendarEventDto> RescheduleAsync(Guid id, RescheduleEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            calendarEvent.Reschedule(input.NewStartAt, input.NewEndAt);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Cancel)]
        public async Task<CalendarEventDto> CancelAsync(Guid id, CancelEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            calendarEvent.Cancel(CurrentUser.Id.ToString(), input.CancellationReason);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Edit)]
        public async Task<CalendarEventDto> LinkEntityAsync(Guid id, LinkEntityToEventDto input)
        {
            var calendarEvent = await _eventRepository.GetAsync(id);
            calendarEvent.LinkToEntity(input.EntityId, input.EntityType);

            await _eventRepository.UpdateAsync(calendarEvent);

            return ObjectMapper.Map<CalendarEvent, CalendarEventDto>(calendarEvent);
        }

        [Authorize(TimeManagementPermissions.Calendar.CalendarEvents.Edit)]
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
            ISpecification<CalendarEvent> spec = new BaseSpecification<CalendarEvent>(e => true);

            if (input.CalendarId.HasValue)
            {
                spec = spec.And(new BaseSpecification<CalendarEvent>(e => e.CalendarId == input.CalendarId.Value));
            }

            if (input.StartDate.HasValue && input.EndDate.HasValue)
            {
                spec = spec.And(new EventsInDateRangeSpecification(input.StartDate.Value, input.EndDate.Value));
            }

            if (input.IsRecurring.HasValue)
            {
                spec = spec.And(new BaseSpecification<CalendarEvent>(e => e.IsRecurring == input.IsRecurring.Value));
            }

            if (!input.IncludeCancelled)
            {
                spec = spec.And(new BaseSpecification<CalendarEvent>(e => !e.IsCancelled));
            }

            return spec;
        }
    }
}