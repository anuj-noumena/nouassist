using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore
{
    /// <summary>
    /// EntityFrameworkCore module for ProjectExecution
    /// </summary>
    [DependsOn(
        typeof(ProjectExecutionDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ProjectExecutionEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<ProjectExecutionDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Project, EfCoreProjectRepository>();
                options.AddRepository<Task, EfCoreTaskRepository>();
                options.AddRepository<TimeLog, EfCoreTimeLogRepository>();
            });
            
            // Configure to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
