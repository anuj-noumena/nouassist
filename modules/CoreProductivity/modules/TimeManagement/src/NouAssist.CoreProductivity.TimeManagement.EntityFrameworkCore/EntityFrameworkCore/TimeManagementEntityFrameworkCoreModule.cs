using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore;

[DependsOn(
    typeof(TimeManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TimeManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TimeManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<ITimeManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
