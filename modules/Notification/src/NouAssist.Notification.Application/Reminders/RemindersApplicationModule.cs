using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.Reminders;

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
