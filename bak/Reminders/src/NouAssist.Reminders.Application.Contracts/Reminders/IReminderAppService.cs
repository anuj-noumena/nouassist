using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Reminders.Reminders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Reminders.Reminders
{
    public interface IReminderAppService : IApplicationService
    {
        Task<ReminderDto> GetAsync(Guid id);
        Task<PagedResultDto<ReminderDto>> GetListAsync(GetReminderListDto input);
        Task<List<ReminderDto>> GetUpcomingRemindersAsync(GetUpcomingRemindersDto input);
        Task<List<ReminderDto>> GetDueRemindersAsync();
        Task<List<ReminderDto>> GetOverdueRemindersAsync();
        Task<int> GetPendingCountAsync();
        Task<ReminderDto> CreateAsync(CreateReminderDto input);
        Task<ReminderDto> CreateRecurringAsync(CreateRecurringReminderDto input);
        Task<ReminderDto> UpdateAsync(Guid id, UpdateReminderDto input);
        Task<ReminderDto> SnoozeAsync(Guid id, SnoozeReminderDto input);
        Task CancelAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
