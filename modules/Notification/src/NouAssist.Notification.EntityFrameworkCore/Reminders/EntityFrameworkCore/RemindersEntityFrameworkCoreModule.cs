using Microsoft.Extensions.DependencyInjection;
using NouAssist.Notification.Reminders.Aggregates;
using NouAssist.Notification.Reminders.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.Reminders.EntityFrameworkCore
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
