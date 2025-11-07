using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore;

[DependsOn(
    typeof(HabitSystemsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class HabitSystemsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<HabitSystemsDbContext>(options =>
        {
            options.AddDefaultRepositories<IHabitSystemsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
