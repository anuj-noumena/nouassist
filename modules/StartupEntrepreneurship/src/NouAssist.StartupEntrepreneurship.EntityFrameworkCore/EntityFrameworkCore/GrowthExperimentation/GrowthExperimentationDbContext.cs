using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore
{
    [ConnectionStringName(GrowthExperimentationDbProperties.ConnectionStringName)]
    public class GrowthExperimentationDbContext 
        : AbpDbContext<GrowthExperimentationDbContext>, IGrowthExperimentationDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<GrowthExperiment> GrowthExperiments { get; set; }
        public DbSet<MarketingChannelPerformance> MarketingChannels { get; set; }
        public DbSet<ConversionFunnelDefinition> ConversionFunnels { get; set; }

        public GrowthExperimentationDbContext(DbContextOptions<GrowthExperimentationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureGrowthExperimentation();
        }
    }
}
