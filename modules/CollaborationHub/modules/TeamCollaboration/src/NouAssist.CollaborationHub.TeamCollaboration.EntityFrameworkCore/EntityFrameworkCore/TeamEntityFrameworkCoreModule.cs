using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore;

[DependsOn(
    typeof(TeamCollaborationDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TeamCollaborationEntityFrameworkCoreModule : AbpModule
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
