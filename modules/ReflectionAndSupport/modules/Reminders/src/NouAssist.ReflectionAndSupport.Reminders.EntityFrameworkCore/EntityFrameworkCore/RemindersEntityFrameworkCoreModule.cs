using Microsoft.Extensions.DependencyInjection;
using NouAssist.ReflectionAndSupport.Reminders.Aggregates;
using NouAssist.ReflectionAndSupport.Reminders.Data;
using NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reminders.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistRemindersDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class RemindersEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RemindersDbContext>(options =>
            {
                options.AddRepository<Reminder, EfCoreReminderRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
