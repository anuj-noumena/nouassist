using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore
{
    [ConnectionStringName(GrowthExperimentationDbProperties.ConnectionStringName)]
    public interface IGrowthExperimentationDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root in this module
        DbSet<GrowthExperiment> GrowthExperiments { get; }
        DbSet<MarketingChannelPerformance> MarketingChannels { get; }
        DbSet<ConversionFunnelDefinition> ConversionFunnels { get; }
    }
}
