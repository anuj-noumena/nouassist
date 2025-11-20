using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore
{
    /// <summary>
    /// EntityFrameworkCore module for the VersionControl bounded context
    /// </summary>
    [DependsOn(
        typeof(VersionControlDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class VersionControlEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext
            context.Services.AddAbpDbContext<VersionControlDbContext>(options =>
            {
                // Add custom repositories
                options.AddRepository<VersionedRequirementHistory, EfCoreVersionedRequirementHistoryRepository>();
                options.AddRepository<BaselineDefinition, EfCoreBaselineDefinitionRepository>();
                options.AddRepository<RequirementBranch, EfCoreRequirementBranchRepository>();
            });

            // Configure EF Core to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
