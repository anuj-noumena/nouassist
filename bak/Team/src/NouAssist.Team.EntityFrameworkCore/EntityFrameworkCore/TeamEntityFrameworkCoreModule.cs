using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Team.EntityFrameworkCore;

[DependsOn(
    typeof(TeamDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TeamEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TeamDbContext>(options =>
        {
            options.AddDefaultRepositories<ITeamDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
