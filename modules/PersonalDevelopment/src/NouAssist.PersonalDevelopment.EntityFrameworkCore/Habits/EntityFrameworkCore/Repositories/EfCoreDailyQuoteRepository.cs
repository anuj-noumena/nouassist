using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Repositories
{
    public class EfCoreDailyQuoteRepository 
        : EfCoreRepository<IHabitsDbContext, DailyQuote, Guid>, IDailyQuoteRepository
    {
        public EfCoreDailyQuoteRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DailyQuote> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(
                    q => q.OwnerId == ownerId && q.Date == normalizedDate,
                    cancellationToken);
        }

        public async Task<List<DailyQuote>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var normalizedStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var normalizedEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            
            var query = await GetQueryableAsync();
            
            return await query
                .Where(q => q.OwnerId == ownerId && 
                           q.Date >= normalizedStart && 
                           q.Date <= normalizedEnd)
                .OrderBy(q => q.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DailyQuote>> GetUnviewedQuotesAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(q => q.OwnerId == ownerId && !q.ViewedAt.HasValue)
                .OrderBy(q => q.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<DailyQuote> GetRandomQuoteAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Simple random selection - in production, consider a better approach
            var quotes = await query
                .Where(q => q.OwnerId == ownerId)
                .ToListAsync(cancellationToken);

            if (!quotes.Any())
            {
                return null;
            }

            var random = new Random();
            var index = random.Next(quotes.Count);
            return quotes[index];
        }
    }
}
