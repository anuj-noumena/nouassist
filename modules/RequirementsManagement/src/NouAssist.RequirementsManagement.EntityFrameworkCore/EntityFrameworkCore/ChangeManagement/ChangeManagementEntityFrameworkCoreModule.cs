using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.ChangeManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore
{
    /// <summary>
    /// Entity Framework Core integration module for Change Management
    /// </summary>
    [DependsOn(
        typeof(ChangeManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ChangeManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register DbContext for dependency injection
            context.Services.AddAbpDbContext<ChangeManagementDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<RequirementChangeRequest, EfCoreRequirementChangeRequestRepository>();

                // Configure entity options if needed
                // options.Entity<RequirementChangeRequest>(entityOptions => { ... });
            });

            // Configure to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
