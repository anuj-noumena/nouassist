using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Meetings.EntityFrameworkCore;

[DependsOn(
    typeof(MeetingsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class MeetingsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<MeetingsDbContext>(options =>
        {
            options.AddDefaultRepositories<IMeetingsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
