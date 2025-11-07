using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore;

[DependsOn(
    typeof(UserProfilesDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class UserProfilesEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<UserProfilesDbContext>(options =>
        {
            options.AddDefaultRepositories<IUserProfilesDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
