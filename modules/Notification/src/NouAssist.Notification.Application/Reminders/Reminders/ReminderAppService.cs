using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Notification.Reminders.Aggregates;
using NouAssist.Notification.Reminders.Data;
using NouAssist.Notification.Reminders.Permissions;
using NouAssist.Notification.Reminders.Reminders.Dtos;
using NouAssist.Notification.Reminders.Services;
using NouAssist.Notification.Reminders.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Notification.Reminders.Reminders
{
    [Authorize(RemindersPermissions.Reminders.Default)]
    public class ReminderAppService : ApplicationService, IReminderAppService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ReminderSchedulingService _schedulingService;

        public ReminderAppService(
            IReminderRepository reminderRepository,
            ReminderSchedulingService schedulingService)
        {
            _reminderRepository = reminderRepository;
            _schedulingService = schedulingService;
        }

        public async Task<ReminderDto> GetAsync(Guid id)
        {
            var reminder = await _reminderRepository.GetAsync(id);
            
            // Authorization check
            if (reminder.OwnerId != CurrentUser.Id)
            {
                throw new BusinessException("Reminders:NotAuthorized", "You are not authorized to access this reminder.");
            }

            return ObjectMapper.Map<Reminder, ReminderDto>(reminder);
        }

        public async Task<PagedResultDto<ReminderDto>> GetListAsync(GetReminderListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var reminders = await _reminderRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _reminderRepository.CountAsync(spec.ToExpression());

            return new PagedResultDto<ReminderDto>(
                totalCount,
                ObjectMapper.Map<List<Reminder>, List<ReminderDto>>(reminders)
            );
        }

        public async Task<List<ReminderDto>> GetUpcomingRemindersAsync(GetUpcomingRemindersDto input)
        {
            var reminders = await _schedulingService.GetUpcomingRemindersAsync(
                CurrentUser.Id.Value, 
                input.HoursAhead
            );

            return ObjectMapper.Map<List<Reminder>, List<ReminderDto>>(reminders);
        }

        public async Task<List<ReminderDto>> GetDueRemindersAsync()
        {
            var now = Clock.Now;
            var spec = new DueRemindersSpecification(now)
                .And(new Specification<Reminder>(r => r.OwnerId == CurrentUser.Id.Value));

            var reminders = await _reminderRepository.GetListAsync(spec, 50);

            return ObjectMapper.Map<List<Reminder>, List<ReminderDto>>(reminders);
        }

        public async Task<List<ReminderDto>> GetOverdueRemindersAsync()
        {
            var now = Clock.Now;
            var spec = new OverdueRemindersSpecification(now)
                .And(new Specification<Reminder>(r => r.OwnerId == CurrentUser.Id.Value));

            var reminders = await _reminderRepository.GetListAsync(spec, 50);

            return ObjectMapper.Map<List<Reminder>, List<ReminderDto>>(reminders);
        }

        public async Task<int> GetPendingCountAsync()
        {
            return await _reminderRepository.GetPendingCountAsync(CurrentUser.Id.Value);
        }

        [Authorize(RemindersPermissions.Reminders.Create)]
        public async Task<ReminderDto> CreateAsync(CreateReminderDto input)
        {
            // Use domain service to create reminder
            var reminder = _schedulingService.CreateSimpleReminder(
                CurrentUser.Id.Value,
                input.Title,
                input.RemindAt,
                CurrentTenant.Id,
                input.RefEntityType,
                input.RefEntityId,
                input.Priority
            );

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                reminder.SetNotes(input.Notes);
            }

            if (!string.IsNullOrWhiteSpace(input.Method))
            {
                // Method is set in constructor, but we could validate it here
            }

            await _reminderRepository.InsertAsync(reminder);

            return ObjectMapper.Map<Reminder, ReminderDto>(reminder);
        }

        [Authorize(RemindersPermissions.Reminders.Create)]
        public async Task<ReminderDto> CreateRecurringAsync(CreateRecurringReminderDto input)
        {
            // Use domain service to create recurring reminder
            var reminder = _schedulingService.CreateRecurringReminder(
                CurrentUser.Id.Value,
                input.Title,
                input.BaseDateTime,
                input.MinutesBeforeEvent,
                input.RecurrencePattern,
                CurrentTenant.Id,
                input.RefEntityType,
                input.RefEntityId,
                input.RecurrenceInterval,
                input.RecurrenceEndDate,
                input.RecurrenceDaysOfWeek,
                input.Priority
            );

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                reminder.SetNotes(input.Notes);
            }

            await _reminderRepository.InsertAsync(reminder);

            return ObjectMapper.Map<Reminder, ReminderDto>(reminder);
        }

        [Authorize(RemindersPermissions.Reminders.Update)]
        public async Task<ReminderDto> UpdateAsync(Guid id, UpdateReminderDto input)
        {
            var reminder = await _reminderRepository.GetAsync(id);

            // Authorization check
            if (reminder.OwnerId != CurrentUser.Id)
            {
                throw new BusinessException("Reminders:NotAuthorized", "You are not authorized to update this reminder.");
            }

            reminder.UpdateTitle(input.Title);
            reminder.Reschedule(input.RemindAt);
            reminder.UpdatePriority(input.Priority);

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                reminder.SetNotes(input.Notes);
            }

            await _reminderRepository.UpdateAsync(reminder);

            return ObjectMapper.Map<Reminder, ReminderDto>(reminder);
        }

        [Authorize(RemindersPermissions.Reminders.Snooze)]
        public async Task<ReminderDto> SnoozeAsync(Guid id, SnoozeReminderDto input)
        {
            var reminder = await _reminderRepository.GetAsync(id);

            // Authorization check
            if (reminder.OwnerId != CurrentUser.Id)
            {
                throw new BusinessException("Reminders:NotAuthorized", "You are not authorized to snooze this reminder.");
            }

            // Use domain service for snoozing
            _schedulingService.SnoozeReminder(reminder, input.Minutes);

            await _reminderRepository.UpdateAsync(reminder);

            return ObjectMapper.Map<Reminder, ReminderDto>(reminder);
        }

        [Authorize(RemindersPermissions.Reminders.Update)]
        public async Task CancelAsync(Guid id)
        {
            var reminder = await _reminderRepository.GetAsync(id);

            // Authorization check
            if (reminder.OwnerId != CurrentUser.Id)
            {
                throw new BusinessException("Reminders:NotAuthorized", "You are not authorized to cancel this reminder.");
            }

            reminder.Cancel();
            await _reminderRepository.UpdateAsync(reminder);
        }

        [Authorize(RemindersPermissions.Reminders.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var reminder = await _reminderRepository.GetAsync(id);

            // Authorization check
            if (reminder.OwnerId != CurrentUser.Id)
            {
                throw new BusinessException("Reminders:NotAuthorized", "You are not authorized to delete this reminder.");
            }

            await _reminderRepository.DeleteAsync(reminder);
        }

        private ISpecification<Reminder> CreateListSpecification(GetReminderListDto input)
        {
            // Always filter by current user
            var spec = new Specification<Reminder>(r => r.OwnerId == CurrentUser.Id.Value);

            if (input.Status.HasValue)
            {
                spec = spec.And(new Specification<Reminder>(r => r.Status == input.Status.Value));
            }

            if (input.EntityType.HasValue && input.EntityId.HasValue)
            {
                spec = spec.And(new RemindersByEntitySpecification(input.EntityType.Value, input.EntityId.Value));
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(new Specification<Reminder>(r => r.RemindAt >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(new Specification<Reminder>(r => r.RemindAt <= input.ToDate.Value));
            }

            if (input.IsSent.HasValue)
            {
                spec = spec.And(new Specification<Reminder>(r => r.IsSent == input.IsSent.Value));
            }

            if (input.Priority.HasValue)
            {
                spec = spec.And(new Specification<Reminder>(r => r.Priority == input.Priority.Value));
            }

            return spec;
        }
    }
}
