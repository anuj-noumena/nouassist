using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.Search.EntityFrameworkCore
{
    [ConnectionStringName(SearchDbProperties.ConnectionStringName)]
    public interface ISearchDbContext : IEfCoreDbContext
    {
        DbSet<SearchHistory> SearchHistories { get; }
        DbSet<SavedSearch> SavedSearches { get; }
        DbSet<PopularSearch> PopularSearches { get; }
        DbSet<SearchAnalytics> SearchAnalytics { get; }
    }
}
