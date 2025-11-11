using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore;

[DependsOn(
    typeof(PersonalDevelopmentDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class PersonalDevelopmentEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PersonalDevelopmentDbContext>(options =>
        {
            options.AddDefaultRepositories<IPersonalDevelopmentDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
