using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore;

[DependsOn(
    typeof(ActivityTrackingDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ActivityTrackingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ActivityTrackingDbContext>(options =>
        {
            options.AddDefaultRepositories<IActivityTrackingDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
