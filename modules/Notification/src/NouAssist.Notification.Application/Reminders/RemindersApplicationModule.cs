using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.Reminders;

[DependsOn(
    typeof(RemindersDomainModule),
    typeof(RemindersApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class RemindersApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<RemindersApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<RemindersApplicationModule>(validate: true);
        });
    }
}
