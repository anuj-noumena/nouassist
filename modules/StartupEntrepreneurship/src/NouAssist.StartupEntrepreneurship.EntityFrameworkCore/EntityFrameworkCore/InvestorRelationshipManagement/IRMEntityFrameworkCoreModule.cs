using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore
{
    /// <summary>
    /// EF Core module for IRM bounded context
    /// </summary>
    [DependsOn(
        typeof(IRMDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class IRMEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<IRMDbContext>(options =>
            {
                // Register custom repositories here
                options.AddRepository<InvestorProfile, EfCoreInvestorProfileRepository>();
                options.AddRepository<InvestorUpdate, EfCoreInvestorUpdateRepository>();
                options.AddRepository<DueDiligenceDataRoom, EfCoreDueDiligenceDataRoomRepository>();
                options.AddRepository<InvestorGroup, EfCoreInvestorGroupRepository>();

                // Default repositories for entities not aggregate roots
                // These are optional since entities are accessed through aggregates
                options.AddDefaultRepository<InvestorInteraction>();
                options.AddDefaultRepository<InvestmentRecord>();
                options.AddDefaultRepository<UpdateSection>();
                options.AddDefaultRepository<MetricSnapshot>();
                options.AddDefaultRepository<DistributionLog>();
                options.AddDefaultRepository<EngagementRecord>();
                options.AddDefaultRepository<SecureDocument>();

                // Configure other DbContext options if needed
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(configureAction: npgsqlOptions =>
                {
                    // Configure Npgsql specific options
                    npgsqlOptions.UseQuerySplittingBehavior(Microsoft.EntityFrameworkCore.QuerySplittingBehavior.SplitQuery);
                    npgsqlOptions.EnableServiceProviderCaching();
                    npgsqlOptions.EnableSensitiveDataLogging(false);
                });
            });

            // Configure global query filters
            Configure<AbpEntityOptions>(options =>
            {
                options.Entity<InvestorProfile>(investorOptions =>
                {
                    investorOptions.DefaultWithDetailsFunc = query => query
                        .Include(ip => ip.Interactions)
                        .Include(ip => ip.Investments);
                });

                options.Entity<InvestorUpdate>(updateOptions =>
                {
                    updateOptions.DefaultWithDetailsFunc = query => query
                        .Include(u => u.Sections)
                        .Include(u => u.Metrics)
                        .Include(u => u.DistributionLogs)
                        .Include(u => u.EngagementRecords);
                });

                options.Entity<DueDiligenceDataRoom>(dataRoomOptions =>
                {
                    dataRoomOptions.DefaultWithDetailsFunc = query => query
                        .Include(dr => dr.Documents);
                });
            });
        }
    }
}
