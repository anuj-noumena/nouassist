using Microsoft.EntityFrameworkCore;
using NouAssist.Search.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Search.EntityFrameworkCore
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
