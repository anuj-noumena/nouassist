using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Repositories
{
    public class EfCoreRemindersRepository : IRemindersRepository, ITransientDependency
    {
        private readonly IDbContextProvider<FreelanceOperationsDbContext> _dbContextProvider;
        private readonly IClock _clock;

        public EfCoreRemindersRepository(
            IDbContextProvider<FreelanceOperationsDbContext> dbContextProvider,
            IClock clock)
        {
            _dbContextProvider = dbContextProvider;
            _clock = clock;
        }

        public async Task CreateReminderAsync(
            Guid id,
            string title,
            string message,
            DateTime dueDate,
            Guid entityId,
            string entityType)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            
            var reminder = new Reminder
            {
                Id = id,
                Title = title,
                Message = message,
                DueDate = dueDate,
                EntityId = entityId,
                EntityType = entityType,
                IsCompleted = false,
                CreationTime = _clock.Now
            };
            
            dbContext.Set<Reminder>().Add(reminder);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Dictionary<string, object>>> GetDueRemindersAsync(Guid userId)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var now = _clock.Now;
            
            // Get reminders that are due and not completed
            var reminders = await dbContext.Set<Reminder>()
                .Where(r => r.DueDate <= now && !r.IsCompleted && r.AssignedToUserId == userId)
                .OrderBy(r => r.DueDate)
                .ToListAsync();
                
            return reminders.Select(r => new Dictionary<string, object>
            {
                ["id"] = r.Id,
                ["title"] = r.Title,
                ["message"] = r.Message,
                ["dueDate"] = r.DueDate,
                ["entityId"] = r.EntityId,
                ["entityType"] = r.EntityType,
                ["isCompleted"] = r.IsCompleted
            }).ToList();
        }

        public async Task MarkReminderCompletedAsync(Guid reminderId)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            
            var reminder = await dbContext.Set<Reminder>()
                .FirstOrDefaultAsync(r => r.Id == reminderId);
                
            if (reminder == null)
            {
                throw new EntityNotFoundException(typeof(Reminder), reminderId);
            }
            
            reminder.IsCompleted = true;
            reminder.CompletionTime = _clock.Now;
            
            await dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Entity for storing reminders in the database
    /// </summary>
    [EntityFrameworkCore.Modeling.ConnectionStringName(FreelanceOperationsDbProperties.ConnectionStringName)]
    public class Reminder : Entity<Guid>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DueDate { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionTime { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
