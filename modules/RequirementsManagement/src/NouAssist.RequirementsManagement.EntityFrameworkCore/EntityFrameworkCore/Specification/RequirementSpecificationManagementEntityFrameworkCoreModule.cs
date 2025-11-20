using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.Specification.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore
{
    [DependsOn(
        typeof(RequirementSpecificationManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class RequirementSpecificationManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RequirementSpecificationManagementDbContext>(options =>
            {
                // Add custom repositories
                options.AddRepository<Requirement, EfCoreRequirementRepository>();
                options.AddRepository<SpecificationStructure, EfCoreSpecificationStructureRepository>();

                // Add default repositories for all aggregate roots
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
