using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore;

[DependsOn(
    typeof(RemindersDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class RemindersEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RemindersDbContext>(options =>
        {
            options.AddDefaultRepositories<IRemindersDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
