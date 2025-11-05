using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.Meetings
{
    [DependsOn(
        typeof(NouAssistMeetingsDomainModule),
        typeof(NouAssistMeetingsApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class NouAssistMeetingsApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<NouAssistMeetingsApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<NouAssistMeetingsApplicationModule>(validate: true);
            });
        }
    }
}
