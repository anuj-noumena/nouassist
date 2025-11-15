using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.TimeManagement.Calendar.Calendars.Dtos;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.Calendar.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;
using Volo.Abp.Users;

namespace NouAssist.TimeManagement.Calendar.Calendars
{
    [Authorize(CalendarPermissions.Calendars.Default)]
    public class CalendarAppService : ApplicationService, ICalendarAppService
    {
        private readonly ICalendarRepository _calendarRepository;

        public CalendarAppService(ICalendarRepository calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<CalendarDto> GetAsync(Guid id)
        {
            var calendar = await _calendarRepository.GetAsync(id);
            return ObjectMapper.Map<Aggregates.Calendar, CalendarDto>(calendar);
        }

        public async Task<CalendarDto> GetDefaultAsync()
        {
            var calendar = await _calendarRepository.GetDefaultCalendarAsync(CurrentUser.GetId());
            if (calendar == null)
            {
                throw new UserFriendlyException("No default calendar found");
            }
            return ObjectMapper.Map<Aggregates.Calendar, CalendarDto>(calendar);
        }

        public async Task<PagedResultDto<CalendarDto>> GetListAsync(GetCalendarListDto input)
        {
            var spec = CreateListSpecification(input);

            var totalCount = await _calendarRepository.GetCountAsync(spec);
            var calendars = await _calendarRepository.GetListAsync(
                spec,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting);

            return new PagedResultDto<CalendarDto>(
                totalCount,
                ObjectMapper.Map<List<Aggregates.Calendar>, List<CalendarDto>>(calendars)
            );
        }

        [Authorize(CalendarPermissions.Calendars.Create)]
        public async Task<CalendarDto> CreateAsync(CreateCalendarDto input)
        {
            var calendar = new Aggregates.Calendar(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.Title,
                CurrentTenant.Id,
                input.Description,
                input.Color,
                input.Timezone,
                input.IsDefault);

            await _calendarRepository.InsertAsync(calendar);

            return ObjectMapper.Map<Aggregates.Calendar, CalendarDto>(calendar);
        }

        [Authorize(CalendarPermissions.Calendars.Edit)]
        public async Task<CalendarDto> UpdateAsync(Guid id, UpdateCalendarDto input)
        {
            var calendar = await _calendarRepository.GetAsync(id);

            calendar.SetTitle(input.Title);
            calendar.Description = input.Description;
            calendar.Color = input.Color;
            calendar.Timezone = input.Timezone;

            await _calendarRepository.UpdateAsync(calendar);

            return ObjectMapper.Map<Aggregates.Calendar, CalendarDto>(calendar);
        }

        [Authorize(CalendarPermissions.Calendars.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _calendarRepository.DeleteAsync(id);
        }

        [Authorize(CalendarPermissions.Calendars.Edit)]
        public async Task<CalendarDto> SetAsDefaultAsync(Guid id)
        {
            // Remove default from all other calendars
            var existingCalendars = await _calendarRepository.GetActiveCalendarsAsync(CurrentUser.GetId());
            foreach (var cal in existingCalendars.Where(c => c.IsDefault && c.Id != id))
            {
                cal.RemoveDefault();
                await _calendarRepository.UpdateAsync(cal);
            }

            // Set this calendar as default
            var calendar = await _calendarRepository.GetAsync(id);
            calendar.MakeDefault();
            await _calendarRepository.UpdateAsync(calendar);

            return ObjectMapper.Map<Aggregates.Calendar, CalendarDto>(calendar);
        }

        [Authorize(CalendarPermissions.Calendars.Share)]
        public async Task<CalendarDto> ShareAsync(Guid id, ShareCalendarDto input)
        {
            var calendar = await _calendarRepository.GetWithSharesAsync(id);

            var permissionLevel = Enum.Parse<Enums.CalendarPermissionLevel>(input.PermissionLevel);
            calendar.ShareWith(input.SharedWithUserId, permissionLevel);

            await _calendarRepository.UpdateAsync(calendar);

            return ObjectMapper.Map<Aggregates.Calendar, CalendarDto>(calendar);
        }

        [Authorize(CalendarPermissions.Calendars.Share)]
        public async Task RevokeShareAsync(Guid id, Guid sharedWithUserId)
        {
            var calendar = await _calendarRepository.GetWithSharesAsync(id);
            calendar.RevokeShare(sharedWithUserId, CurrentUser.Id.ToString());

            await _calendarRepository.UpdateAsync(calendar);
        }

        private ISpecification<Aggregates.Calendar> CreateListSpecification(GetCalendarListDto input)
        {
            ISpecification<Aggregates.Calendar> spec = new CalendarsByOwnerSpecification(CurrentUser.GetId());

            if (input.IsActive.HasValue)
            {
                spec = spec.And(new BaseSpecification<Aggregates.Calendar>(c => c.IsActive == input.IsActive.Value));
            }

            if (input.IsShared.HasValue)
            {
                spec = spec.And(new BaseSpecification<Aggregates.Calendar>(c => c.IsShared == input.IsShared.Value));
            }

            return spec;
        }
    }
}
