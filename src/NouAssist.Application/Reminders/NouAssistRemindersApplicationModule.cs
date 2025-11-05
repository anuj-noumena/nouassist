using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.Reminders
{
    [DependsOn(
        typeof(NouAssistRemindersDomainModule),
        typeof(NouAssistRemindersApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class NouAssistRemindersApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<NouAssistRemindersApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<NouAssistRemindersApplicationModule>(validate: true);
            });
        }
    }
}
