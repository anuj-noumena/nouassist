using Microsoft.Extensions.DependencyInjection;
using NouAssist.EntityFrameworkCore.Projects.Repositories;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.EntityFrameworkCore.Projects
{
    [DependsOn(
        typeof(ProjectsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ProjectsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ProjectsDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Project, EfCoreProjectRepository>();
                options.AddRepository<ProjectSprint, EfCoreProjectSprintRepository>();
                options.AddRepository<ProjectDependency, EfCoreProjectDependencyRepository>();
                options.AddRepository<ProjectSuccessCriterion, EfCoreProjectSuccessCriterionRepository>();
                options.AddRepository<ProjectEvent, EfCoreProjectEventRepository>();
                options.AddRepository<ProjectRequirement, EfCoreProjectRequirementRepository>();

                // If you need default repositories for other entities:
                // options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
