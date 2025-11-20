using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore
{
    /// <summary>
    /// EF Core module configuration for CapTable
    /// </summary>
    [DependsOn(
        typeof(CapTableDomainModule), // Depends on the Domain module
        typeof(AbpEntityFrameworkCorePostgreSQLModule) // Use specific DB provider module
    )]
    public class CapTableEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<CapTableDbContext>(options =>
            {
                // Register CUSTOM repositories here
                options.AddRepository<Aggregates.CapTable, EfCoreCapTableRepository>();
                options.AddRepository<SecurityHolder, EfCoreSecurityHolderRepository>();
                options.AddRepository<SecurityIssuance, EfCoreSecurityIssuanceRepository>();
                options.AddRepository<FundingRound, EfCoreFundingRoundRepository>();
                options.AddRepository<OptionPool, EfCoreOptionPoolRepository>();

                // Configure default repositories for entities within aggregates
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(); // Configure to use PostgreSQL
            });
        }
    }
}
