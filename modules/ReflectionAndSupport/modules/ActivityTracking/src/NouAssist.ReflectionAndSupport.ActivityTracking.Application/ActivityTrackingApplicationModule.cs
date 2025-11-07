using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(ActivityTrackingDomainModule),
    typeof(ActivityTrackingApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class ActivityTrackingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ActivityTrackingApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ActivityTrackingApplicationModule>(validate: true);
        });
    }
}
