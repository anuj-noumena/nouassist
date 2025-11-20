using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore
{
    [ConnectionStringName(MarketIntelligenceDbProperties.ConnectionStringName)]
    public interface IMarketIntelligenceDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root
        DbSet<Competitor> Competitors { get; }
        DbSet<MarketTrend> MarketTrends { get; }
        DbSet<CustomerSegmentProfile> CustomerSegmentProfiles { get; }
        DbSet<SwotAnalysis> SwotAnalyses { get; }
    }
}
