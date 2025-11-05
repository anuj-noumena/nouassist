using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Habits.Data
{
    public interface IDailyQuoteRepository : IRepository<DailyQuote, Guid>
    {
        Task<DailyQuote> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<DailyQuote>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<List<DailyQuote>> GetUnviewedQuotesAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default
        );

        Task<DailyQuote> GetRandomQuoteAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default
        );
    }
}
