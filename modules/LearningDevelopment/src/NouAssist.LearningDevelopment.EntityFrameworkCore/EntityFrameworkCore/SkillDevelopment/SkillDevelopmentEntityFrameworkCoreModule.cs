using Microsoft.Extensions.DependencyInjection;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using NouAssist.SkillDevelopment.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore
{
    [DependsOn(
        typeof(SkillDevelopmentDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class SkillDevelopmentEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SkillDevelopmentDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Skill, EfCoreSkillRepository>();
                options.AddRepository<SkillDevelopmentSession, EfCoreSkillDevelopmentSessionRepository>();
                options.AddRepository<SkillLog, EfCoreSkillLogRepository>();
                options.AddRepository<SkillTree, EfCoreSkillTreeRepository>();
                options.AddRepository<ResourceHistory, EfCoreResourceHistoryRepository>();

                // Add default repositories for aggregate roots if needed
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
