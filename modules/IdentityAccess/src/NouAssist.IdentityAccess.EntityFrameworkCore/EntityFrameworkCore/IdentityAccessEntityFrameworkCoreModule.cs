using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.EntityFrameworkCore;

[DependsOn(
    typeof(IdentityAccessDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class IdentityAccessEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityAccessDbContext>(options =>
        {
            options.AddDefaultRepositories<IIdentityAccessDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
