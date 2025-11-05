using Microsoft.Extensions.DependencyInjection;
using NouAssist.Team.Aggregates;
using NouAssist.Team.Data;
using NouAssist.Team.Entities;
using NouAssist.Team.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Team.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistTeamDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class NouAssistTeamEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TeamDbContext>(options =>
            {
                options.AddRepository<Team.Aggregates.Team, EfCoreTeamRepository>();
                options.AddRepository<Role, EfCoreRoleRepository>();
                options.AddRepository<UserRole, EfCoreUserRoleRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
