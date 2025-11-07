using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.Reminders.Aggregates;
using NouAssist.ReflectionAndSupport.Reminders.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.ReflectionAndSupport.Reminders.Data
{
    public interface IReminderRepository : IRepository<Reminder, Guid>
    {
        Task<List<Reminder>> GetDueRemindersAsync(DateTime upToDateTime);
        Task<List<Reminder>> GetOverdueRemindersAsync(DateTime now);
        Task<List<Reminder>> GetUpcomingRemindersAsync(Guid userId, DateTime startTime, DateTime endTime);
        Task<List<Reminder>> GetByEntityAsync(ReminderEntityType entityType, Guid entityId);
        Task<List<Reminder>> GetListAsync(ISpecification<Reminder> spec, int maxResultCount = 50);
        Task<int> GetPendingCountAsync(Guid userId);
    }
}
