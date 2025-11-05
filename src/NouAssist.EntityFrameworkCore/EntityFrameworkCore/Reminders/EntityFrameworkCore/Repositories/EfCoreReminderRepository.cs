using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Reminders.Aggregates;
using NouAssist.Reminders.Data;
using NouAssist.Reminders.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Reminders.EntityFrameworkCore.Repositories
{
    public class EfCoreReminderRepository 
        : EfCoreRepository<IRemindersDbContext, Reminder, Guid>, IReminderRepository
    {
        public EfCoreReminderRepository(
            IDbContextProvider<IRemindersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Reminder>> GetDueRemindersAsync(DateTime upToDateTime)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => !r.IsSent && 
                           r.RemindAt <= upToDateTime && 
                           r.Status != ReminderStatus.Cancelled &&
                           !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Reminder>> GetOverdueRemindersAsync(DateTime now)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => !r.IsSent && 
                           r.RemindAt < now && 
                           r.Status == ReminderStatus.Pending &&
                           !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Reminder>> GetUpcomingRemindersAsync(
            Guid userId, 
            DateTime startTime, 
            DateTime endTime)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == userId &&
                           !r.IsSent &&
                           r.RemindAt >= startTime &&
                           r.RemindAt <= endTime &&
                           r.Status != ReminderStatus.Cancelled &&
                           !r.IsDeleted)
                .OrderBy(r => r.RemindAt)
                .ToListAsync();
        }

        public async Task<List<Reminder>> GetByEntityAsync(ReminderEntityType entityType, Guid entityId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.RefEntityType == entityType && 
                           r.RefEntityId == entityId &&
                           !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Reminder>> GetListAsync(
            ISpecification<Reminder> spec, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderBy(r => r.RemindAt)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<int> GetPendingCountAsync(Guid userId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == userId && 
                           !r.IsSent && 
                           r.Status == ReminderStatus.Pending &&
                           !r.IsDeleted)
                .CountAsync();
        }
    }
}
