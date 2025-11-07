using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore;

[DependsOn(
    typeof(GoalTrackingDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class GoalTrackingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<GoalTrackingDbContext>(options =>
        {
            options.AddDefaultRepositories<IGoalTrackingDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
