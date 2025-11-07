using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore;

[DependsOn(
    typeof(CustomFieldsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CustomFieldsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CustomFieldsDbContext>(options =>
        {
            options.AddDefaultRepositories<ICustomFieldsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
