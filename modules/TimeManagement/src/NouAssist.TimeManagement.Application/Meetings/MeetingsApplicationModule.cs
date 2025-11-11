using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Meetings;

[DependsOn(
    typeof(MeetingsDomainModule),
    typeof(MeetingsApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class MeetingsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<MeetingsApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MeetingsApplicationModule>(validate: true);
        });
    }
}
