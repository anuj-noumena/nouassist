using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore
{
    [DependsOn(
        typeof(FreelanceOperationsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class FreelanceOperationsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<FreelanceOperationsDbContext>(options =>
            {
                // Register CUSTOM repositories here
                options.AddRepository<FreelanceProject, EfCoreFreelanceProjectRepository>();
                options.AddRepository<FreelanceClient, EfCoreClientRepository>();
                options.AddRepository<Proposal, EfCoreProposalRepository>();
                options.AddRepository<UpworkEngagement, EfCoreUpworkEngagementRepository>();
            });

            // Configure EF Core options for PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
