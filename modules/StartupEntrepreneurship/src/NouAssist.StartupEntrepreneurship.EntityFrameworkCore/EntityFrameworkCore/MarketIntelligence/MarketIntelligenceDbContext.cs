using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore
{
    [ConnectionStringName(MarketIntelligenceDbProperties.ConnectionStringName)]
    public class MarketIntelligenceDbContext : AbpDbContext<MarketIntelligenceDbContext>, IMarketIntelligenceDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<MarketTrend> MarketTrends { get; set; }
        public DbSet<CustomerSegmentProfile> CustomerSegmentProfiles { get; set; }
        public DbSet<SwotAnalysis> SwotAnalyses { get; set; }

        public MarketIntelligenceDbContext(DbContextOptions<MarketIntelligenceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureMarketIntelligence();
        }
    }
}
