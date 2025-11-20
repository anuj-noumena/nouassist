using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.Traceability.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore
{
    /// <summary>
    /// EF Core module for TraceabilityManagement module
    /// </summary>
    [DependsOn(
        typeof(TraceabilityManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class TraceabilityManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register DbContext
            context.Services.AddAbpDbContext<TraceabilityManagementDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<TraceLink, EfCoreTraceLinkRepository>();
                
                // Add default repositories for any non-aggregate entities if needed
                // options.AddDefaultRepositories(includeAllEntities: true);
            });

            // Configure to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
